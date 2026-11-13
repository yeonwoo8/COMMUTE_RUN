using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.UI;

public class LuckPopups : MonoBehaviour
{
    const int MaxGlyphs = 4;
    const float GlyphStep = 7f;
    static readonly Vector2 GlyphSize = new(8f, 12f);

    class Popup
    {
        public RectTransform root;
        public Image[] glyphs;
        public Vector2 start;
        public float elapsed;
    }

    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] Sprite plus;
    [SerializeField] Sprite minus;
    [SerializeField] Sprite[] digits;

    readonly List<Popup> active = new();
    ObjectPool<Popup> pool;
    RectTransform area;

    void Awake()
    {
        area = (RectTransform)transform;
        pool = new ObjectPool<Popup>(Create, p => p.root.gameObject.SetActive(true), p => p.root.gameObject.SetActive(false));
        pool.Prewarm(config.popupPoolSize);
        run.LuckGained += Spawn;
        enabled = false;
    }

    void OnDestroy() => run.LuckGained -= Spawn;

    Popup Create()
    {
        var root = (RectTransform)new GameObject("Popup", typeof(RectTransform)).transform;
        root.SetParent(area, false);

        var glyphs = new Image[MaxGlyphs];
        for (int i = 0; i < glyphs.Length; i++)
        {
            glyphs[i] = new GameObject("Glyph", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image)).GetComponent<Image>();
            glyphs[i].rectTransform.SetParent(root, false);
            glyphs[i].rectTransform.sizeDelta = GlyphSize;
            glyphs[i].raycastTarget = false;
        }
        return new Popup { root = root, glyphs = glyphs };
    }

    void Spawn(int amount, int multiplier)
    {
        if (amount == 0) return;

        Popup popup = pool.Get();
        popup.elapsed = 0f;
        popup.start = PointerPosition();
        popup.root.anchoredPosition = popup.start;
        popup.root.localScale = Vector3.one;
        Fill(popup, amount, PickColor(amount, multiplier));
        active.Add(popup);
        enabled = true;
    }

    Vector2 PointerPosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(area, Pointer.current.position.ReadValue(), null, out Vector2 local);
        local += config.popupOffset + Vector2.right * Random.Range(-config.popupJitter, config.popupJitter);
        return new Vector2(
            Mathf.Round(Mathf.Clamp(local.x, -config.popupEdgeX, config.popupEdgeX)),
            Mathf.Round(Mathf.Clamp(local.y, -config.popupEdgeY, config.popupEdgeY)));
    }

    // x1 keeps the plain gain color; boosted gains use popupBoostColors[multiplier - 2], capped at the last entry.
    Color PickColor(int amount, int multiplier)
    {
        if (amount < 0) return config.luckDropColor;
        if (multiplier <= 1) return config.popupGainColor;
        return config.popupBoostColors[Mathf.Min(multiplier - 2, config.popupBoostColors.Length - 1)];
    }

    void Fill(Popup popup, int amount, Color color)
    {
        int value = Mathf.Abs(amount);
        int digitCount = value >= 100 ? 3 : value >= 10 ? 2 : 1;
        int count = digitCount + 1;
        float x = Mathf.Floor(-GlyphStep * (count - 1) * 0.5f);

        for (int i = 0; i < MaxGlyphs; i++)
        {
            Image glyph = popup.glyphs[i];
            glyph.enabled = i < count;
            if (i >= count) continue;

            glyph.color = color;
            glyph.rectTransform.anchoredPosition = new Vector2(x + i * GlyphStep, 0f);
        }

        popup.glyphs[0].sprite = amount > 0 ? plus : minus;
        for (int i = digitCount; i >= 1; i--, value /= 10)
            popup.glyphs[i].sprite = digits[value % 10];
    }

    // Unscaled so a popup does not hang in the air during the collision freeze.
    void Update()
    {
        for (int i = active.Count - 1; i >= 0; i--)
        {
            Popup popup = active[i];
            popup.elapsed += Time.unscaledDeltaTime;
            float t = popup.elapsed / config.popupSeconds;
            if (t >= 1f)
            {
                pool.Release(popup);
                active[i] = active[^1];
                active.RemoveAt(active.Count - 1);
                continue;
            }

            float rise = 1f - (1f - t) * (1f - t);
            popup.root.anchoredPosition = popup.start + Vector2.up * Mathf.Round(config.popupRisePixels * rise);
            bool hidden = t > config.popupBlinkFrom && (int)(popup.elapsed / config.luckBlinkSeconds) % 2 == 0;
            popup.root.localScale = hidden ? Vector3.zero : Vector3.one;
        }

        if (active.Count == 0) enabled = false;
    }
}
