using UnityEngine;
using UnityEngine.UI;

public class PlayerWalk : MonoBehaviour
{
    const int StandFrame = 1;

    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] Image body;
    [SerializeField] Sprite[] frames;
    [SerializeField] WalkDust dust;

    Vector3 home;
    float timer;
    float stumbleLeft;
    int frame;

    void Awake() => home = transform.localPosition;

    public void Bump()
    {
        stumbleLeft = config.bumpStumbleSeconds;
        body.sprite = frames[StandFrame];
    }

    void Stumble()
    {
        stumbleLeft = Mathf.Max(stumbleLeft - Time.deltaTime, 0f);

        float pixels = Mathf.Round(config.bumpKnockbackPixels * stumbleLeft / config.bumpStumbleSeconds);
        transform.localPosition = home + Vector3.left * pixels;

        float elapsed = config.bumpStumbleSeconds - stumbleLeft;
        bool flash = stumbleLeft > 0f && (int)(elapsed / config.bumpBlinkSeconds) % 2 == 0;
        body.color = flash ? config.bumpFlashColor : Color.white;
    }

    void Update()
    {
        if (stumbleLeft > 0f)
        {
            Stumble();
            return;
        }

        if (!run.Running)
        {
            body.sprite = frames[StandFrame];
            return;
        }

        timer += Time.deltaTime;
        if (timer < config.walkFrameSeconds) return;

        timer -= config.walkFrameSeconds;
        frame = (frame + 1) % frames.Length;
        body.sprite = frames[frame];
        // Frames 0 and 2 are the wide strides where a foot lands.
        if (frame % 2 == 0) dust.Spawn(body.rectTransform.anchoredPosition);
    }
}
