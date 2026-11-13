using UnityEngine;
using UnityEngine.EventSystems;

public class CloverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject tip;

    public void OnPointerEnter(PointerEventData eventData) => tip.SetActive(true);

    public void OnPointerExit(PointerEventData eventData) => tip.SetActive(false);
}
