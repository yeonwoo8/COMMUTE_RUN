using UnityEngine;
using UnityEngine.UI;

// Shows an integer with digit sprites, centered on this transform.
public class NumberSprites : MonoBehaviour
{
    [SerializeField] Image[] digits;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image prefix;
    [SerializeField] float step;

    int shown = -1;

    public void Show(int value)
    {
        if (value == shown) return;
        shown = value;

        int count = 1;
        for (int v = value; v >= 10; v /= 10) count++;

        // A value longer than the slots shows as all nines instead of overflowing.
        if (count > digits.Length)
        {
            count = digits.Length;
            value = (int)Mathf.Pow(10f, count) - 1;
        }

        int first = prefix ? 1 : 0;
        float left = -(count + first - 1) * step * 0.5f;
        if (prefix) prefix.rectTransform.anchoredPosition = new Vector2(left, 0f);

        for (int i = 0; i < digits.Length; i++)
        {
            digits[i].enabled = i < count;
            digits[i].rectTransform.anchoredPosition = new Vector2(left + (first + i) * step, 0f);
        }

        for (int i = count - 1; i >= 0; i--, value /= 10)
            digits[i].sprite = sprites[value % 10];
    }
}
