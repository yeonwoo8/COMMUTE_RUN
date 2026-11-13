using UnityEngine;
using UnityEngine.UI;

// From the moment the golden clover is found, luck gains are sometimes doubled.
// The clover sits in its own badge beside the inventory rather than taking a slot.
public class CloverCharm : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] Image badge;
    [SerializeField] PickupFx fx;

    float popLeft;

    public bool Has => badge.enabled;

    void Awake() => enabled = false;

    // Once found, the clover is kept for the rest of the game.
    public void Discard() => badge.enabled = false;

    public void Grant(RectTransform source)
    {
        badge.enabled = true;
        fx.Play(source, badge);
    }

    public int Roll(int amount)
    {
        if (amount <= 0 || !Has || Random.value >= config.cloverChance) return 1;

        popLeft = config.cloverPopSeconds;
        enabled = true;
        return config.cloverMultiplier;
    }

    void Update()
    {
        popLeft = Mathf.Max(popLeft - Time.unscaledDeltaTime, 0f);
        float t = popLeft / config.cloverPopSeconds;
        badge.rectTransform.localScale = Vector3.one * (1f + (config.cloverPopScale - 1f) * Mathf.Sin(t * Mathf.PI));
        if (popLeft <= 0f) enabled = false;
    }
}
