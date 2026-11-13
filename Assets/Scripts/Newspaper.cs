using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Opening cutscene: a tiny lucky-3 story in the morning paper sends the player out the door.
public class Newspaper : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] Button button;
    [SerializeField] RectTransform paper;
    [SerializeField] RectTransform article;
    [SerializeField] TMP_Text line;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip click;
    [SerializeField] string[] lines =
    {
        "Hm? There's a tiny story down in the corner...",
        "\"Grab every lucky 3 on your way to work... and everything goes right!\"",
        "Whoa... Could that work for me too? Let's go!",
    };

    public event Action Done;

    int index;
    float elapsed;

    void Awake()
    {
        button.onClick.AddListener(Advance);
        enabled = false;
    }

    public void Show()
    {
        index = 0;
        paper.localScale = Vector3.one;
        paper.anchoredPosition = Vector2.zero;
        line.text = lines[0];
        gameObject.SetActive(true);
    }

    void Advance()
    {
        if (enabled) return;

        sfx.PlayOneShot(click);
        index++;
        if (index == 1)
        {
            elapsed = 0f;
            enabled = true;
        }

        if (index < lines.Length)
        {
            line.text = lines[index];
            return;
        }

        gameObject.SetActive(false);
        Done?.Invoke();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / config.newsZoomSeconds);
        float ease = 1f - (1f - t) * (1f - t);
        float scale = Mathf.Lerp(1f, config.newsZoomScale, ease);
        paper.localScale = Vector3.one * scale;
        paper.anchoredPosition = (Vector2.up * config.newsZoomLift - article.anchoredPosition * config.newsZoomScale) * ease;
        if (t >= 1f) enabled = false;
    }
}
