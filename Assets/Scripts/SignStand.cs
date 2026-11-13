using UnityEngine;

public class SignStand : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] RectTransform sign;
    [SerializeField] SignStain stain;

    float timer;

    void Awake() => run.Started += StartDay;

    void StartDay()
    {
        timer = 0f;
        sign.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!run.Running) return;

        if (sign.gameObject.activeSelf) Move();
        else Wait();
    }

    void Move()
    {
        sign.anchoredPosition += Vector2.left * (config.GroundPixelsPerSecond * Time.deltaTime);
        if (sign.anchoredPosition.x < -config.offscreenEdgeX) sign.gameObject.SetActive(false);
    }

    void Wait()
    {
        timer += Time.deltaTime;
        if (timer < config.signInterval) return;

        timer = 0f;
        sign.anchoredPosition = new Vector2(config.offscreenEdgeX, config.signCenterY);
        sign.gameObject.SetActive(true);
        stain.Show();
    }
}
