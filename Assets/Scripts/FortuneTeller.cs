using UnityEngine;
using UnityEngine.UI;

// A fortune teller stands by the road at set points of the commute; one click reads the crystal ball for luck.
public class FortuneTeller : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] Button button;
    [SerializeField] Image teller;
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite readingSprite;

    int next;
    float x;

    void Awake()
    {
        button.onClick.AddListener(Read);
        run.Started += StartDay;
    }

    void StartDay()
    {
        next = 0;
        teller.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!run.Running) return;

        if (teller.gameObject.activeSelf) Move();
        else if (next < config.fortuneProgress.Length && run.Progress >= config.fortuneProgress[next]) Appear();
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
        run.AddLuck(config.PickFortunePoints());
    }
}
