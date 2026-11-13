using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LuckScoreboard : MonoBehaviour
{
    const int MaxShownLuck = 999;
    const int MilestoneMod = 100;
    const int MilestoneValue = 33;

    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] Image flash;
    [SerializeField] Image[] digits;
    [SerializeField] Sprite[] digitSprites;

    RectTransform panel;
    Vector2 panelHome;
    float[] digitHomeY;
    float[] bounceLeft;
    int shown;
    int target;
    float stepLeft;
    float shakeLeft;
    float milestoneLeft;
    bool animating;

    void Awake()
    {
        panel = (RectTransform)transform;
        panelHome = panel.anchoredPosition;
        bounceLeft = new float[digits.Length];
        digitHomeY = new float[digits.Length];
        for (int i = 0; i < digits.Length; i++) digitHomeY[i] = digits[i].rectTransform.anchoredPosition.y;
    }

    void OnEnable()
    {
        Snap();
        run.LuckChanged += OnLuckChanged;
        run.Started += Snap;
    }

    void OnDisable()
    {
        run.LuckChanged -= OnLuckChanged;
        run.Started -= Snap;
    }

    // A new day resets luck to zero without rolling the digits down.
    void Snap()
    {
        shown = target = Mathf.Min(run.Luck, MaxShownLuck);
        int value = shown;
        for (int i = digits.Length - 1; i >= 0; i--, value /= 10) digits[i].sprite = digitSprites[value % 10];
    }

    void OnLuckChanged(int luck)
    {
        target = Mathf.Min(luck, MaxShownLuck);
        if (!animating) StartCoroutine(Animate());
    }

    // Unscaled so the board keeps moving during the collision freeze.
    IEnumerator Animate()
    {
        animating = true;
        while (IsBusy())
        {
            float dt = Time.unscaledDeltaTime;
            Roll(dt);
            Bounce(dt);
            Shake(dt);
            Flash(dt);
            yield return null;
        }
        animating = false;
    }

    bool IsBusy()
    {
        if (shown != target || shakeLeft > 0f || milestoneLeft > 0f) return true;
        foreach (float left in bounceLeft)
            if (left > 0f) return true;
        return false;
    }

    void Roll(float dt)
    {
        stepLeft -= dt;
        if (shown == target || stepLeft > 0f) return;

        stepLeft = config.luckRollStepSeconds;
        int previous = shown;
        bool down = target < shown;
        shown += down ? -1 : 1;

        for (int i = digits.Length - 1, a = previous, b = shown; i >= 0; i--, a /= 10, b /= 10)
        {
            digits[i].color = down ? config.luckDropColor : Color.white;
            if (a % 10 == b % 10) continue;
            digits[i].sprite = digitSprites[b % 10];
            bounceLeft[i] = config.luckBounceSeconds;
        }

        if (down) shakeLeft = config.luckShakeSeconds;
        else if (shown % MilestoneMod == MilestoneValue || shown == run.Quota) milestoneLeft = config.luckMilestoneSeconds;
    }

    void Bounce(float dt)
    {
        for (int i = 0; i < digits.Length; i++)
        {
            if (bounceLeft[i] <= 0f) continue;

            bounceLeft[i] = Mathf.Max(bounceLeft[i] - dt, 0f);
            float offset = Mathf.Round(config.luckBouncePixels * bounceLeft[i] / config.luckBounceSeconds);
            RectTransform rect = digits[i].rectTransform;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, digitHomeY[i] + offset);
        }
    }

    void Shake(float dt)
    {
        if (shakeLeft <= 0f) return;

        shakeLeft = Mathf.Max(shakeLeft - dt, 0f);
        float strength = config.luckShakePixels * shakeLeft / config.luckShakeSeconds;
        panel.anchoredPosition = panelHome + Vector2.right * Mathf.Round(strength * Mathf.Sin(shakeLeft * config.bumpShakeSpeed));
        if (shakeLeft > 0f) return;

        foreach (Image digit in digits) digit.color = Color.white;
    }

    void Flash(float dt)
    {
        if (milestoneLeft <= 0f) return;

        milestoneLeft = Mathf.Max(milestoneLeft - dt, 0f);
        flash.enabled = milestoneLeft > 0f && (int)(milestoneLeft / config.luckBlinkSeconds) % 2 == 0;
    }
}
