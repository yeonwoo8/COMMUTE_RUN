using System;
using UnityEngine;
using UnityEngine.UI;

// The end of a commute: the boss either hands over a bonus or scolds, and the pay counts up.
public class OfficeScene : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] Button continueButton;
    [SerializeField] Image boss;
    [SerializeField] Sprite[] happyFrames;
    [SerializeField] Sprite[] angryFrames;
    [SerializeField] Image headline;
    [SerializeField] Sprite clearHeadline;
    [SerializeField] Sprite failHeadline;
    [SerializeField] NumberSprites pay;

    public event Action Done;

    Sprite[] frames;
    int payTarget;
    float elapsed;

    void Awake() => continueButton.onClick.AddListener(Continue);

    public void Show()
    {
        bool clear = run.Cleared;
        frames = clear ? happyFrames : angryFrames;
        headline.sprite = clear ? clearHeadline : failHeadline;
        headline.rectTransform.sizeDelta = headline.sprite.rect.size;
        payTarget = run.Pay;

        elapsed = 0f;
        boss.sprite = frames[0];
        pay.Show(0);
        gameObject.SetActive(true);
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        boss.sprite = frames[(int)(elapsed / config.bossFrameSeconds) % frames.Length];
        pay.Show(Mathf.RoundToInt(payTarget * Mathf.Clamp01(elapsed / config.payCountSeconds)));
    }

    // The first click skips the count-up, the next one moves on.
    void Continue()
    {
        if (elapsed < config.payCountSeconds)
        {
            elapsed = config.payCountSeconds;
            return;
        }

        gameObject.SetActive(false);
        Done?.Invoke();
    }
}
