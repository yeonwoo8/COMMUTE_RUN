"""Generates retro SFX and two chiptune BGM loops as 16-bit mono WAVs (stdlib only)."""
import math
import random
import struct
import sys
import wave
from pathlib import Path

RATE = 22050
random.seed(3)


def midi(n):
    return 440.0 * 2 ** ((n - 69) / 12)


def square(phase, duty=0.5):
    return 1.0 if (phase % 1.0) < duty else -1.0


def triangle(phase):
    p = phase % 1.0
    return 4 * p - 1 if p < 0.5 else 3 - 4 * p


def tone(freq_fn, seconds, wave_fn, volume, attack=0.003, decay=None, duty=0.5):
    """freq_fn(t) -> Hz. decay: exponential time constant, None = flat with short release."""
    n = int(seconds * RATE)
    out, phase = [], 0.0
    for i in range(n):
        t = i / RATE
        phase += freq_fn(t) / RATE
        env = min(1.0, t / attack) if attack else 1.0
        env *= math.exp(-t / decay) if decay else 1.0
        env *= min(1.0, (seconds - t) / 0.01)  # click-free tail
        sample = wave_fn(phase, duty) if wave_fn is square else wave_fn(phase)
        out.append(sample * env * volume)
    return out


def noise(seconds, volume, decay, smooth=1):
    n = int(seconds * RATE)
    raw = [random.uniform(-1, 1) for _ in range(n)]
    if smooth > 1:
        raw = [sum(raw[max(0, i - smooth):i + 1]) / smooth for i in range(n)]
    return [raw[i] * math.exp(-(i / RATE) / decay) * volume * min(1.0, (seconds - i / RATE) / 0.005) for i in range(n)]


def concat(*parts):
    out = []
    for p in parts:
        out.extend(p)
    return out


def mix(*parts):
    out = [0.0] * max(len(p) for p in parts)
    for p in parts:
        for i, s in enumerate(p):
            out[i] += s
    return out


def notes(seq, step, volume, wave_fn=square, duty=0.5, decay=None):
    return concat(*[tone(lambda t, f=midi(m): f, step, wave_fn, volume, decay=decay, duty=duty) for m in seq])


def save(path, samples):
    peak = max(1e-9, max(abs(s) for s in samples))
    gain = 1.0 if peak <= 0.95 else 0.95 / peak
    with wave.open(str(path), "wb") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(RATE)
        w.writeframes(b"".join(struct.pack("<h", int(max(-1, min(1, s * gain)) * 32767)) for s in samples))


def sfx():
    return {
        "Gain": concat(tone(lambda t: 988, 0.05, square, 0.35),
                       tone(lambda t: 1319, 0.16, square, 0.35, decay=0.07)),
        "Loss": tone(lambda t: (220 - 130 * t / 0.35) * (1 + 0.04 * math.sin(t * 2 * math.pi * 14)),
                     0.35, square, 0.35, decay=0.25),
        "ComboUp": concat(notes([72, 76, 79], 0.055, 0.3, duty=0.25),
                          tone(lambda t: midi(84), 0.18, square, 0.3, decay=0.08, duty=0.25)),
        "Fever": concat(notes([72, 76, 79, 84, 88, 91], 0.045, 0.3, duty=0.25),
                        *[tone(lambda t, f=midi(m): f, 0.05, square, 0.28, duty=0.25) for m in [96, 91] * 3],
                        tone(lambda t: midi(96), 0.2, square, 0.28, decay=0.1, duty=0.25)),
        "SlotTick": tone(lambda t: 1800, 0.018, square, 0.12, decay=0.006),
        "ReelStop": mix(noise(0.03, 0.25, 0.01), tone(lambda t: 140, 0.08, square, 0.35, decay=0.03)),
        "Jackpot": concat(notes([79, 84, 88, 91], 0.08, 0.3, duty=0.25),
                          mix(tone(lambda t: midi(96) * (1 + 0.01 * math.sin(t * 2 * math.pi * 7)), 0.6, square, 0.25, decay=0.35, duty=0.25),
                              tone(lambda t: midi(88), 0.6, triangle, 0.25, decay=0.35))),
        "Pop": tone(lambda t: 400 + 800 * (t / 0.09), 0.09, triangle, 0.5, decay=0.04),
        "Bump": mix(noise(0.12, 0.45, 0.04, smooth=4),
                    tone(lambda t: 150 - 100 * t / 0.2, 0.2, triangle, 0.6, decay=0.07)),
        "Click": tone(lambda t: 1200, 0.03, square, 0.2, decay=0.01),
        "Door": concat(tone(lambda t: (260 + 160 * t / 0.3) * (1 + 0.08 * square(t * 28)), 0.3, triangle, 0.25),
                       mix(noise(0.08, 0.3, 0.02, smooth=6), tone(lambda t: 90, 0.1, triangle, 0.5, decay=0.03))),
    }


