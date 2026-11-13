using System;
using UnityEngine;

// 삼세번: every gain extends the streak, three in a row doubles the next gains, nine starts a fever.
public class ComboMeter : MonoBehaviour
{
    [SerializeField] GameConfig config;

    public event Action Changed;
    public int Streak { get; private set; }
    public bool Fever => feverLeft > 0f;
    public int Multiplier => Fever ? config.feverMultiplier : Streak >= config.comboStreak ? config.comboMultiplier : 1;

    float idleLeft;
    float feverLeft;

    void Awake() => enabled = false;

    public int Apply(int amount)
    {
        if (amount <= 0)
        {
            Break();
            return 1;
        }

        int multiplier = Multiplier;
        idleLeft = config.comboIdleSeconds;
        if (!Fever && ++Streak >= config.feverStreak) feverLeft = config.feverSeconds;
        enabled = true;
        Changed?.Invoke();
        return multiplier;
    }

    // A fever cannot be broken; it only runs out.
    public void Break()
    {
        if (Fever || Streak == 0) return;

        Streak = 0;
        Changed?.Invoke();
    }

    public void ResetAll()
    {
        Streak = 0;
        feverLeft = 0f;
        enabled = false;
        Changed?.Invoke();
    }

    // Unscaled so the late-commute speed-up does not shorten the combo window.
    void Update()
    {
        float dt = Time.unscaledDeltaTime;
        if (Fever)
        {
            feverLeft -= dt;
            if (feverLeft <= 0f) ResetAll();
            return;
        }

        idleLeft -= dt;
        if (idleLeft > 0f) return;

        Break();
        enabled = false;
    }
}
