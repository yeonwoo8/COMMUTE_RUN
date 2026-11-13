using UnityEngine;
using UnityEngine.UI;

public class PickupFx : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] Image flyer;
    [SerializeField] RectTransform[] sparks;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip pop;

    RectTransform area;
    RectTransform target;
    Vector2[] directions;
    Vector2 from;
    Vector2 to;
    float elapsed;

    void Awake()
    {
        area = (RectTransform)transform;
        directions = new Vector2[sparks.Length];
        for (int i = 0; i < sparks.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / sparks.Length;
            directions[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        enabled = false;
    }

    public void Play(RectTransform source, Image icon)
    {
        if (enabled) target.localScale = Vector3.one;

        from = area.InverseTransformPoint(source.position);
        to = area.InverseTransformPoint(icon.rectTransform.position);
        target = icon.rectTransform;
        target.localScale = Vector3.zero;
        flyer.sprite = icon.sprite;
        // SetNativeSize scales by the canvas's 100 reference PPU, which blows 32 PPU art up about 3x.
        flyer.rectTransform.sizeDelta = icon.sprite.rect.size;
        flyer.gameObject.SetActive(true);
        foreach (RectTransform spark in sparks) spark.gameObject.SetActive(true);
        elapsed = 0f;
        enabled = true;
        sfx.PlayOneShot(pop);
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        UpdateSparks(elapsed / config.sparkSeconds);

        float fly = elapsed / config.pickupFlySeconds;
        if (fly < 1f)
        {
            float ease = 1f - (1f - fly) * (1f - fly);
            Vector2 position = Vector2.Lerp(from, to, ease) + Vector2.up * (config.pickupArcPixels * 4f * fly * (1f - fly));
            flyer.rectTransform.anchoredPosition = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
            flyer.rectTransform.localScale = Vector3.one * Mathf.Lerp(config.pickupStartScale, 1f, ease);
            return;
        }

        flyer.gameObject.SetActive(false);
        float pop = (elapsed - config.pickupFlySeconds) / config.slotPopSeconds;
        if (pop < 1f)
        {
            target.localScale = Vector3.one * (1f + (config.slotPopScale - 1f) * Mathf.Sin(pop * Mathf.PI));
            return;
        }

        target.localScale = Vector3.one;
        enabled = false;
    }

    void UpdateSparks(float t)
    {
        bool visible = t < 1f && (t < 0.6f || (int)(elapsed / config.luckBlinkSeconds) % 2 == 0);
        float reach = config.sparkDistance * (1f - (1f - t) * (1f - t));
        for (int i = 0; i < sparks.Length; i++)
        {
            sparks[i].gameObject.SetActive(visible);
            Vector2 position = from + directions[i] * reach;
            sparks[i].anchoredPosition = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        }
    }
}
