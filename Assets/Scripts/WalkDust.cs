using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class WalkDust : MonoBehaviour
{
    class Puff
    {
        public Image image;
        public Vector2 start;
        public float elapsed;
    }

    [SerializeField] GameConfig config;
    [SerializeField] Sprite[] frames;

    readonly List<Puff> active = new();
    ObjectPool<Puff> pool;
    RectTransform area;

    void Awake()
    {
        area = (RectTransform)transform;
        pool = new ObjectPool<Puff>(Create, p => p.image.gameObject.SetActive(true), p => p.image.gameObject.SetActive(false));
        pool.Prewarm(config.dustPoolSize);
        enabled = false;
    }

    Puff Create()
    {
        var image = new GameObject("Puff", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image)).GetComponent<Image>();
        image.rectTransform.SetParent(area, false);
        image.rectTransform.sizeDelta = frames[0].rect.size;
        image.raycastTarget = false;
        return new Puff { image = image };
    }

    public void Spawn(Vector2 feet)
    {
        Puff puff = pool.Get();
        puff.start = feet + config.dustOffset;
        puff.elapsed = 0f;
        puff.image.sprite = frames[0];
        puff.image.rectTransform.anchoredPosition = puff.start;
        active.Add(puff);
        enabled = true;
    }

    void Update()
    {
        for (int i = active.Count - 1; i >= 0; i--)
        {
            Puff puff = active[i];
            puff.elapsed += Time.deltaTime;
            int frame = (int)(puff.elapsed / config.dustFrameSeconds);
            if (frame >= frames.Length)
            {
                pool.Release(puff);
                active[i] = active[^1];
                active.RemoveAt(active.Count - 1);
                continue;
            }

            float t = puff.elapsed / (config.dustFrameSeconds * frames.Length);
            puff.image.sprite = frames[frame];
            puff.image.rectTransform.anchoredPosition = puff.start + new Vector2(
                -Mathf.Round(config.GroundPixelsPerSecond * puff.elapsed),
                Mathf.Round(config.dustRisePixels * t));
        }

        if (active.Count == 0) enabled = false;
    }
}
