function createPerfMetrics() {
    return {
        enqueueCount: 0,
        totalEnqueueMs: 0,
        maxEnqueueMs: 0,
        underrunCount: 0,
        maxQueuedAheadSeconds: 0,
        samplesPushed: 0
    };
}

class ZxBoxAudioController {
    constructor(requestedSampleRate, gain, targetQueuedSeconds, lowWatermarkSeconds) {
        this.requestedSampleRate = requestedSampleRate;
        this.targetQueuedSeconds = targetQueuedSeconds;
        this.lowWatermarkSeconds = lowWatermarkSeconds;
        this.context = null;
        this.gainNode = null;
        this.workletNode = null;
        this.sampleRate = requestedSampleRate;
        this.audioDriven = false;
        this.perfMetrics = createPerfMetrics();
        this.queuedSamples = 0;
        this.instanceId = 0;
        this.callbackInProgress = false;
        this.callbackQueued = false;
        this.resumeRequested = false;
        this.nextStartTime = 0;
        this.bufferedUntilTime = 0;
        this.pumpHandle = 0;
        this.resumeHandler = () => { void this.resume(); };
    }

    async initialize() {
        if (this.context) {
            return this;
        }

        try {
            this.context = new AudioContext({
                sampleRate: this.requestedSampleRate,
                latencyHint: "interactive"
            });
        } catch {
            this.context = new AudioContext();
        }

        this.sampleRate = this.context.sampleRate;
        this.bufferedUntilTime = this.context.currentTime;
        this.gainNode = new GainNode(this.context, { gain: 0.35 });
        this.gainNode.gain.value = 0.35;
        this.gainNode.connect(this.context.destination);
        this.context.onstatechange = () => {
            if (this.context.state === "running") {
                this.requestPump();
            }
        };

        this.installResumeListeners();

        try {
            await this.context.audioWorklet.addModule(new URL("./zxAudioProcessor.js", import.meta.url));
            this.workletNode = new AudioWorkletNode(this.context, "zxbox-audio-processor", {
                numberOfInputs: 0,
                numberOfOutputs: 1,
                outputChannelCount: [1],
                processorOptions: {
                    lowWatermarkSamples: Math.max(128, Math.round(this.sampleRate * this.lowWatermarkSeconds))
                }
            });
            this.workletNode.port.onmessage = (event) => this.handleWorkletMessage(event.data);
            this.workletNode.connect(this.gainNode);
            this.audioDriven = true;
        } catch {
            this.audioDriven = false;
        }

        if (this.context.state === "running") {
            this.requestPump();
        } else {
            void this.resume();
        }

        return this;
    }

    async resume() {
        if (!this.context || this.resumeRequested) {
            return false;
        }

        this.resumeRequested = true;
        try {
            if (this.context.state === "suspended") {
                await this.context.resume();
            }
            if (this.context.state === "running") {
                this.requestPump();
                return true;
            }
            return false;
        } catch {
            return false;
        } finally {
            this.resumeRequested = false;
        }
    }

    installResumeListeners() {
        window.addEventListener("pointerdown", this.resumeHandler, { passive: true });
        window.addEventListener("keydown", this.resumeHandler, { passive: true });
        window.addEventListener("touchstart", this.resumeHandler, { passive: true });
    }

    removeResumeListeners() {
        window.removeEventListener("pointerdown", this.resumeHandler);
        window.removeEventListener("keydown", this.resumeHandler);
        window.removeEventListener("touchstart", this.resumeHandler);
    }

    setGain(gain) {
        if (this.gainNode) {
            this.gainNode.gain.value = gain;
        }
    }

