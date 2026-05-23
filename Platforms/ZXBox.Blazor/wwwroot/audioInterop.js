let audioContext;
let gainNode;
let nextStartTime = 0;
let bufferSize = 960;

function ensureAudio() {
    if (!audioContext) {
        audioContext = new AudioContext();
        gainNode = new GainNode(audioContext, { gain: 0.35 });
        gainNode.connect(audioContext.destination);
    }
}

export function initializeAudio(size, gain) {
    bufferSize = size;
    ensureAudio();
    gainNode.gain.value = gain;
}

export function addAudioBuffer(floatData) {
    ensureAudio();

    if (audioContext.state === "suspended") {
        void audioContext.resume();
    }

    const audioBuffer = audioContext.createBuffer(1, bufferSize, audioContext.sampleRate);
    audioBuffer.getChannelData(0).set(floatData);

    const source = new AudioBufferSourceNode(audioContext, { buffer: audioBuffer });
    source.connect(gainNode);

    const frameDuration = audioBuffer.length / audioBuffer.sampleRate;
    if (nextStartTime === 0 || nextStartTime < audioContext.currentTime) {
        nextStartTime = audioContext.currentTime + frameDuration;
    }

    source.start(nextStartTime);
    nextStartTime += frameDuration;
}
