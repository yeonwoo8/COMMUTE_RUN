using System;
using UnityEngine;

public class CommuteRun : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] ComboMeter combo;
    [SerializeField] CloverCharm clover;
    [SerializeField] Inventory inventory;

    public event Action Started;
    public event Action Finished;
    public event Action<int> LuckChanged;
    public event Action<int, int> LuckGained;
    public int Luck { get; private set; }
    public int Day { get; private set; } = 1;
    public int Quota => config.dailyQuotas[Day - 1];
    public bool Cleared => Luck >= Quota;
    public bool IsLastDay => Day >= DayCount;
    public int DayCount => config.dailyQuotas.Length;
    public int Pay => Cleared
        ? config.basePay + (Luck - Quota) * config.bonusPerLuck
        : Luck * config.failPayPerLuck;
    public int Retries { get; private set; }

    // Only cleared days count; otherwise failing on purpose would pad the total.
    int[] dayPays;

    public int DayPay(int day) => dayPays[day - 1];

    public int TotalPay
    {
        get
        {
            int total = 0;
            foreach (int pay in dayPays) total += pay;
            return total;
        }
    }

    void Awake() => dayPays = new int[DayCount];
    public float Progress { get; private set; }
    public bool Running { get; private set; }
    public float Speed => Mathf.Lerp(1f, config.endSpeedScale, Progress * Progress);

    public void Begin()
    {
        Luck = 0;
        Progress = 0f;
        Running = true;
        combo.ResetAll();
        Started?.Invoke();
    }

    public void NextDay() => Day++;

    public void ResetDays()
    {
        Day = 1;
        Retries = 0;
        clover.Discard();
        inventory.Clear();
        Array.Clear(dayPays, 0, dayPays.Length);
    }

    void Update()
    {
        if (!Running) return;

        // deltaTime already carries the speed-up, so dividing it out keeps the commute length fixed.
        Progress = Mathf.Clamp01(Progress + Time.deltaTime / Speed / config.runSeconds);
        if (Progress >= 1f)
        {
            Finish();
            return;
        }

        // ScreenShake holds timeScale at zero during a hit-stop.
        if (Time.timeScale > 0f) Time.timeScale = Speed;
    }

    void Finish()
    {
        Running = false;
        Time.timeScale = 1f;
        combo.ResetAll();
        if (Cleared) dayPays[Day - 1] = Pay;
        else Retries++;
        Finished?.Invoke();
    }

    public void AddLuck(int amount)
    {
        int multiplier = combo.Apply(amount) * clover.Roll(amount);
        amount *= multiplier;
        Luck = Mathf.Max(0, Luck + amount);
        LuckChanged?.Invoke(Luck);
        LuckGained?.Invoke(amount, multiplier);
    }
}
