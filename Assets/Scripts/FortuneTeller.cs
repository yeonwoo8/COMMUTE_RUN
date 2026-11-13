using UnityEngine;
using UnityEngine.UI;

// A fortune teller stands by the road at set points of the commute; one click draws a card for luck.
public class FortuneTeller : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] Button button;
    [SerializeField] Image teller;
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite readingSprite;
    [SerializeField] Image glow;
    [SerializeField] Image card;
    [Tooltip("Devil, blank and lucky card, matching config.fortuneGlowColors.")]
    [SerializeField] Sprite[] cardSprites;

    int next;
    float x;
    float cardLeft;
    Color glowColor;

    void Awake()
    {
        button.onClick.AddListener(Read);
        run.Started += StartDay;
    }

    void StartDay()
    {
        next = 0;
        cardLeft = 0f;
        glow.gameObject.SetActive(false);
        teller.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!run.Running) return;

        if (teller.gameObject.activeSelf) Move();
        else if (next < config.fortuneProgress.Length && run.Progress >= config.fortuneProgress[next]) Appear();

        if (cardLeft > 0f) FadeCard();
    }

    void Appear()
    {
        next++;
        x = config.offscreenEdgeX;
        teller.sprite = idleSprite;
        teller.gameObject.SetActive(true);
        Move();
    }

    void Move()
    {
        x -= config.GroundPixelsPerSecond * Time.deltaTime;
        teller.rectTransform.anchoredPosition = new Vector2(x, config.fortuneCenterY);
        if (x < -config.offscreenEdgeX) teller.gameObject.SetActive(false);
    }

    // Each teller reads only once.
    void Read()
    {
        if (!run.Running || teller.sprite == readingSprite) return;

        teller.sprite = readingSprite;
        int points = config.PickFortunePoints();
        ShowCard(points < 0 ? 0 : points == 0 ? 1 : 2);
        // A blank card is no reading at all, so it leaves the combo alone.
        if (points != 0) run.AddLuck(points);
    }

    void ShowCard(int kind)
    {
        card.sprite = cardSprites[kind];
        glowColor = config.fortuneGlowColors[kind];
        cardLeft = config.fortuneCardSeconds;
        glow.gameObject.SetActive(true);
        FadeCard();
    }

    // The glow flares and dies first; the card itself fades out at the end.
    void FadeCard()
    {
        cardLeft = Mathf.Max(cardLeft - Time.deltaTime, 0f);
        float shown = config.fortuneCardSeconds - cardLeft;
        glowColor.a = 1f - Mathf.Clamp01(shown / config.fortuneGlowSeconds);
        glow.color = glowColor;
        card.color = new Color(1f, 1f, 1f, Mathf.Clamp01(cardLeft / config.fortuneFadeSeconds));
        if (cardLeft <= 0f) glow.gameObject.SetActive(false);
    }
}
