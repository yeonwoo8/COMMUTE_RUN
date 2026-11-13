using UnityEngine;
using UnityEngine.UI;

public class CommuteHud : MonoBehaviour
{
    [SerializeField] CommuteRun run;
    [SerializeField] RectTransform marker;
    [SerializeField] Image fill;

    float laneWidth;
    int shownPixel = -1;

    void Awake() => laneWidth = fill.rectTransform.rect.width;

    void Update()
    {
        int pixel = Mathf.RoundToInt(run.Progress * laneWidth);
        if (pixel == shownPixel) return;

        shownPixel = pixel;
        marker.anchoredPosition = new Vector2(pixel, marker.anchoredPosition.y);
        fill.fillAmount = pixel / laneWidth;
    }
}
