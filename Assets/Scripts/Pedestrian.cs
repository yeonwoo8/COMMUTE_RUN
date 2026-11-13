using UnityEngine;
using UnityEngine.UI;

public class Pedestrian : MonoBehaviour
{
    enum Mode { Pass, Bump, Ask, Trade, Clover, Served, Fallen }

    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] Inventory inventory;
    [SerializeField] BreadPickup bread;
    [SerializeField] Button button;
    [SerializeField] Image body;
    [SerializeField] GameObject bubble;
    [SerializeField] GameObject tradeBubble;
    [SerializeField] GameObject cloverBubble;
    [SerializeField] GameObject reward;
    [SerializeField] PickupFx fx;
    [SerializeField] GameObject warning;
    [SerializeField] PlayerWalk player;
    [SerializeField] ScreenShake shake;
    [SerializeField] ComboMeter combo;
    [SerializeField] CloverCharm clover;
    [SerializeField] Sprite[] walkFrames;
    [SerializeField] Sprite[] fallFrames;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip bumpSound;

    Mode mode;
    float waitTimer;
    float frameTimer;
    float flyTimer;
    int frame;
    int droppers;
    int askers;
    int trades;
    bool cloverAsked;

    void Awake()
    {
        button.onClick.AddListener(GiveBread);
        run.Started += StartDay;
    }

    void StartDay()
    {
        droppers = askers = 0;
        trades = 0;
        cloverAsked = false;
        waitTimer = 0f;
        warning.SetActive(false);
        body.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!run.Running) return;

        if (!body.gameObject.activeSelf)
        {
            Wait();
            return;
        }

        Animate();
        Move();
    }

    void Wait()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer < config.pedestrianInterval) return;

        waitTimer = 0f;
        mode = PickMode();
        frame = 0;
        frameTimer = 0f;
        body.sprite = walkFrames[0];
        bubble.SetActive(mode == Mode.Ask);
        tradeBubble.SetActive(mode == Mode.Trade);
        cloverBubble.SetActive(mode == Mode.Clover);
        reward.SetActive(false);
        // Only people asking for bread can be clicked; everyone else lets clicks pass through.
        body.raycastTarget = mode == Mode.Ask || mode == Mode.Trade || mode == Mode.Clover;
        body.rectTransform.anchoredPosition = new Vector2(config.pedestrianEdgeX, config.pedestrianFeetY);
        body.gameObject.SetActive(true);
    }

    Mode PickMode()
    {
        if (!cloverAsked && run.Progress >= config.cloverAskProgress && System.Array.IndexOf(config.cloverAskDays, run.Day) >= 0)
        {
            cloverAsked = true;
            return Mode.Clover;
        }

        // Later in the commute, traders offer a key for bread; once the key is won, the rest skip.
        if (trades < config.tradeProgresses.Length && run.Progress >= config.tradeProgresses[trades])
        {
            trades++;
            if (!inventory.Contains(config.keyItem) && !clover.Has) return Mode.Trade;
        }

        // With the clover held, the key-trade slots go to extra bread askers instead.
        int maxAskers = config.maxBreadAskers + (clover.Has ? config.tradeProgresses.Length : 0);
        if (askers < maxAskers && inventory.Contains(config.breadItem) && Random.value < config.askChance)
        {
            askers++;
            return Mode.Ask;
        }

        if (droppers < config.maxBreadDroppers && Random.value < config.dropChance)
        {
            droppers++;
            return Mode.Bump;
        }

        return Mode.Pass;
    }

    void Animate()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer < config.walkFrameSeconds) return;

        frameTimer -= config.walkFrameSeconds;
        if (mode == Mode.Fallen)
        {
            frame = Mathf.Min(frame + 1, fallFrames.Length - 1);
            body.sprite = fallFrames[frame];
            return;
        }

        frame = (frame + 1) % walkFrames.Length;
        body.sprite = walkFrames[frame];
    }

    void Move()
    {
        float speed = config.GroundPixelsPerSecond;
        float y = config.pedestrianFeetY;

        if (mode != Mode.Fallen)
        {
            speed += config.pedestrianWalkSpeed;
        }
        else if (flyTimer > 0f)
        {
            flyTimer -= Time.deltaTime;
            float t = Mathf.Clamp01(1f - flyTimer / config.bumpFlySeconds);
            speed -= config.bumpFlySpeed;
            y += Mathf.Round(4f * config.bumpFlyHeight * t * (1f - t));
        }

        float x = body.rectTransform.anchoredPosition.x - speed * Time.deltaTime;
        body.rectTransform.anchoredPosition = new Vector2(x, y);

        if (mode == Mode.Bump) warning.SetActive(x <= config.bumpX + config.bumpWarnDistance);
        if (mode == Mode.Bump && x <= config.bumpX) Fall(x);
        if (x < -config.pedestrianEdgeX) body.gameObject.SetActive(false);
    }

    void Fall(float x)
    {
        mode = Mode.Fallen;
        frame = 0;
        frameTimer = 0f;
        flyTimer = config.bumpFlySeconds;
        body.sprite = fallFrames[0];
        warning.SetActive(false);
        bread.Drop(x + config.breadDropOffsetX);
        player.Bump();
        shake.Play();
        sfx.PlayOneShot(bumpSound);
        combo.Break();
    }

    void GiveBread()
    {
        if (!run.Running) return;

        if (mode == Mode.Clover) ShowClover();
        else if (mode == Mode.Trade) TradeForKey();
        else if (mode == Mode.Ask && inventory.TryRemove(config.breadItem)) ServeBread();
    }

    void ServeBread()
    {
        mode = Mode.Served;
        body.raycastTarget = false;
        bubble.SetActive(false);
        reward.SetActive(true);
        run.AddLuck(config.breadRewardPoints);
    }

    // Showing the clover is enough; the player keeps it.
    void ShowClover()
    {
        if (!clover.Has) return;

        mode = Mode.Served;
        body.raycastTarget = false;
        cloverBubble.SetActive(false);
        reward.SetActive(true);
        run.AddLuck(config.cloverAskPoints);
    }

    void TradeForKey()
    {
        if (inventory.Count(config.breadItem) < config.breadsForKey) return;

        for (int i = 0; i < config.breadsForKey; i++) inventory.TryRemove(config.breadItem);
        mode = Mode.Served;
        body.raycastTarget = false;
        tradeBubble.SetActive(false);
        if (inventory.TryAdd(config.keyItem, out Image icon)) fx.Play(body.rectTransform, icon);
    }
}
