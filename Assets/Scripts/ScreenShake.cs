using System.Collections;
using UnityEngine;

// Overlay canvases ignore the camera, so the UI layer has to be moved along with it.
public class ScreenShake : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] Transform worldCamera;
    [SerializeField] RectTransform overlay;

    Vector3 cameraHome;
    Vector2 overlayHome;

    void Awake()
    {
        cameraHome = worldCamera.localPosition;
        overlayHome = overlay.anchoredPosition;
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(Freeze());
        StartCoroutine(Shake());
    }

    IEnumerator Freeze()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(config.bumpFreezeSeconds);
        Time.timeScale = 1f;
    }

    IEnumerator Shake()
    {
        for (float left = config.bumpShakeSeconds; left > 0f; left -= Time.unscaledDeltaTime)
        {
            float strength = config.bumpShakePixels * left / config.bumpShakeSeconds;
            Apply(Mathf.Round(strength * Mathf.Sin(left * config.bumpShakeSpeed)));
            yield return null;
        }
        Apply(0f);
    }

    void Apply(float pixels)
    {
        worldCamera.localPosition = cameraHome + Vector3.right * (pixels / GameConfig.PixelsPerUnit);
        overlay.anchoredPosition = overlayHome + Vector2.left * pixels;
    }
}
