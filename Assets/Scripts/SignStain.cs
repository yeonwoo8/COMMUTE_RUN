using UnityEngine;
using UnityEngine.EventSystems;

public class SignStain : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;

    RectTransform rect;
    Vector2 home;
    int shakes;
    int lastDirection;
    bool falling;

    void Awake()
    {
        rect = (RectTransform)transform;
        home = rect.anchoredPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        rect.anchoredPosition = home;
        falling = false;
        ResetShake();
    }

    void Update()
    {
        if (!falling) return;

        rect.anchoredPosition += Vector2.down * (config.stainFallSpeed * Time.deltaTime);
        if (rect.anchoredPosition.y < home.y - config.stainFallDistance) gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (falling || !run.Running) return;

        Vector2 delta = eventData.delta;
        float move = Mathf.Abs(delta.x) >= Mathf.Abs(delta.y) ? delta.x : delta.y;
        if (Mathf.Abs(move) < config.shakeMinDelta) return;

        int direction = move > 0f ? 1 : -1;
        rect.anchoredPosition = home + Vector2.right * (direction * config.stainWiggle);
        if (direction == lastDirection) return;

        lastDirection = direction;
        if (++shakes >= config.signShakesNeeded) Drop();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetShake();
        if (!falling) rect.anchoredPosition = home;
    }

    void Drop()
    {
        falling = true;
        run.AddLuck(config.signPoints);
    }

    void ResetShake()
    {
        shakes = 0;
        lastDirection = 0;
    }
}
