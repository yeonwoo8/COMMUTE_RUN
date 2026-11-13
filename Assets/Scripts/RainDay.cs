using UnityEngine;
using UnityEngine.UI;

// On the rain day the street is dim and plates come slower until an umbrella is clicked for luck.
public class RainDay : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] CanvasGroup overlay;
    [SerializeField] RawImage drops;
    [SerializeField] Button umbrella;
    [SerializeField] AudioSource sound;

    float clearLeft;
    float umbrellaWait;
    float umbrellaX;

    public bool Raining => enabled && clearLeft <= 0f;

    void Awake()
    {
        umbrella.onClick.AddListener(Clear);
        run.Started += StartDay;
        run.Finished += Stop;
        Stop();
    }

    void StartDay()
    {
        if (run.Day != config.rainDay) return;

        clearLeft = 0f;
        umbrellaWait = config.umbrellaFirstSeconds;
        overlay.alpha = 1f;
        sound.volume = config.rainVolume;
        sound.Play();
        enabled = true;
    }

    void Stop()
    {
        overlay.alpha = 0f;
        sound.Stop();
        umbrella.gameObject.SetActive(false);
        enabled = false;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        clearLeft -= dt;
        overlay.alpha = Mathf.MoveTowards(overlay.alpha, Raining ? 1f : 0f, dt / config.rainFadeSeconds);
        sound.volume = overlay.alpha * config.rainVolume;

        Rect uv = drops.uvRect;
        uv.position += config.rainScroll * dt;
        drops.uvRect = uv;

        if (umbrella.gameObject.activeSelf) MoveUmbrella(dt);
        else if (Raining) WaitUmbrella(dt);
    }

    void WaitUmbrella(float dt)
    {
        umbrellaWait -= dt;
        if (umbrellaWait > 0f) return;

        umbrellaX = config.offscreenEdgeX;
        umbrella.gameObject.SetActive(true);
    }

    void MoveUmbrella(float dt)
    {
        umbrellaX -= config.GroundPixelsPerSecond * dt;
        umbrella.image.rectTransform.anchoredPosition = new Vector2(Mathf.Round(umbrellaX), config.umbrellaY);
        if (umbrellaX >= -config.offscreenEdgeX) return;

        umbrella.gameObject.SetActive(false);
        umbrellaWait = config.umbrellaIntervalSeconds;
    }

    void Clear()
    {
        if (!run.Running) return;

        clearLeft = config.rainClearSeconds;
        run.AddLuck(config.umbrellaPoints);
        umbrellaWait = config.umbrellaIntervalSeconds;
        umbrella.gameObject.SetActive(false);
    }
}
