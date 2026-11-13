using UnityEngine;

public class BuildingRow : MonoBehaviour
{
    const int AnimFrames = 3;
    const int LandmarkStreet = 0;

    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] SpriteRenderer[] blocks;
    [SerializeField] SpriteRenderer[] glows;
    [SerializeField] SpriteRenderer[] overlays;
    [SerializeField] Sprite[] streets;
    [SerializeField] Sprite[] glowSprites;
    [SerializeField] Sprite[] animSprites;
    [SerializeField] float tileWidth = 20f;

    readonly int[] shown = new int[2];
    float offset;
    float animTimer;
    int animFrame;

    void Awake()
    {
        shown[0] = LandmarkStreet;
        shown[1] = PickStreet();
        ShowStreets();
    }

    void Update()
    {
        Animate();
        if (!run.Running) return;

        offset += config.scrollSpeed * Time.deltaTime;
        if (offset >= tileWidth)
        {
            offset -= tileWidth;
            shown[0] = shown[1];
            shown[1] = PickStreet();
            ShowStreets();
        }
        transform.localPosition = new Vector3(-offset, 0f, 0f);
    }

    // The landmark only opens the commute, so it stays special.
    int PickStreet() => Random.Range(LandmarkStreet + 1, streets.Length);

    void Animate()
    {
        animTimer += Time.deltaTime;
        if (animTimer < config.cityAnimFrameSeconds) return;

        animTimer -= config.cityAnimFrameSeconds;
        animFrame = (animFrame + 1) % AnimFrames;
        ShowOverlays();
    }

    void ShowStreets()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].sprite = streets[shown[i]];
            glows[i].sprite = glowSprites[shown[i]];
        }
        ShowOverlays();
    }

    void ShowOverlays()
    {
        for (int i = 0; i < overlays.Length; i++)
            overlays[i].sprite = animSprites[shown[i] * AnimFrames + animFrame];
    }
}
