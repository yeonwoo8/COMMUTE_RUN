using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static partial class PrototypeSetup
{
    static ResultScreen BuildResult(Transform canvas, CommuteRun run)
    {
        Vector2 center = Vector2.one * 0.5f;
        Color32 gold = new Color32(255, 214, 92, 255);

        var panel = Ui<Image>("Result", canvas, Vector2.zero, Vector2.one, Vector2.zero);
        panel.color = new Color32(28, 18, 48, 255);
        var closeButton = panel.gameObject.AddComponent<Button>();
        closeButton.transition = Selectable.Transition.None;

        Label("Heading", panel.transform, "ALL CLEAR!", 30f, new Vector2(0f, 128f)).color = gold;
        Label("Thanks", panel.transform, "THANKS FOR PLAYING", 10f, new Vector2(0f, 102f));

        TextMeshProUGUI days = Label("Days", panel.transform, "", 13f, new Vector2(0f, 38f));
        days.rectTransform.sizeDelta = new Vector2(300f, 90f);
        days.lineSpacing = 10f;

        Bar(panel.transform, new Vector2(0f, -18f), new Vector2(260f, 2f), gold);
        TextMeshProUGUI total = Label("Total", panel.transform, "", 20f, new Vector2(0f, -44f));
        total.color = gold;
        TextMeshProUGUI best = Label("Best", panel.transform, "", 12f, new Vector2(0f, -76f));
        Label("Tip", panel.transform, "CLICK TO RETURN TO TITLE", 9f, new Vector2(0f, -150f));
        panel.gameObject.SetActive(false);

        var result = panel.gameObject.AddComponent<ResultScreen>();
        Wire(result, ("run", run), ("closeButton", closeButton), ("days", days), ("total", total), ("best", best));
        return result;
    }
}
