using UnityEngine.EventSystems;
using UnityEngine.UI;

// Targets move off the pointer before release, so a normal Button click would be cancelled.
public class PressButton : Button
{
    public override void OnPointerDown(PointerEventData e)
    {
        base.OnPointerDown(e);
        if (e.button == PointerEventData.InputButton.Left && IsInteractable()) onClick.Invoke();
    }

    public override void OnPointerClick(PointerEventData e) { }
}