    pushFrame(floatData) {
        if (!this.context || !this.gainNode) {
            return;
        }

        const start = performance.now();
        const source = toFloat32ArrayView(floatData);
        const copy = new Float32Array(source.length);
        copy.set(source);
        const sampleCount = copy.length;

        if (this.audioDriven && this.workletNode) {
            const currentTime = this.context.currentTime;
            if (this.bufferedUntilTime < currentTime) {
                if (this.bufferedUntilTime !== 0) {
                    this.perfMetrics.underrunCount++;
                }
                this.bufferedUntilTime = currentTime;
            }

            this.workletNode.port.postMessage({ type: "samples", buffer: copy.buffer }, [copy.buffer]);
            this.bufferedUntilTime += sampleCount / this.sampleRate;
            this.updateQueuedAheadMetric();
        } else {
            if (this.context.state === "suspended") {
                void this.resume();
            }

            const audioBuffer = this.context.createBuffer(1, sampleCount, this.context.sampleRate);
            audioBuffer.getChannelData(0).set(copy);

            const source = new AudioBufferSourceNode(this.context, { buffer: audioBuffer });
            source.connect(this.gainNode);

            const frameDuration = audioBuffer.length / audioBuffer.sampleRate;
            if (this.nextStartTime !== 0 && this.nextStartTime < this.context.currentTime) {
                this.perfMetrics.underrunCount++;
            }

            if (this.nextStartTime === 0 || this.nextStartTime < this.context.currentTime) {
                this.nextStartTime = this.context.currentTime + frameDuration;
            }

            const queuedAheadSeconds = Math.max(0, this.nextStartTime - this.context.currentTime);
            if (queuedAheadSeconds > this.perfMetrics.maxQueuedAheadSeconds) {
                this.perfMetrics.maxQueuedAheadSeconds = queuedAheadSeconds;
            }

            source.start(this.nextStartTime);
            this.nextStartTime += frameDuration;
        }

        const elapsed = performance.now() - start;
        this.perfMetrics.enqueueCount++;
        this.perfMetrics.totalEnqueueMs += elapsed;
        this.perfMetrics.samplesPushed += sampleCount;
        if (elapsed > this.perfMetrics.maxEnqueueMs) {
            this.perfMetrics.maxEnqueueMs = elapsed;
        }
    }

    startPump(instanceId) {
        this.instanceId = instanceId;
        if (this.pumpHandle !== 0) {
            clearInterval(this.pumpHandle);
        }
        this.pumpHandle = window.setInterval(() => this.requestPump(), 5);
        this.requestPump();
    }

    stopPump() {
        this.instanceId = 0;
        this.callbackQueued = false;
        this.callbackInProgress = false;
        if (this.pumpHandle !== 0) {
            clearInterval(this.pumpHandle);
            this.pumpHandle = 0;
        }
    }

    requestPump() {
        if (!this.audioDriven || !this.instanceId || !this.context || this.context.state !== "running") {
            return;
        }

        if (this.getQueuedSeconds() > this.lowWatermarkSeconds) {
            return;
        }

        if (this.callbackInProgress) {
            this.callbackQueued = true;
            return;
        }

        this.callbackInProgress = true;
        try {
            globalThis.DotNet.invokeMethod("ZXBox.Blazor", "RequestAudioPump", this.instanceId);
        } finally {
            this.callbackInProgress = false;
            if (this.callbackQueued) {
                this.callbackQueued = false;
                queueMicrotask(() => this.requestPump());
            }
        }
    }

    handleWorkletMessage(message) {
        if (!message || message.type !== "needData") {
            return;
        }

        this.queuedSamples = message.queuedSamples ?? 0;
        this.perfMetrics.underrunCount = Math.max(this.perfMetrics.underrunCount, message.underrunCount ?? 0);
    }

    updateQueuedAheadMetric() {
        const queuedAheadSeconds = this.getQueuedSeconds();
        if (queuedAheadSeconds > this.perfMetrics.maxQueuedAheadSeconds) {
            this.perfMetrics.maxQueuedAheadSeconds = queuedAheadSeconds;
        }
    }

    getQueuedSeconds() {
        if (!this.context) {
            return 0;
        }

        if (this.audioDriven) {
            return Math.max(0, this.bufferedUntilTime - this.context.currentTime);
        }

        return Math.max(0, this.nextStartTime - this.context.currentTime);
    }

    resetPerformanceMetrics() {
        this.perfMetrics = createPerfMetrics();
        this.nextStartTime = 0;
    }

    resetForMeasurement() {
        this.resetPerformanceMetrics();
        this.queuedSamples = 0;
        this.bufferedUntilTime = this.context ? this.context.currentTime : 0;
        if (this.workletNode) {
            this.workletNode.port.postMessage({ type: "clear" });
        }
    }

