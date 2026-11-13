using UnityEngine;
using UnityEngine.UI;

// 까치 brings good news: a fast flyer worth extra luck if the player can catch it.
public class Magpie : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] RectTransform bird;
    [SerializeField] Button button;
    [SerializeField] Image body;
    [SerializeField] Sprite[] frames;

    float waitLeft;
    float flyTime;
    int frame;

    void Awake()
    {
        button.onClick.AddListener(Catch);
        run.Started += Hide;
        Hide();
    }

    void Update()
    {
        if (!run.Running) return;

        if (bird.gameObject.activeSelf) Fly();
        else Wait();
    }

    void Wait()
    {
        waitLeft -= Time.deltaTime;
        if (waitLeft > 0f) return;

        flyTime = 0f;
        bird.gameObject.SetActive(true);
        Fly();
    }

    void Fly()
    {
        flyTime += Time.deltaTime;
        int nextFrame = (int)(flyTime / config.magpieFlapSeconds) % frames.Length;
        if (nextFrame != frame)
        {
            frame = nextFrame;
            body.sprite = frames[frame];
        }

        float x = config.magpieEdgeX - config.magpieSpeed * flyTime;
        float y = config.magpieY + config.magpieWave * Mathf.Sin(flyTime * config.magpieWaveSpeed);
        bird.anchoredPosition = new Vector2(Mathf.Round(x), Mathf.Round(y));
        if (x < -config.magpieEdgeX) Hide();
    }

    void Catch()
    {
        if (!run.Running) return;

        run.AddLuck(config.magpiePoints);
        Hide();
    }

    void Hide()
    {
        bird.gameObject.SetActive(false);
        waitLeft = Random.Range(config.magpieIntervalMin, config.magpieIntervalMax);
    }
}
