using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static partial class PrototypeSetup
{
    const string RainPath = "Assets/Art/Rain.png";
    const string UmbrellaPath = "Assets/Art/Umbrella.png";

    static RainDay BuildRain(Transform hud, Transform items, CommuteRun run, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        // Own canvas: the drops scroll every frame, so only this part gets rebuilt.
        var canvas = Child<Canvas>("Rain", hud);
        canvas.transform.SetSiblingIndex(items.GetSiblingIndex() + 1);
        Stretch((RectTransform)canvas.transform);
        var overlay = canvas.gameObject.AddComponent<CanvasGroup>();
        overlay.blocksRaycasts = false;
        overlay.interactable = false;

        var tint = Ui<Image>("Tint", canvas.transform, Vector2.zero, Vector2.one, Vector2.zero);
        tint.color = new Color32(20, 24, 60, 90);
        tint.raycastTarget = false;

        ImportSprite(RainPath);
        var importer = (TextureImporter)AssetImporter.GetAtPath(RainPath);
        importer.wrapMode = TextureWrapMode.Repeat;
        importer.SaveAndReimport();
        var drops = Ui<RawImage>("Drops", canvas.transform, Vector2.zero, Vector2.one, Vector2.zero);
        drops.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(RainPath);
        drops.uvRect = new Rect(0f, 0f, 640f / 64f, 360f / 64f);
        drops.color = new Color(1f, 1f, 1f, 0.7f);
        drops.raycastTarget = false;

        Sprite umbrellaSprite = ImportSprite(UmbrellaPath);
        var umbrella = Ui<Image>("Umbrella", items, center, center, umbrellaSprite.rect.size);
        umbrella.sprite = umbrellaSprite;
        umbrella.transform.SetSiblingIndex(Object.FindAnyObjectByType<PlayerWalk>().transform.GetSiblingIndex());
        var button = umbrella.gameObject.AddComponent<Button>();
        button.transition = Selectable.Transition.None;
        umbrella.gameObject.SetActive(false);

        var sound = canvas.gameObject.AddComponent<AudioSource>();
        sound.playOnAwake = false;
        sound.loop = true;

        var rain = canvas.gameObject.AddComponent<RainDay>();
        Wire(rain, ("sound", sound), ("config", config), ("run", run), ("overlay", overlay), ("drops", drops), ("umbrella", button));
        return rain;
    }
}
