using System;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Shown after the last day is cleared: pay per day, total, and the saved best total.
public class ResultScreen : MonoBehaviour
{
    const string BestKey = "BestPay";

    [SerializeField] CommuteRun run;
    [SerializeField] Button closeButton;
    [SerializeField] TMP_Text days;
    [SerializeField] TMP_Text total;
    [SerializeField] TMP_Text best;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip fanfare;

    public event Action Done;

    void Awake() => closeButton.onClick.AddListener(Close);

    public void Show()
    {
        var lines = new StringBuilder();
        for (int day = 1; day <= run.DayCount; day++)
            lines.Append("DAY ").Append(day).Append("   ").Append(Won(run.DayPay(day))).Append('\n');
        lines.Append("RETRIES   ").Append(run.Retries);
        days.text = lines.ToString();

        int totalPay = run.TotalPay;
        int bestPay = PlayerPrefs.GetInt(BestKey, 0);
        total.text = "TOTAL   " + Won(totalPay);
        if (totalPay > bestPay)
        {
            PlayerPrefs.SetInt(BestKey, totalPay);
            PlayerPrefs.Save();
            best.text = "NEW BEST!";
        }
        else
        {
            best.text = "BEST   " + Won(bestPay);
        }

        gameObject.SetActive(true);
        sfx.PlayOneShot(fanfare);
    }

    static string Won(int amount) => amount.ToString("N0", CultureInfo.InvariantCulture) + " WON";

    void Close()
    {
        gameObject.SetActive(false);
        Done?.Invoke();
    }
}
