using UnityEngine;
using UnityEngine.UI;

public class ComboHud : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] ComboMeter combo;
    [SerializeField] Image[] pips;
    [SerializeField] Sprite pipOn;
    [SerializeField] Sprite pipOff;
    [SerializeField] Image multiplier;
    [SerializeField] Sprite x2;
    [SerializeField] Sprite x3;
    [SerializeField] GameObject fever;

    int lit;
    float popLeft;

    void Awake()
    {
        combo.Changed += Show;
        Show();
    }

    void OnDestroy() => combo.Changed -= Show;

    void Show()
    {
        int nowLit = combo.Fever ? pips.Length : Mathf.Min(combo.Streak / config.comboStreak, pips.Length);
        if (nowLit > lit) popLeft = config.comboPopSeconds;
        lit = nowLit;

        for (int i = 0; i < pips.Length; i++)
        {
            pips[i].sprite = i < lit ? pipOn : pipOff;
            pips[i].rectTransform.localScale = Vector3.one;
        }

        multiplier.enabled = combo.Multiplier > 1;
        multiplier.sprite = combo.Fever ? x3 : x2;
        fever.SetActive(combo.Fever);
        enabled = popLeft > 0f || combo.Fever;
    }

    void Update()
    {
        popLeft = Mathf.Max(popLeft - Time.unscaledDeltaTime, 0f);
        if (lit > 0)
        {
            float scale = 1f + (config.comboPopScale - 1f) * popLeft / config.comboPopSeconds;
            pips[lit - 1].rectTransform.localScale = Vector3.one * scale;
        }

        if (combo.Fever) fever.SetActive((int)(Time.unscaledTime / config.feverBlinkSeconds) % 4 != 0);
        enabled = popLeft > 0f || combo.Fever;
    }
}
