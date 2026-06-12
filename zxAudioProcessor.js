class ZxBoxAudioProcessor extends AudioWorkletProcessor {
    constructor(options) {
        super();
        const processorOptions = options?.processorOptions ?? {};
        this.lowWatermarkSamples = Math.max(128, processorOptions.lowWatermarkSamples ?? 960);
        this.queue = [];
        this.currentChunk = null;
        this.currentOffset = 0;
        this.queuedSamples = 0;
        this.underrunCount = 0;
        this.needDataPosted = false;

        this.port.onmessage = (event) => {
            const data = event.data;
            if (!data) {
                return;
            }

            if (data.type === "clear") {
                this.queue = [];
                this.currentChunk = null;
                this.currentOffset = 0;
                this.queuedSamples = 0;
                this.underrunCount = 0;
                this.needDataPosted = false;
                return;
            }

            if (data.type !== "samples") {
                return;
            }

            const samples = new Float32Array(data.buffer);
            if (samples.length === 0) {
                return;
            }

            this.queue.push(samples);
            this.queuedSamples += samples.length;
            if (this.queuedSamples > this.lowWatermarkSamples) {
                this.needDataPosted = false;
            }
        };
    }

    process(inputs, outputs) {
        const output = outputs[0][0];
        let writeIndex = 0;

        while (writeIndex < output.length) {
            if (this.currentChunk === null || this.currentOffset >= this.currentChunk.length) {
                this.currentChunk = this.queue.length > 0 ? this.queue.shift() : null;
                this.currentOffset = 0;
            }

            if (this.currentChunk === null) {
                output.fill(0, writeIndex);
                if (writeIndex === 0 || output.length !== 0) {
                    this.underrunCount++;
                }
                this.queuedSamples = 0;
                break;
            }

            const remainingInChunk = this.currentChunk.length - this.currentOffset;
            const remainingOutput = output.length - writeIndex;
            const copyLength = Math.min(remainingInChunk, remainingOutput);
            output.set(this.currentChunk.subarray(this.currentOffset, this.currentOffset + copyLength), writeIndex);
            this.currentOffset += copyLength;
            writeIndex += copyLength;
            this.queuedSamples -= copyLength;
        }

        if (this.queuedSamples <= this.lowWatermarkSamples && !this.needDataPosted) {
            this.needDataPosted = true;
            this.port.postMessage({
                type: "needData",
                queuedSamples: this.queuedSamples,
                underrunCount: this.underrunCount
            });
        }

        return true;
    }
}

registerProcessor("zxbox-audio-processor", ZxBoxAudioProcessor);
