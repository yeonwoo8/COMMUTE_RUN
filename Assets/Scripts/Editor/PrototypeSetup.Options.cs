using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static partial class PrototypeSetup
{
    static readonly Color32 PanelColor = new Color32(28, 18, 48, 235);
    static readonly Color32 AccentColor = new Color32(255, 214, 92, 255);

    static void BuildOptions(AudioSource music)
    {
        var flow = Object.FindAnyObjectByType<GameFlow>();
        var title = (GameObject)new SerializedObject(flow).FindProperty("title").objectReferenceValue;
        BuildVolumeSlider(flow.transform, music);
        BuildHowToPlay(title.transform);
    }

    // Stays on top of every screen so the music can be turned down at any time.
    static void BuildVolumeSlider(Transform canvas, AudioSource music)
    {
        var topRight = Vector2.one;
        var root = Child<RectTransform>("MusicVolume", canvas);
        root.anchorMin = root.anchorMax = topRight;
        root.sizeDelta = new Vector2(80f, 8f);
        root.anchoredPosition = new Vector2(-52f, -24f);
        root.SetAsLastSibling();

        var label = Label("Label", root, "BGM", 9f, new Vector2(-56f, 0f));
        label.rectTransform.sizeDelta = new Vector2(30f, 12f);

        var background = Ui<Image>("Background", root, Vector2.zero, Vector2.one, Vector2.zero);
        background.color = PanelColor;

        var fillArea = Child<RectTransform>("FillArea", root);
        Stretch(fillArea);
        var fill = Ui<Image>("Fill", fillArea, Vector2.zero, Vector2.one, Vector2.zero);
        fill.color = AccentColor;
        fill.raycastTarget = false;

        var handleArea = Child<RectTransform>("HandleArea", root);
        Stretch(handleArea);
        var handle = Ui<Image>("Handle", handleArea, Vector2.zero, Vector2.one, new Vector2(6f, 6f));
        handle.color = new Color32(251, 241, 234, 255);

        var slider = root.gameObject.AddComponent<Slider>();
        slider.fillRect = fill.rectTransform;
        slider.handleRect = handle.rectTransform;
        slider.targetGraphic = handle;
        slider.value = music.volume;

        Wire(root.gameObject.AddComponent<MusicVolume>(), ("slider", slider), ("music", music));
    }

    static void BuildHowToPlay(Transform title)
    {
        Vector2 center = Vector2.one * 0.5f;
        var topRight = Vector2.one;
        var open = Ui<Image>("Help", title, topRight, topRight, new Vector2(20f, 20f));
        open.color = PanelColor;
        open.rectTransform.anchoredPosition = new Vector2(-24f, -50f);
        var openButton = open.gameObject.AddComponent<Button>();
        TextMeshProUGUI mark = Label("Mark", open.transform, "?", 14f, Vector2.zero);
        mark.rectTransform.sizeDelta = open.rectTransform.sizeDelta;
        mark.color = AccentColor;

        var panel = Ui<Image>("HowToPlay", title, Vector2.zero, Vector2.one, Vector2.zero);
        panel.color = new Color32(10, 8, 20, 200);
        var closeButton = panel.gameObject.AddComponent<Button>();
        closeButton.transition = Selectable.Transition.None;

        var box = Ui<Image>("Box", panel.transform, center, center, new Vector2(420f, 170f));
        box.color = PanelColor;
        box.raycastTarget = false;

        Label("Heading", box.transform, "HOW TO PLAY", 20f, new Vector2(0f, 58f)).color = AccentColor;
        TextMeshProUGUI body = Label("Body", box.transform,
            "Click anything with the number 3 to raise your luck score!\n\n" +
            "Lucky things are hidden all over the street.\nFind them and click them to score even more!",
            11f, new Vector2(0f, -2f));
        body.rectTransform.sizeDelta = new Vector2(390f, 80f);
        Label("Tip", box.transform, "CLICK TO CLOSE", 8f, new Vector2(0f, -70f));
        panel.gameObject.SetActive(false);

        Wire(title.gameObject.AddComponent<HowToPlay>(),
            ("openButton", openButton), ("closeButton", closeButton), ("panel", panel.gameObject));
    }

    // The HUD canvas has no raycaster, so the badge gets its own to receive hover events.
    static void BuildCloverTooltip(Image badge)
    {
        Vector2 center = Vector2.one * 0.5f;
        badge.gameObject.AddComponent<Canvas>();
        badge.gameObject.AddComponent<GraphicRaycaster>();

        var tip = Ui<Image>("Tooltip", badge.transform, center, center, new Vector2(150f, 32f));
        tip.color = PanelColor;
        tip.raycastTarget = false;
        tip.rectTransform.anchoredPosition = new Vector2(0f, 30f);
        Label("Title", tip.transform, "GOLDEN CLOVER", 9f, new Vector2(0f, 7f)).color = AccentColor;
        TextMeshProUGUI body = Label("Body", tip.transform, "Sometimes doubles your luck!", 8f, new Vector2(0f, -7f));
        body.rectTransform.sizeDelta = new Vector2(144f, 12f);
        tip.gameObject.SetActive(false);

        Wire(badge.gameObject.AddComponent<CloverTooltip>(), ("tip", tip.gameObject));
    }

    static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
    }
}
