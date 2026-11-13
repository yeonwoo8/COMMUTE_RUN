using UnityEngine;
using UnityEngine.UI;

public class LicensePlate : MonoBehaviour
{
    const string SafeDigits = "01256789";
    const int FourTier = 4;

    [SerializeField] Image details;
    [SerializeField] RectTransform plate;
    [SerializeField] Sprite[] digitSprites;
    [SerializeField] Sprite luckySprite;

    readonly char[] digits = new char[4];
    Image carBody;
    Sprite[] wheelFrames;

    public Button[] Cells { get; private set; }
    public RectTransform Rect { get; private set; }

    void Awake()
    {
        Rect = (RectTransform)transform;
        carBody = GetComponent<Image>();
        Cells = GetComponentsInChildren<Button>(true);
    }

    public void Roll(GameConfig config, bool fever)
    {
        VehicleType vehicle = config.PickVehicle();
        carBody.sprite = vehicle.body;
        carBody.color = config.carColors[Random.Range(0, config.carColors.Length)];
        wheelFrames = vehicle.details;
        details.sprite = wheelFrames[0];
        Rect.sizeDelta = details.rectTransform.sizeDelta = vehicle.size;
        plate.anchoredPosition = vehicle.platePosition;

        int tier = config.PickPlateTier();
        bool hasFour = !fever && tier == FourTier;
        int wins = fever ? digits.Length : hasFour ? Random.Range(1, 3) : tier;

        for (int i = 0; i < digits.Length; i++)
            digits[i] = i < wins ? '3' : SafeDigits[Random.Range(0, SafeDigits.Length)];
        if (hasFour) digits[^1] = '4';

        for (int i = digits.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (digits[i], digits[j]) = (digits[j], digits[i]);
        }

        for (int i = 0; i < digits.Length; i++) ShowCell(i);
    }

    public void ShowWheelFrame(int frame) => details.sprite = wheelFrames[frame % wheelFrames.Length];

    void ShowCell(int index)
    {
        bool win = digits[index] == '3';
        Image cell = Cells[index].image;
        cell.enabled = true;
        cell.raycastTarget = win;
        cell.sprite = win ? luckySprite : digitSprites[digits[index] - '0'];
    }
}