    getPerformanceMetrics() {
        return {
            enqueueCount: this.perfMetrics.enqueueCount,
            averageEnqueueMs: this.perfMetrics.enqueueCount === 0 ? 0 : this.perfMetrics.totalEnqueueMs / this.perfMetrics.enqueueCount,
            maxEnqueueMs: this.perfMetrics.maxEnqueueMs,
            underrunCount: this.perfMetrics.underrunCount,
            maxQueuedAheadSeconds: this.perfMetrics.maxQueuedAheadSeconds,
            contextState: this.context ? this.context.state : "uninitialized",
            currentTimeSeconds: this.context ? this.context.currentTime : 0,
            sampleRate: this.sampleRate,
            samplesPushed: this.perfMetrics.samplesPushed,
            generatedSeconds: this.sampleRate <= 0 ? 0 : this.perfMetrics.samplesPushed / this.sampleRate,
            audioDriven: this.audioDriven
        };
    }

    async dispose() {
        this.stopPump();
        this.removeResumeListeners();

        if (this.workletNode) {
            this.workletNode.port.onmessage = null;
            this.workletNode.disconnect();
            this.workletNode = null;
        }

        if (this.gainNode) {
            this.gainNode.disconnect();
            this.gainNode = null;
        }

        if (this.context) {
            try {
                await this.context.close();
            } catch {
            }
            this.context = null;
        }
    }
}

function toFloat32ArrayView(floatData) {
    if (floatData instanceof Float32Array) {
        return floatData;
    }

    if (floatData?.buffer instanceof ArrayBuffer && typeof floatData.byteLength === "number") {
        return new Float32Array(
            floatData.buffer,
            floatData.byteOffset ?? 0,
            floatData.byteLength / Float32Array.BYTES_PER_ELEMENT);
    }

    return Float32Array.from(floatData ?? []);
}

export async function createAudioController(requestedSampleRate, gain, targetQueuedSeconds, lowWatermarkSeconds) {
    const controller = new ZxBoxAudioController(requestedSampleRate, gain, targetQueuedSeconds, lowWatermarkSeconds);
    await controller.initialize();
    controller.setGain(gain);
    globalThis.__zxboxLastAudioController = controller;
    return controller;
}

export async function disposeAudioController(controller) {
    if (controller) {
        if (globalThis.__zxboxLastAudioController === controller) {
            globalThis.__zxboxLastAudioController = null;
        }
        await controller.dispose();
    }
}

export async function resumeAudioController(controller) {
    if (!controller) {
        return false;
    }

    return await controller.resume();
}

export function startAudioPump(controller, instanceId) {
    controller?.startPump(instanceId);
}

export function stopAudioPump(controller) {
    controller?.stopPump();
}

export function pushAudioFrame(controller, floatData) {
    controller?.pushFrame(floatData);
}

export function pushPendingAudioFrame(controller, sampleCount) {
    if (!controller || !controller.pendingFrameBytes) {
        return;
    }

    const bytes = controller.pendingFrameBytes;
    const floatView = new Float32Array(
        bytes.buffer,
        bytes.byteOffset ?? 0,
        sampleCount);
    controller.pushFrame(floatView);
}

export function getAudioSampleRate(controller) {
    return controller?.sampleRate ?? 0;
}

export function getQueuedSeconds(controller) {
    return controller?.getQueuedSeconds() ?? 0;
}

export function isAudioDriven(controller) {
    return controller?.audioDriven === true;
}

export function resetPerformanceMetrics(controller) {
    controller?.resetPerformanceMetrics();
}

export function resetControllerForMeasurement(controller) {
    controller?.resetForMeasurement();
}

export function getPerformanceMetricsJson(controller) {
    return JSON.stringify(controller?.getPerformanceMetrics() ?? {
        enqueueCount: 0,
        averageEnqueueMs: 0,
        maxEnqueueMs: 0,
        underrunCount: 0,
        maxQueuedAheadSeconds: 0,
        contextState: "uninitialized",
        currentTimeSeconds: 0,
        sampleRate: 0,
        samplesPushed: 0,
        generatedSeconds: 0,
        audioDriven: false
    });
}
