using UnityEngine;
using UnityEngine.UI;

public class SlotStand : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] SlotGame game;
    [SerializeField] Button button;
    [SerializeField] RectTransform machine;

    float timer;

    void Awake()
    {
        button.onClick.AddListener(Open);
        run.Started += StartDay;
    }

    void StartDay()
    {
        timer = 0f;
        machine.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!run.Running) return;

        if (machine.gameObject.activeSelf) Move();
        else if (!game.IsOpen) Wait();
    }

    void Move()
    {
        machine.anchoredPosition += Vector2.left * (config.GroundPixelsPerSecond * Time.deltaTime);
        if (machine.anchoredPosition.x < -config.offscreenEdgeX) machine.gameObject.SetActive(false);
    }

    void Wait()
    {
        timer += Time.deltaTime;
        if (timer < config.slotInterval) return;

        timer = 0f;
        machine.anchoredPosition = new Vector2(config.offscreenEdgeX, config.slotCenterY);
        machine.gameObject.SetActive(true);
    }

    void Open()
    {
        if (!run.Running) return;

        machine.gameObject.SetActive(false);
        game.Open();
    }
}
