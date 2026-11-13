using UnityEngine;

// Luck, combo and music cues, all driven by run and combo events.
public class GameAudio : MonoBehaviour
{
    // Each gain in a streak climbs one note of the major scale: do, re, mi...
    static readonly int[] ScaleSemitones = { 0, 2, 4, 5, 7, 9, 11, 12, 14 };

    [SerializeField] CommuteRun run;
    [SerializeField] ComboMeter combo;
    [SerializeField] GameConfig config;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioSource luckVoice;
    [SerializeField] AudioSource music;
    [SerializeField] AudioClip gain;
    [SerializeField] AudioClip loss;
    [SerializeField] AudioClip comboUp;
    [SerializeField] AudioClip fever;
    [SerializeField] AudioClip homeMusic;
    [SerializeField] AudioClip commuteMusic;

    bool wasCombo;
    bool wasFever;

    void Awake()
    {
        run.LuckGained += PlayLuck;
        run.Started += PlayCommuteMusic;
        run.Finished += PlayHomeMusic;
        combo.Changed += PlayCombo;
        PlayHomeMusic();
    }

    void PlayLuck(int amount, int multiplier)
    {
        if (amount < 0)
        {
            sfx.PlayOneShot(loss);
            return;
        }
        if (amount == 0) return;

        int step = Mathf.Clamp(combo.Streak - 1, 0, ScaleSemitones.Length - 1);
        luckVoice.pitch = Mathf.Pow(2f, ScaleSemitones[step] / 12f);
        luckVoice.PlayOneShot(gain);
    }

    void PlayCombo()
    {
        bool isCombo = combo.Streak >= config.comboStreak;
        if (combo.Fever && !wasFever) sfx.PlayOneShot(fever);
        else if (isCombo && !wasCombo) sfx.PlayOneShot(comboUp);

        wasCombo = isCombo;
        wasFever = combo.Fever;
    }

    void PlayHomeMusic() => PlayMusic(homeMusic);

    void PlayCommuteMusic() => PlayMusic(commuteMusic);

    void PlayMusic(AudioClip clip)
    {
        if (music.clip == clip) return;

        music.clip = clip;
        music.Play();
    }
}
