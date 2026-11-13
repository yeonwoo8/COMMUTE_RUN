using UnityEngine;
using UnityEngine.UI;

public class BreadPickup : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] Inventory inventory;
    [SerializeField] Button button;
    [SerializeField] PickupFx fx;

    RectTransform rect;

    void Awake()
    {
        rect = (RectTransform)transform;
        button.onClick.AddListener(PickUp);
        run.Started += Hide;
    }

    void Hide() => gameObject.SetActive(false);

    public void Drop(float x)
    {
        gameObject.SetActive(true);
        rect.anchoredPosition = new Vector2(x, config.breadY);
    }

    void Update()
    {
        if (!run.Running) return;

        rect.anchoredPosition += Vector2.left * (config.GroundPixelsPerSecond * Time.deltaTime);
        if (rect.anchoredPosition.x < -config.pedestrianEdgeX) gameObject.SetActive(false);
    }

    void PickUp()
    {
        if (!run.Running || !inventory.TryAdd(config.breadItem, out Image icon)) return;

        fx.Play(rect, icon);
        gameObject.SetActive(false);
    }
}
