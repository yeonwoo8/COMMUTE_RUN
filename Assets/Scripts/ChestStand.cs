using UnityEngine;
using UnityEngine.UI;

// Late in the commute a chest rolls by; the key from the bread trade opens it for the golden clover.
// On the bonus day an extra chest comes early. Every chest needs the key; none comes once the clover is held.
public class ChestStand : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] Inventory inventory;
    [SerializeField] CloverCharm clover;
    [SerializeField] Button button;
    [SerializeField] Image chest;
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openSprite;

    bool appeared;
    bool bonusPending;
    float x;
    float shakeLeft;

    void Awake()
    {
        button.onClick.AddListener(Open);
        run.Started += StartDay;
    }

    void StartDay()
    {
        appeared = false;
        bonusPending = run.Day == config.bonusChestDay;
        chest.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!run.Running) return;

        if (chest.gameObject.activeSelf) Move();
        else if (bonusPending && !clover.Has && run.Progress >= config.bonusChestProgress) Appear(isBonus: true);
        else if (!appeared && !clover.Has && run.Progress >= config.chestProgress) Appear(isBonus: false);
    }

    void Appear(bool isBonus)
    {
        if (isBonus) bonusPending = false;
        else appeared = true;
        x = config.offscreenEdgeX;
        chest.sprite = closedSprite;
        chest.gameObject.SetActive(true);
        Move();
    }

    void Move()
    {
        x -= config.GroundPixelsPerSecond * Time.deltaTime;
        shakeLeft = Mathf.Max(shakeLeft - Time.deltaTime, 0f);
        float wiggle = Mathf.Round(config.chestShakePixels * Mathf.Sin(shakeLeft * config.bumpShakeSpeed));
        chest.rectTransform.anchoredPosition = new Vector2(x + wiggle, config.chestCenterY);
        if (x < -config.offscreenEdgeX) chest.gameObject.SetActive(false);
    }

    // Without the key the chest only rattles.
    void Open()
    {
        if (!run.Running || chest.sprite == openSprite) return;

        if (!inventory.TryRemove(config.keyItem))
        {
            shakeLeft = config.chestShakeSeconds;
            return;
        }

        chest.sprite = openSprite;
        clover.Grant(chest.rectTransform);
    }
}