def song(bpm, melody, lead_volume, lead_duty, lead_step_beats, chords, arp, bass_style, drums):
    """melody: MIDI per step, -1 = hold previous, None = rest. chords: per bar (root, [arp tones])."""
    beat = 60.0 / bpm
    bars = len(chords)
    length = int(bars * 4 * beat * RATE)
    out = [0.0] * length

    def put(start, samples):
        i0 = int(start * RATE)
        for i, s in enumerate(samples):
            if i0 + i < length:
                out[i0 + i] += s

    step = lead_step_beats * beat
    i = 0
    while i < len(melody):
        m = melody[i]
        hold = 1
        while i + hold < len(melody) and melody[i + hold] == -1:
            hold += 1
        if m is not None:
            put(i * step, tone(lambda t, f=midi(m): f, hold * step * 0.95, square, lead_volume, decay=hold * step * 1.5, duty=lead_duty))
        i += hold

    for bar, (root, tones) in enumerate(chords):
        t0 = bar * 4 * beat
        if bass_style == "bounce":
            for e in range(8):
                put(t0 + e * beat / 2, tone(lambda t, f=midi(root + (12 if e % 2 else 0)): f, beat / 2 * 0.9, triangle, 0.3))
        else:
            for h in range(2):
                put(t0 + h * 2 * beat, tone(lambda t, f=midi(root): f, 2 * beat * 0.95, triangle, 0.3, decay=2 * beat))
        if arp:
            for e in range(8):
                put(t0 + e * beat / 2, tone(lambda t, f=midi(tones[e % len(tones)]): f, beat / 2 * 0.9, square, 0.05, decay=beat / 3, duty=0.125))
        if drums:
            for b in range(4):
                if b % 2 == 0:
                    put(t0 + b * beat, tone(lambda t: 120 - 80 * t / 0.12, 0.12, triangle, 0.5, decay=0.05))
                else:
                    put(t0 + b * beat, noise(0.1, 0.18, 0.04, smooth=2))
                put(t0 + b * beat + beat / 2, noise(0.03, 0.08, 0.01))
    return out


def music():
    H = -1
    commute = [
        72, H, 76, 79, 76, H, 72, 74,
        76, H, 72, 69, 72, H, H, H,
        77, H, 77, 76, 77, H, 81, H,
        79, H, 77, 76, 74, H, H, H,
        72, H, 76, 79, 84, H, 79, H,
        81, H, 79, 76, 72, H, 76, H,
        77, 76, 74, 72, 74, H, 77, H,
        79, H, 74, H, 71, H, 67, H,
    ]
    home = [
        76, H, 79, 77,
        76, H, 72, H,
        74, H, 77, 76,
        74, H, H, None,
        72, 76, 79, H,
        81, H, 79, 76,
        77, H, 74, 71,
        74, H, H, None,
    ]
    c, am, f, g, dm = (48, [60, 64, 67]), (45, [57, 60, 64]), (41, [53, 57, 60]), (43, [55, 59, 62]), (50, [62, 65, 69])
    return {
        "BgmCommute": song(140, commute, 0.16, 0.25, 0.5, [c, am, f, g] * 2, arp=False, bass_style="bounce", drums=True),
        "BgmHome": song(90, home, 0.12, 0.5, 1.0, [c, am, dm, g] * 2, arp=True, bass_style="soft", drums=False),
    }


def rain(seconds=6.0, fade=0.5):
    """Seamless rain loop: filtered noise hiss plus scattered drip ticks, own seed so other sounds stay unchanged."""
    rng = random.Random(33)
    n, f = int(seconds * RATE), int(fade * RATE)
    total = n + f
    out, low, band = [], 0.0, 0.0
    for _ in range(total):
        x = rng.uniform(-1, 1)
        low += 0.08 * (x - low)
        band += 0.5 * (x - band)
        out.append(low * 0.9 + (band - low) * 0.12)
    for _ in range(int(seconds * 30)):
        start, freq = rng.randrange(total - 400), rng.uniform(1800, 4200)
        vol = rng.uniform(0.04, 0.12)
        for i in range(300):
            out[start + i] += math.sin(2 * math.pi * freq * i / RATE) * math.exp(-i / 60) * vol
    # Fold the extra tail over the head so the loop point is seamless.
    for i in range(f):
        k = i / f
        out[i] = out[i] * k + out[n + i] * (1 - k)
    return out[:n]


if __name__ == "__main__":
    out_dir = Path(sys.argv[1])
    out_dir.mkdir(parents=True, exist_ok=True)
    only = set(sys.argv[2:])
    sounds = {"Rain": rain()} if only == {"Rain"} else {**sfx(), **music(), "Rain": rain()}
    for name, samples in sounds.items():
        if only and name not in only:
            continue
        save(out_dir / f"{name}.wav", samples)
        print(name, f"{len(samples) / RATE:.2f}s")
