using UnityEngine;
using UnityEngine.UI;

public class Motorbike : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] PlateSpawner spawner;
    [SerializeField] Image body;
    [SerializeField] Sprite[] frames;

    float waitTimer;
    float frameTimer;
    int frame;

    void Awake() => run.Started += StartDay;

    void StartDay()
    {
        waitTimer = 0f;
        body.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!run.Running) return;

        if (body.gameObject.activeSelf) Ride();
        else Wait();
    }

    void Wait()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer < config.motorbikeInterval) return;

        // Reserving room for the widest vehicle behind it keeps the lane collision-free.
        Vector2 size = body.rectTransform.sizeDelta;
        if (!spawner.TryReserveLane((size.x + config.bus.size.x) * 0.5f, out float laneBottom)) return;

        waitTimer = 0f;
        body.rectTransform.anchoredPosition = new Vector2(config.vehicleEdgeX, laneBottom + size.y * 0.5f);
        body.gameObject.SetActive(true);
    }

    void Ride()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer >= config.cityAnimFrameSeconds)
        {
            frameTimer -= config.cityAnimFrameSeconds;
            frame = (frame + 1) % frames.Length;
            body.sprite = frames[frame];
        }

        body.rectTransform.anchoredPosition += Vector2.left * (config.itemSpeed * Time.deltaTime);
        if (body.rectTransform.anchoredPosition.x < -config.vehicleEdgeX) body.gameObject.SetActive(false);
    }
}
