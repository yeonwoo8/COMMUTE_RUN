using System;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static partial class PrototypeSetup
{
    const string StripePath = "Assets/Art/Placeholder.png";
    const string SidewalkPath = "Assets/Art/Sidewalk.png";
    const string BuildingsPath = "Assets/Art/Buildings_{0}.png";
    const string GlowPath = "Assets/Art/Glow_{0}.png";
    const string BuildingsAnimPath = "Assets/Art/BuildingsAnim_{0}_{1}.png";
    const string SkyPath = "Assets/Art/Sky.png";
    const string SkylinePath = "Assets/Art/Skyline.png";
    const string PropsPath = "Assets/Art/Props.png";
    const string ForegroundPath = "Assets/Art/Foreground.png";
    const string MotorbikePath = "Assets/Art/Motorbike_{0}.png";
    const float PropsCenterY = 1.71875f;
    static readonly string[] StreetNames = { "A", "B", "C" };
    const string PlayerFramePath = "Assets/Art/Player_{0}.png";
    const int PlayerFrameCount = 4;
    const string SlotPath = "Assets/Art/Slot.png";
    const string CarBodyPath = "Assets/Art/CarBody.png";
    const string CarDetailsPath = "Assets/Art/CarDetails_{0}.png";
    const string BusBodyPath = "Assets/Art/BusBody.png";
    const string BusDetailsPath = "Assets/Art/BusDetails_{0}.png";
    const int WheelFrameCount = 6;
    const string PersonWalkPath = "Assets/Art/Person_Walk_{0}.png";
    const string PersonFallPath = "Assets/Art/Person_Fall_{0}.png";
    const string HomePath = "Assets/Art/Home.png";
    const string LogoPath = "Assets/Art/Logo.png";
    const string StartButtonPath = "Assets/Art/StartButton.png";
    const string StartButtonHoverPath = "Assets/Art/StartButtonHover.png";
    const string StartButtonPressedPath = "Assets/Art/StartButtonPressed.png";
    const string DustPath = "Assets/Art/Dust_{0}.png";
    const string SparkPath = "Assets/Art/Spark.png";
    const string TrackBarPath = "Assets/Art/TrackBar.png";
    const string TrackFillPath = "Assets/Art/TrackFill.png";
    const string TrackPlayerPath = "Assets/Art/TrackPlayer.png";
    const string TrackHomePath = "Assets/Art/TrackHome.png";
    const string TrackOfficePath = "Assets/Art/TrackOffice.png";
    const string OfficePath = "Assets/Art/Office.png";
    const string OfficeDeskPath = "Assets/Art/OfficeDesk.png";
    const string BossAngryPath = "Assets/Art/Boss_Angry_{0}.png";
    const string BossHappyPath = "Assets/Art/Boss_Happy_{0}.png";
    const string QuotaClearPath = "Assets/Art/QuotaClear.png";
    const string QuotaFailPath = "Assets/Art/QuotaFail.png";
    const string PayDigitPath = "Assets/Art/PayDigit_{0}.png";
    const string PayWonPath = "Assets/Art/PayWon.png";
    const string DayLabelPath = "Assets/Art/DayLabel.png";
    const string DayDigitPath = "Assets/Art/DayDigit_{0}.png";
    const string TvDigitPath = "Assets/Art/TvDigit_{0}.png";
    const string MagpiePath = "Assets/Art/Magpie_{0}.png";
    const string ComboPipOnPath = "Assets/Art/ComboPipOn.png";
    const string ComboPipOffPath = "Assets/Art/ComboPipOff.png";
    const string ComboX2Path = "Assets/Art/ComboX2.png";
    const string ComboX3Path = "Assets/Art/ComboX3.png";
    const string FeverPath = "Assets/Art/Fever.png";
    const string BubbleWidePath = "Assets/Art/BubbleWide.png";
    const string KeyPath = "Assets/Art/Key.png";
    const string CloverPath = "Assets/Art/Clover.png";
    const string ChestClosedPath = "Assets/Art/ChestClosed.png";
    const string ChestOpenPath = "Assets/Art/ChestOpen.png";
    const string FortuneTellerPath = "Assets/Art/FortuneTeller.png";
    const string FortuneTellerLitPath = "Assets/Art/FortuneTellerLit.png";
    const string CardGlowPath = "Assets/Art/CardGlow.png";
    static readonly string[] CardPaths = { "Assets/Art/CardDevil.png", "Assets/Art/CardBlank.png", "Assets/Art/CardLucky.png" };
    const string DoorPath = "Assets/Art/Door.png";
    const string DoorOpenPath = "Assets/Art/DoorOpen.png";
    const string SignPath = "Assets/Art/Sign.png";
    const string StainPath = "Assets/Art/Stain.png";
    const string BreadPath = "Assets/Art/Bread.png";
    const string TicketPath = "Assets/Art/Lucky3.png";
    const string BubblePath = "Assets/Art/Bubble.png";
    const string PlateSpritePath = "Assets/Art/Plate.png";
    const string PlateLuckyPath = "Assets/Art/PlateLucky.png";
    const string PlateDigitPath = "Assets/Art/PlateDigit_{0}.png";
    const string HudLuckPath = "Assets/Art/HudLuck.png";
    const string HudPanelPath = "Assets/Art/HudPanel.png";
    const string HudFlashPath = "Assets/Art/HudFlash.png";
    const string PopPlusPath = "Assets/Art/PopPlus.png";
    const string PopMinusPath = "Assets/Art/PopMinus.png";
    const string PopDigitPath = "Assets/Art/PopDigit_{0}.png";
    const string HudDigitPath = "Assets/Art/HudDigit_{0}.png";
    const string HudDayPath = "Assets/Art/HudDay.png";
    const string HudDayPanelPath = "Assets/Art/HudDayPanel.png";
    const string ConfigPath = "Assets/Data/GameConfig.asset";
    const string PrefabPath = "Assets/Prefabs/LicensePlate.prefab";
    const string TmpPackage = "Packages/com.unity.ugui/Package Resources/TMP Essential Resources.unitypackage";
    const float SidewalkTopY = 1.25f;
    const float PlayerFeetY = 0.15f;

    [MenuItem("Jam/Setup Prototype")]
    static void Run()
    {
        if (!AssetDatabase.IsValidFolder("Assets/TextMesh Pro"))
        {
            AssetDatabase.ImportPackage(TmpPackage, false);
            Debug.Log("TMP resources imported. Run Jam/Setup Prototype again.");
            return;
        }

        Directory.CreateDirectory("Assets/Art");
        Directory.CreateDirectory("Assets/Data");
        Directory.CreateDirectory("Assets/Prefabs");

        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.defaultWebScreenWidth = 1280;
        PlayerSettings.defaultWebScreenHeight = 720;

        Camera camera = SetupCamera();
        GameConfig config = LoadOrCreateConfig();
        BuildScene(config, CreateStripe(), CreatePlatePrefab());

        EditorSceneManager.MarkSceneDirty(camera.gameObject.scene);
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
    }

    static Camera SetupCamera()
    {
        Camera camera = Camera.main;
        camera.allowHDR = false;
        camera.allowMSAA = false;
        camera.orthographicSize = 5.625f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color32(185, 166, 224, 255);

        if (!camera.TryGetComponent(out PixelPerfectCamera pixelPerfect))
            pixelPerfect = camera.gameObject.AddComponent<PixelPerfectCamera>();
        pixelPerfect.assetsPPU = 32;
        pixelPerfect.refResolutionX = 640;
        pixelPerfect.refResolutionY = 360;
        return camera;
    }

    static Sprite CreateStripe()
    {
        var pixels = new Color32[64 * 32];
        Array.Fill(pixels, new Color32(255, 255, 255, 255));
        return CreateSprite(StripePath, 64, 32, pixels);
    }

    static Sprite CreateSprite(string path, int width, int height, Color32[] pixels)
    {
        if (!File.Exists(path))
        {
            var texture = new Texture2D(width, height);
            texture.SetPixels32(pixels);
            File.WriteAllBytes(path, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(path);
        }
        return ImportSprite(path);
    }

    static Sprite ImportSprite(string path)
    {
        var importer = (TextureImporter)AssetImporter.GetAtPath(path);
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spritePixelsPerUnit = 32;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = false;
        importer.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    static GameConfig LoadOrCreateConfig()
    {
        var config = AssetDatabase.LoadAssetAtPath<GameConfig>(ConfigPath);
        if (!config)
        {
            config = ScriptableObject.CreateInstance<GameConfig>();
            AssetDatabase.CreateAsset(config, ConfigPath);
        }

        if (!config.breadItem || !config.keyItem)
        {
            config.breadItem = ImportSprite(BreadPath);
            config.keyItem = ImportSprite(KeyPath);
            EditorUtility.SetDirty(config);
        }

        if (!config.car.body)
        {
            config.car = new VehicleType
            {
                body = ImportSprite(CarBodyPath),
                size = new Vector2(176f, 72f),
                platePosition = new Vector2(-61f, -12f),
            };
            config.bus = new VehicleType
            {
                body = ImportSprite(BusBodyPath),
                size = new Vector2(240f, 80f),
                platePosition = new Vector2(-93f, -13f),
            };
        }

        config.car.details = LoadFrames(CarDetailsPath, WheelFrameCount);
        config.bus.details = LoadFrames(BusDetailsPath, WheelFrameCount);
        EditorUtility.SetDirty(config);
        return config;
    }

    static LicensePlate CreatePlatePrefab()
    {
        var root = new GameObject("LicensePlate");
        var body = root.AddComponent<Image>();
        body.raycastTarget = false;
        body.rectTransform.pivot = new Vector2(0.5f, 0f);

        var details = Child<Image>("Details", root.transform);
        details.raycastTarget = false;

        var plate = Child<Image>("Plate", root.transform);
        plate.sprite = ImportSprite(PlateSpritePath);
        plate.raycastTarget = false;
        plate.rectTransform.sizeDelta = new Vector2(52f, 18f);

        var cellSize = new Vector2(12f, 14f);
        for (int i = 0; i < 4; i++)
        {
            var cell = Child<Image>("Cell", plate.transform);
            cell.rectTransform.sizeDelta = cellSize;
            cell.rectTransform.anchoredPosition = new Vector2(i * 12f - 18f, 0f);
            cell.gameObject.AddComponent<PressButton>();
        }

        var licensePlate = root.AddComponent<LicensePlate>();
        Wire(licensePlate, ("details", details), ("plate", plate.rectTransform), ("luckySprite", ImportSprite(PlateLuckyPath)));
        WireArray(licensePlate, "digitSprites", LoadFrames(PlateDigitPath, 10));
        var prefab = PrefabUtility.SaveAsPrefabAsset(root, PrefabPath).GetComponent<LicensePlate>();
        Object.DestroyImmediate(root);
        return prefab;
    }

    static void BuildScene(GameConfig config, Sprite stripeSprite, LicensePlate prefab)
    {
        const string RootName = "Prototype";
        GameObject previous = GameObject.Find(RootName);
        if (previous) Object.DestroyImmediate(previous);
        Transform root = new GameObject(RootName).transform;

        Child<EventSystem>("EventSystem", root).gameObject.AddComponent<InputSystemUIInputModule>();

        var run = Child<CommuteRun>("Run", root);
        var combo = run.gameObject.AddComponent<ComboMeter>();
        Wire(combo, ("config", config));
        Wire(run, ("config", config), ("combo", combo));

        var road = Child<RoadScroller>("Road", root);
        Wire(road, ("config", config), ("run", run));
        Sprite sidewalk = ImportSprite(SidewalkPath);
        for (int i = 0; i < 7; i++)
        {
            float x = i * 4f - 12f;
            Art("Sidewalk", road.transform, sidewalk, new Vector2(x, 0.5f), -2);
            Art("Stripe", road.transform, stripeSprite, new Vector2(x, -2.5f), -1)
                .transform.localScale = new Vector3(0.75f, 0.1f, 1f);
        }

        BuildCity(root, run, config);

        var asphalt = Art("Asphalt", root, stripeSprite, new Vector2(0f, -2.95f), -3);
        asphalt.color = new Color32(86, 74, 98, 255);
        asphalt.transform.localScale = new Vector3(11f, 5.4f, 1f);

        NumberSprites hudDay = BuildUi(root, run, config, prefab);
        BuildScreens(root, run, config, hudDay);
        AddBumpFeedback();
        BuildAudio(root);
    }

    static void AddBumpFeedback()
    {
        var pedestrian = Object.FindAnyObjectByType<Pedestrian>();
        var shake = Object.FindAnyObjectByType<ScreenShake>();
        if (!shake) shake = Child<ScreenShake>("ScreenShake", pedestrian.transform.root);
        Wire(shake, ("config", AssetDatabase.LoadAssetAtPath<GameConfig>(ConfigPath)), ("worldCamera", Camera.main.transform),
            ("overlay", Object.FindAnyObjectByType<PlateSpawner>().transform));

        var body = (Image)new SerializedObject(pedestrian).FindProperty("body").objectReferenceValue;
        Transform warning = body.transform.Find("Warning");
        if (!warning) warning = CreateWarning(body.transform);

        Wire(pedestrian, ("warning", warning.gameObject), ("player", Object.FindAnyObjectByType<PlayerWalk>()), ("shake", shake));
        EditorSceneManager.MarkSceneDirty(pedestrian.gameObject.scene);
    }

    static Transform CreateWarning(Transform parent)
    {
        Vector2 center = Vector2.one * 0.5f;
        var warning = Ui<TextMeshProUGUI>("Warning", parent, center, center, new Vector2(20f, 28f));
        warning.text = "!";
        warning.fontSize = 24f;
        warning.alignment = TextAlignmentOptions.Center;
        warning.color = new Color32(214, 69, 65, 255);
        warning.raycastTarget = false;
        warning.rectTransform.anchoredPosition = new Vector2(0f, 40f);
        warning.gameObject.SetActive(false);
        return warning.transform;
    }

    static void BuildCity(Transform root, CommuteRun run, GameConfig config)
    {
        Sprite sky = ImportSprite(SkyPath);
        float skyY = SidewalkTopY + sky.bounds.extents.y;
        Art("Sky", root, sky, new Vector2(0f, skyY), -8);
        BuildScrollingLayer("Skyline", root, run, config, ImportSprite(SkylinePath), skyY, -7, 0.3f);
        BuildScrollingLayer("Props", root, run, config, ImportSprite(PropsPath), PropsCenterY, 0, 1f);

        Sprite[] streets = LoadStreets(BuildingsPath);
        Sprite[] glowSprites = LoadStreets(GlowPath);
        var animSprites = new Sprite[StreetNames.Length * 3];
        for (int i = 0; i < animSprites.Length; i++)
            animSprites[i] = ImportSprite(string.Format(BuildingsAnimPath, StreetNames[i / 3], i % 3));

        var row = Child<BuildingRow>("Buildings", root);
        var blocks = new SpriteRenderer[2];
        var glows = new SpriteRenderer[2];
        var overlays = new SpriteRenderer[2];
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i] = Art("Block", row.transform, streets[0], new Vector2(i * 20f, skyY), -3);
            glows[i] = Art("Glow", blocks[i].transform, glowSprites[0], Vector2.zero, -6);
            overlays[i] = Art("Overlay", blocks[i].transform, animSprites[0], Vector2.zero, -2);
        }

        Wire(row, ("config", config), ("run", run));
        WireArray(row, "blocks", blocks);
        WireArray(row, "glows", glows);
        WireArray(row, "overlays", overlays);
        WireArray(row, "streets", streets);
        WireArray(row, "glowSprites", glowSprites);
        WireArray(row, "animSprites", animSprites);
    }

    static Sprite[] LoadStreets(string pathFormat)
    {
        var sprites = new Sprite[StreetNames.Length];
        for (int i = 0; i < sprites.Length; i++)
            sprites[i] = ImportSprite(string.Format(pathFormat, StreetNames[i]));
        return sprites;
    }

    static void BuildScrollingLayer(string name, Transform root, CommuteRun run, GameConfig config, Sprite sprite, float y, int order, float speedScale)
    {
        var layer = Child<RoadScroller>(name, root);
        WireScroller(layer, run, config, 20f, speedScale);
        for (int i = 0; i < 2; i++)
            Art("Block", layer.transform, sprite, new Vector2(i * 20f, y), order);
    }

    static void WireScroller(RoadScroller scroller, CommuteRun run, GameConfig config, float tileWidth, float speedScale)
    {
        Wire(scroller, ("config", config), ("run", run));
        var serialized = new SerializedObject(scroller);
        serialized.FindProperty("tileWidth").floatValue = tileWidth;
        serialized.FindProperty("speedScale").floatValue = speedScale;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    static void BuildForeground(Transform items, CommuteRun run, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        Sprite sprite = ImportSprite(ForegroundPath);
        var scroller = Child<RoadScroller>("Foreground", items);
        scroller.gameObject.AddComponent<RectTransform>();
        WireScroller(scroller, run, config, 640f, 1.5f * GameConfig.PixelsPerUnit);
        scroller.transform.SetSiblingIndex(Object.FindAnyObjectByType<PlayerWalk>().transform.GetSiblingIndex() + 1);

        for (int i = 0; i < 2; i++)
        {
            var image = Ui<Image>("Block", scroller.transform, center, center, sprite.rect.size);
            image.sprite = sprite;
            image.raycastTarget = false;
            image.rectTransform.anchoredPosition = new Vector2(i * 640f, 85f);
        }
    }

    static void BuildMotorbike(Transform items, CommuteRun run, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        Sprite[] frames = LoadFrames(MotorbikePath, 2);
        var root = Child<RectTransform>("Motorbike", items);
        var body = Ui<Image>("Body", root, center, center, frames[0].rect.size);
        body.sprite = frames[0];
        body.raycastTarget = false;
        body.gameObject.SetActive(false);

        var motorbike = root.gameObject.AddComponent<Motorbike>();
        Wire(motorbike, ("config", config), ("run", run), ("spawner", items.GetComponent<PlateSpawner>()), ("body", body));
        WireArray(motorbike, "frames", frames);
    }

    static void BuildChest(Transform items, CommuteRun run, GameConfig config, Inventory inventory, CloverCharm clover)
    {
        Vector2 center = Vector2.one * 0.5f;
        var root = Child<RectTransform>("ChestStand", items);
        root.SetSiblingIndex(Object.FindAnyObjectByType<PlayerWalk>().transform.GetSiblingIndex());

        Sprite closed = ImportSprite(ChestClosedPath);
        var chest = Ui<Image>("Chest", root, center, center, closed.rect.size);
        chest.sprite = closed;
        var button = chest.gameObject.AddComponent<PressButton>();
        button.transition = Selectable.Transition.None;
        chest.gameObject.SetActive(false);

        Wire(root.gameObject.AddComponent<ChestStand>(), ("config", config), ("run", run), ("inventory", inventory), ("clover", clover),
            ("button", button), ("chest", chest), ("closedSprite", closed), ("openSprite", ImportSprite(ChestOpenPath)));
    }

    static void BuildFortuneTeller(Transform items, CommuteRun run, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        var root = Child<RectTransform>("FortuneStand", items);
        root.SetSiblingIndex(Object.FindAnyObjectByType<PlayerWalk>().transform.GetSiblingIndex());

        Sprite idle = ImportSprite(FortuneTellerPath);
        var teller = Ui<Image>("Teller", root, center, center, idle.rect.size);
        teller.sprite = idle;
        var button = teller.gameObject.AddComponent<PressButton>();
        button.transition = Selectable.Transition.None;
        teller.gameObject.SetActive(false);

        // The card floats beside the teller, on the side facing the player.
        Sprite glowSprite = ImportSprite(CardGlowPath);
        var glow = Ui<Image>("CardGlow", teller.transform, center, center, glowSprite.rect.size);
        glow.sprite = glowSprite;
        glow.raycastTarget = false;
        glow.rectTransform.anchoredPosition = new Vector2(-36f, 26f);
        var cardSprites = new Sprite[CardPaths.Length];
        for (int i = 0; i < cardSprites.Length; i++) cardSprites[i] = ImportSprite(CardPaths[i]);
        var card = Ui<Image>("Card", glow.transform, center, center, cardSprites[0].rect.size);
        card.raycastTarget = false;
        glow.gameObject.SetActive(false);

        var fortune = root.gameObject.AddComponent<FortuneTeller>();
        Wire(fortune, ("config", config), ("run", run), ("button", button), ("teller", teller), ("idleSprite", idle),
            ("readingSprite", ImportSprite(FortuneTellerLitPath)), ("glow", glow), ("card", card));
        WireArray(fortune, "cardSprites", cardSprites);
    }

    static void BuildMagpie(Transform items, CommuteRun run, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        Sprite[] frames = LoadFrames(MagpiePath, 2);
        var root = Child<RectTransform>("Magpie", items);

        // A clear image wider than the bird makes the moving target fair to click.
        var bird = Ui<Image>("Bird", root, center, center, new Vector2(48f, 40f));
        bird.color = Color.clear;
        var button = bird.gameObject.AddComponent<PressButton>();
        button.transition = Selectable.Transition.None;

        var body = Ui<Image>("Body", bird.transform, center, center, frames[0].rect.size);
        body.sprite = frames[0];
        body.raycastTarget = false;

        var magpie = root.gameObject.AddComponent<Magpie>();
        Wire(magpie, ("config", config), ("run", run), ("bird", bird.rectTransform), ("button", button), ("body", body));
        WireArray(magpie, "frames", frames);
    }

    static void BuildComboHud(Transform parent, ComboMeter combo, GameConfig config)
    {
        var root = (RectTransform)Child<Canvas>("Combo", parent).transform;
        root.anchorMin = Vector2.zero;
        root.anchorMax = Vector2.one;
        root.sizeDelta = Vector2.zero;

        var topLeft = new Vector2(0f, 1f);
        Sprite pipOff = ImportSprite(ComboPipOffPath);
        var pips = new Image[3];
        for (int i = 0; i < pips.Length; i++)
        {
            pips[i] = Ui<Image>("Pip", root, topLeft, topLeft, pipOff.rect.size);
            pips[i].sprite = pipOff;
            pips[i].raycastTarget = false;
            pips[i].rectTransform.anchoredPosition = new Vector2(24f + i * 20f, -66f);
        }

        Sprite x2 = ImportSprite(ComboX2Path);
        var multiplier = Ui<Image>("Multiplier", root, topLeft, topLeft, x2.rect.size);
        multiplier.sprite = x2;
        multiplier.raycastTarget = false;
        multiplier.rectTransform.anchoredPosition = new Vector2(92f, -66f);

        var top = new Vector2(0.5f, 1f);
        Sprite feverSprite = ImportSprite(FeverPath);
        var fever = Ui<Image>("Fever", root, top, top, feverSprite.rect.size);
        fever.sprite = feverSprite;
        fever.raycastTarget = false;
        fever.rectTransform.anchoredPosition = new Vector2(0f, -62f);
        fever.gameObject.SetActive(false);

        var hud = root.gameObject.AddComponent<ComboHud>();
        Wire(hud, ("config", config), ("combo", combo), ("pipOn", ImportSprite(ComboPipOnPath)), ("pipOff", pipOff),
            ("multiplier", multiplier), ("x2", x2), ("x3", ImportSprite(ComboX3Path)), ("fever", fever.gameObject));
        WireArray(hud, "pips", pips);
    }

    static void BuildLuckPopups(Transform parent, CommuteRun run, GameConfig config)
    {
        var popups = Child<Canvas>("LuckPopups", parent).gameObject.AddComponent<LuckPopups>();
        Wire(popups, ("config", config), ("run", run), ("plus", ImportSprite(PopPlusPath)), ("minus", ImportSprite(PopMinusPath)));
        WireArray(popups, "digits", LoadFrames(PopDigitPath, 10));
    }

    static Canvas CreateCanvas(string name, Transform root)
    {
        var canvas = Child<Canvas>(name, root);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(640f, 360f);
        return canvas;
    }

    static void BuildScreens(Transform root, CommuteRun run, GameConfig config, NumberSprites hudDay)
    {
        Vector2 center = Vector2.one * 0.5f;
        var canvas = CreateCanvas("Screens", root);
        canvas.sortingOrder = 10;
        canvas.gameObject.AddComponent<GraphicRaycaster>();

        var title = Ui<Image>("Title", canvas.transform, Vector2.zero, Vector2.one, Vector2.zero);
        title.color = new Color32(20, 20, 40, 190);

        Sprite logoSprite = ImportSprite(LogoPath);
        var logo = Ui<Image>("Logo", title.transform, center, center, logoSprite.rect.size);
        logo.sprite = logoSprite;
        logo.raycastTarget = false;
        logo.rectTransform.anchoredPosition = new Vector2(0f, 50f);

        Sprite startSprite = ImportSprite(StartButtonPath);
        var start = Ui<Image>("Start", title.transform, center, center, startSprite.rect.size);
        start.sprite = startSprite;
        start.rectTransform.anchoredPosition = new Vector2(0f, -50f);
        var startButton = start.gameObject.AddComponent<Button>();
        startButton.transition = Selectable.Transition.SpriteSwap;
        startButton.spriteState = new SpriteState
        {
            highlightedSprite = ImportSprite(StartButtonHoverPath),
            pressedSprite = ImportSprite(StartButtonPressedPath),
        };

        Newspaper newspaper = BuildNewspaper(canvas.transform, config);

        var home = Ui<Image>("Home", canvas.transform, Vector2.zero, Vector2.one, Vector2.zero);
        home.sprite = ImportSprite(HomePath);

        var door = Ui<Image>("Door", home.transform, center, center, new Vector2(88f, 128f));
        door.sprite = ImportSprite(DoorPath);
        door.rectTransform.anchoredPosition = new Vector2(186f, -16f);
        var doorButton = door.gameObject.AddComponent<Button>();
        doorButton.transition = Selectable.Transition.None;

        var player = Ui<Image>("Player", home.transform, center, center, new Vector2(64f, 92f));
        player.sprite = ImportSprite(string.Format(PlayerFramePath, 1));
        player.rectTransform.anchoredPosition = new Vector2(90f, -34f);
        player.raycastTarget = false;

        var hint = Label("Hint", home.transform, "CLICK THE DOOR", 16f, new Vector2(0f, 160f));
        NumberSprites tvQuota = BuildNumber("TvQuota", home.transform, LoadFrames(TvDigitPath, 10), 2, 20f, new Vector2(-8.5f, 8.5f), null);
        home.gameObject.SetActive(false);

        OfficeScene office = BuildOffice(canvas.transform, run, config);

        var dayCard = Ui<Image>("DayCard", canvas.transform, Vector2.zero, Vector2.one, Vector2.zero);
        dayCard.color = new Color32(28, 18, 48, 255);
        Sprite dayLabel = ImportSprite(DayLabelPath);
        var label = Ui<Image>("Label", dayCard.transform, center, center, dayLabel.rect.size);
        label.sprite = dayLabel;
        label.rectTransform.anchoredPosition = new Vector2(-22f, 0f);
        NumberSprites dayNumber = BuildNumber("Number", dayCard.transform, LoadFrames(DayDigitPath, 10), 2, 36f, new Vector2(58f, 0f), null);
        ResultScreen result = BuildResult(canvas.transform, run);
        dayCard.gameObject.SetActive(false);

        Wire(canvas.gameObject.AddComponent<GameFlow>(),
            ("config", config), ("run", run), ("title", title.gameObject), ("home", home.gameObject), ("dayCard", dayCard.gameObject),
            ("startButton", startButton), ("doorButton", doorButton), ("door", door), ("doorOpen", ImportSprite(DoorOpenPath)),
            ("hint", hint), ("tvQuota", tvQuota), ("dayNumber", dayNumber), ("hudDay", hudDay),
            ("result", result), ("office", office), ("newspaper", newspaper));
    }

    static readonly Color32 PaperColor = new Color32(236, 228, 210, 255);
    static readonly Color32 InkColor = new Color32(40, 34, 44, 255);
    static readonly Color32 SmudgeColor = new Color32(160, 150, 150, 255);

    static Newspaper BuildNewspaper(Transform parent, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        // Own canvas: the paper zooms every frame, so only this part gets rebuilt.
        var canvas = Child<Canvas>("Newspaper", parent);
        canvas.gameObject.AddComponent<GraphicRaycaster>();
        var root = (RectTransform)canvas.transform;
        root.anchorMin = Vector2.zero;
        root.anchorMax = Vector2.one;
        root.sizeDelta = Vector2.zero;

        var backdrop = Ui<Image>("Backdrop", root, Vector2.zero, Vector2.one, Vector2.zero);
        backdrop.color = new Color32(28, 18, 48, 255);
        var button = backdrop.gameObject.AddComponent<Button>();
        button.transition = Selectable.Transition.None;

        var paper = Ui<Image>("Paper", backdrop.transform, center, center, new Vector2(520f, 330f));
        paper.color = PaperColor;
        Transform page = paper.transform;

        NewsText("Masthead", page, "THE DAILY GRIND", 30f, new Vector2(0f, 135f), 500f).fontStyle = FontStyles.Bold;
        Bar(page, new Vector2(0f, 115f), new Vector2(500f, 2f), InkColor);
        NewsText("Edition", page, "VOL. 333   -   MONDAY MORNING EDITION   -   300 WON", 8f, new Vector2(0f, 106f), 500f);
        Bar(page, new Vector2(0f, 98f), new Vector2(500f, 2f), InkColor);

        NewsText("Headline", page, "CITY TRAFFIC JAM HITS RECORD HIGH", 17f, new Vector2(-80f, 80f), 340f).fontStyle = FontStyles.Bold;
        Bar(page, new Vector2(-170f, 22f), new Vector2(150f, 90f), SmudgeColor);
        Filler(page, -8f, 60f, 160f, 13);
        NewsText("Story2", page, "MAGPIES SPOTTED DOWNTOWN", 11f, new Vector2(-80f, -40f), 340f).fontStyle = FontStyles.Bold;
        Filler(page, -80f, -55f, 330f, 16);

        Bar(page, new Vector2(100f, -10f), new Vector2(1f, 210f), InkColor);
        NewsText("Story3", page, "STOCKS FLAT AGAIN", 11f, new Vector2(178f, 78f), 150f).fontStyle = FontStyles.Bold;
        Filler(page, 178f, 64f, 140f, 20);

        // The tiny article is unreadable until the paper zooms in on it.
        var article = Bar(page, new Vector2(178f, -120f), new Vector2(124f, 70f), InkColor);
        Bar(article.transform, Vector2.zero, new Vector2(122f, 68f), PaperColor);
        NewsText("Title", article.transform, "LUCKY 3s TURN LIFE AROUND", 7f, new Vector2(0f, 26f), 116f).fontStyle = FontStyles.Bold;
        TextMeshProUGUI body = NewsText("Body", article.transform,
            "A local office worker grabbed every lucky 3 spotted on the way to work. " +
            "\"Then everything just went right!\" they told us. Readers are urged to try it.",
            5f, new Vector2(0f, -6f), 116f);
        body.rectTransform.sizeDelta = new Vector2(116f, 46f);
        body.alignment = TextAlignmentOptions.TopLeft;

        var box = Ui<Image>("Dialogue", root, center, center, new Vector2(600f, 52f));
        box.color = new Color32(28, 18, 48, 230);
        box.raycastTarget = false;
        box.rectTransform.anchoredPosition = new Vector2(0f, -152f);
        TextMeshProUGUI line = Label("Line", box.transform, "", 13f, new Vector2(0f, 4f));
        line.rectTransform.sizeDelta = new Vector2(570f, 40f);
        TextMeshProUGUI tip = Label("Tip", box.transform, "CLICK >", 8f, new Vector2(0f, -17f));
        tip.rectTransform.sizeDelta = new Vector2(580f, 12f);
        tip.alignment = TextAlignmentOptions.Right;
        canvas.gameObject.SetActive(false);

        var newspaper = canvas.gameObject.AddComponent<Newspaper>();
        Wire(newspaper, ("config", config), ("button", button), ("paper", paper.rectTransform),
            ("article", article.rectTransform), ("line", line));
        return newspaper;
    }

    static TextMeshProUGUI NewsText(string name, Transform parent, string text, float size, Vector2 position, float width)
    {
        TextMeshProUGUI label = Label(name, parent, text, size, position);
        label.rectTransform.sizeDelta = new Vector2(width, size * 1.4f);
        label.color = InkColor;
        return label;
    }

    static Image Bar(Transform parent, Vector2 position, Vector2 size, Color32 color)
    {
        Vector2 center = Vector2.one * 0.5f;
        var bar = Ui<Image>("Bar", parent, center, center, size);
        bar.color = color;
        bar.raycastTarget = false;
        bar.rectTransform.anchoredPosition = position;
        return bar;
    }

    // Grey strokes stand in for body text the player never needs to read.
    static void Filler(Transform parent, float x, float top, float width, int rows)
    {
        for (int i = 0; i < rows; i++)
        {
            float rowWidth = i % 5 == 4 ? width * 0.6f : width;
            Bar(parent, new Vector2(x - (width - rowWidth) * 0.5f, top - i * 6f), new Vector2(rowWidth, 2f), SmudgeColor);
        }
    }

    static OfficeScene BuildOffice(Transform parent, CommuteRun run, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        var background = Ui<Image>("Office", parent, Vector2.zero, Vector2.one, Vector2.zero);
        background.sprite = ImportSprite(OfficePath);
        var continueButton = background.gameObject.AddComponent<Button>();
        continueButton.transition = Selectable.Transition.None;

        Sprite[] angry = LoadFrames(BossAngryPath, 2);
        var boss = Ui<Image>("Boss", background.transform, center, center, angry[0].rect.size * 2f);
        boss.sprite = angry[0];
        boss.raycastTarget = false;
        boss.rectTransform.anchoredPosition = new Vector2(170f, -14f);

        Sprite deskSprite = ImportSprite(OfficeDeskPath);
        var desk = Ui<Image>("Desk", background.transform, center, center, deskSprite.rect.size);
        desk.sprite = deskSprite;
        desk.raycastTarget = false;
        desk.rectTransform.anchoredPosition = new Vector2(170f, -88f);

        var player = Ui<Image>("Player", background.transform, center, center, new Vector2(64f, 92f));
        player.sprite = ImportSprite(string.Format(PlayerFramePath, 1));
        player.raycastTarget = false;
        player.rectTransform.anchoredPosition = new Vector2(-120f, -74f);

        var top = new Vector2(0.5f, 1f);
        var banner = Ui<Image>("Banner", background.transform, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, 108f));
        banner.color = new Color32(28, 18, 48, 200);
        banner.raycastTarget = false;
        banner.rectTransform.pivot = top;

        var headline = Ui<Image>("Headline", background.transform, center, center, Vector2.zero);
        headline.raycastTarget = false;
        headline.rectTransform.anchoredPosition = new Vector2(0f, 150f);
        NumberSprites pay = BuildNumber("Pay", background.transform, LoadFrames(PayDigitPath, 10), 6, 20f, new Vector2(0f, 104f), ImportSprite(PayWonPath));
        Label("Tip", background.transform, "CLICK TO CONTINUE", 14f, new Vector2(0f, -165f));
        background.gameObject.SetActive(false);

        var office = background.gameObject.AddComponent<OfficeScene>();
        Wire(office, ("config", config), ("run", run), ("continueButton", continueButton), ("boss", boss), ("headline", headline),
            ("clearHeadline", ImportSprite(QuotaClearPath)), ("failHeadline", ImportSprite(QuotaFailPath)), ("pay", pay));
        WireArray(office, "angryFrames", angry);
        WireArray(office, "happyFrames", LoadFrames(BossHappyPath, 2));
        return office;
    }

    static TextMeshProUGUI Label(string name, Transform parent, string text, float size, Vector2 position)
    {
        Vector2 center = Vector2.one * 0.5f;
        var label = Ui<TextMeshProUGUI>(name, parent, center, center, new Vector2(600f, 24f));
        label.text = text;
        label.fontSize = size;
        label.alignment = TextAlignmentOptions.Center;
        label.color = new Color32(251, 241, 234, 255);
        label.raycastTarget = false;
        label.rectTransform.anchoredPosition = position;
        return label;
    }

    static NumberSprites BuildNumber(string name, Transform parent, Sprite[] sprites, int slots, float step, Vector2 position, Sprite prefixSprite)
    {
        Vector2 center = Vector2.one * 0.5f;
        var root = Child<RectTransform>(name, parent);
        root.anchorMin = root.anchorMax = center;
        root.sizeDelta = Vector2.zero;
        root.anchoredPosition = position;

        Image prefix = null;
        if (prefixSprite)
        {
            prefix = Ui<Image>("Prefix", root, center, center, prefixSprite.rect.size);
            prefix.sprite = prefixSprite;
            prefix.raycastTarget = false;
        }

        var digits = new Image[slots];
        for (int i = 0; i < slots; i++)
        {
            digits[i] = Ui<Image>("Digit", root, center, center, sprites[0].rect.size);
            digits[i].raycastTarget = false;
        }

        var number = root.gameObject.AddComponent<NumberSprites>();
        Wire(number, ("prefix", prefix));
        WireArray(number, "digits", digits);
        WireArray(number, "sprites", sprites);
        var serialized = new SerializedObject(number);
        serialized.FindProperty("step").floatValue = step;
        serialized.ApplyModifiedPropertiesWithoutUndo();
        return number;
    }

    static void BuildPlayer(Transform items, CommuteRun run, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        Sprite[] frames = LoadFrames(PlayerFramePath, PlayerFrameCount);

        var body = Ui<Image>("Player", items, center, center, frames[0].rect.size);
        body.sprite = frames[0];
        body.raycastTarget = false;
        body.rectTransform.anchoredPosition =
            new Vector2(-5.5f, PlayerFeetY + frames[0].bounds.extents.y) * GameConfig.PixelsPerUnit;

        int signIndex = Object.FindAnyObjectByType<SignStand>().transform.GetSiblingIndex();
        int slotIndex = Object.FindAnyObjectByType<SlotStand>().transform.GetSiblingIndex();
        body.transform.SetSiblingIndex(Mathf.Max(signIndex, slotIndex) + 1);

        var dust = Child<RectTransform>("WalkDust", items).gameObject.AddComponent<WalkDust>();
        dust.transform.SetSiblingIndex(body.transform.GetSiblingIndex());
        Wire(dust, ("config", config));
        WireArray(dust, "frames", LoadFrames(DustPath, 3));

        var walk = body.gameObject.AddComponent<PlayerWalk>();
        Wire(walk, ("config", config), ("run", run), ("body", body), ("dust", dust));
        WireArray(walk, "frames", frames);
    }

    static PickupFx BuildPickupFx(Transform parent, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        var root = Child<Canvas>("PickupFx", parent);
        var fx = root.gameObject.AddComponent<PickupFx>();

        Sprite bread = ImportSprite(BreadPath);
        var flyer = Ui<Image>("Flyer", root.transform, center, center, bread.rect.size);
        flyer.raycastTarget = false;
        flyer.gameObject.SetActive(false);

        Sprite sparkSprite = ImportSprite(SparkPath);
        var sparks = new RectTransform[8];
        for (int i = 0; i < sparks.Length; i++)
        {
            var spark = Ui<Image>("Spark", root.transform, center, center, sparkSprite.rect.size);
            spark.sprite = sparkSprite;
            spark.raycastTarget = false;
            spark.gameObject.SetActive(false);
            sparks[i] = spark.rectTransform;
        }

        Wire(fx, ("config", config), ("flyer", flyer));
        WireArray(fx, "sparks", sparks);
        return fx;
    }

    static Sprite[] LoadFrames(string pathFormat, int count)
    {
        var frames = new Sprite[count];
        for (int i = 0; i < count; i++)
            frames[i] = ImportSprite(string.Format(pathFormat, i));
        return frames;
    }

    static Inventory BuildInventory(Transform parent)
    {
        Vector2 center = Vector2.one * 0.5f;
        var root = Child<RectTransform>("Inventory", parent);
        var icons = new Image[5];
        for (int i = 0; i < icons.Length; i++)
        {
            var slot = Ui<Image>("Slot", root, center, center, new Vector2(26f, 26f));
            slot.color = new Color32(20, 20, 30, 200);
            slot.rectTransform.anchoredPosition = new Vector2(i * 30f - 60f, -163f);

            icons[i] = Ui<Image>("Icon", slot.transform, center, center, new Vector2(22f, 22f));
            icons[i].preserveAspect = true;
            icons[i].enabled = false;
        }

        var inventory = root.gameObject.AddComponent<Inventory>();
        WireArray(inventory, "icons", icons);
        return inventory;
    }

    static void BuildPedestrian(Transform parent, CommuteRun run, GameConfig config, Inventory inventory, Sprite breadSprite, PickupFx fx)
    {
        Vector2 center = Vector2.one * 0.5f;
        Sprite[] walk = LoadFrames(PersonWalkPath, 4);
        Sprite[] fall = LoadFrames(PersonFallPath, 3);
        var root = Child<RectTransform>("Pedestrians", parent);

        var body = Ui<Image>("Person", root, center, center, walk[0].rect.size);
        body.sprite = walk[0];
        body.rectTransform.pivot = new Vector2(0.5f, 0f);
        var button = body.gameObject.AddComponent<Button>();

        var bubble = Ui<Image>("Bubble", body.transform, center, center, new Vector2(34f, 30f));
        bubble.sprite = ImportSprite(BubblePath);
        bubble.rectTransform.anchoredPosition = new Vector2(8f, 40f);
        var wanted = Ui<Image>("Bread", bubble.transform, center, center, breadSprite.rect.size);
        wanted.sprite = breadSprite;
        wanted.rectTransform.anchoredPosition = new Vector2(0f, 3f);
        bubble.gameObject.SetActive(false);

        Sprite wideSprite = ImportSprite(BubbleWidePath);
        var tradeBubble = Ui<Image>("TradeBubble", body.transform, center, center, wideSprite.rect.size);
        tradeBubble.sprite = wideSprite;
        tradeBubble.rectTransform.anchoredPosition = new Vector2(-4f, 40f);
        for (int i = 0; i < 2; i++)
        {
            var tradeBread = Ui<Image>("Bread", tradeBubble.transform, center, center, breadSprite.rect.size);
            tradeBread.sprite = breadSprite;
            tradeBread.rectTransform.anchoredPosition = new Vector2(i * 22f - 18f, 3f);
        }
        tradeBubble.gameObject.SetActive(false);

        var cloverBubble = Ui<Image>("CloverBubble", body.transform, center, center, bubble.rectTransform.sizeDelta);
        cloverBubble.sprite = bubble.sprite;
        cloverBubble.rectTransform.anchoredPosition = bubble.rectTransform.anchoredPosition;
        Sprite cloverSprite = ImportSprite(CloverPath);
        var wantedClover = Ui<Image>("Clover", cloverBubble.transform, center, center, cloverSprite.rect.size);
        wantedClover.sprite = cloverSprite;
        wantedClover.rectTransform.anchoredPosition = new Vector2(0f, 3f);
        cloverBubble.gameObject.SetActive(false);

        var reward = Ui<Image>("Reward", body.transform, center, center, new Vector2(26f, 18f));
        reward.sprite = ImportSprite(TicketPath);
        reward.rectTransform.anchoredPosition = new Vector2(0f, 38f);
        reward.gameObject.SetActive(false);
        body.gameObject.SetActive(false);

        var drop = Ui<Image>("BreadDrop", parent, center, center, breadSprite.rect.size);
        drop.sprite = breadSprite;
        var dropButton = drop.gameObject.AddComponent<Button>();
        var pickup = drop.gameObject.AddComponent<BreadPickup>();
        Wire(pickup, ("config", config), ("run", run), ("inventory", inventory), ("button", dropButton), ("fx", fx));
        drop.gameObject.SetActive(false);

        // Street lamps in the foreground must cover people walking behind them.
        Transform foreground = parent.Find("Foreground");
        root.SetSiblingIndex(foreground.GetSiblingIndex());
        drop.transform.SetSiblingIndex(foreground.GetSiblingIndex());

        var pedestrian = root.gameObject.AddComponent<Pedestrian>();
        Wire(pedestrian, ("config", config), ("run", run), ("inventory", inventory), ("bread", pickup), ("button", button),
            ("body", body), ("bubble", bubble.gameObject), ("tradeBubble", tradeBubble.gameObject), ("cloverBubble", cloverBubble.gameObject), ("reward", reward.gameObject),
            ("fx", fx), ("combo", run.GetComponent<ComboMeter>()), ("clover", run.GetComponent<CloverCharm>()));
        WireArray(pedestrian, "walkFrames", walk);
        WireArray(pedestrian, "fallFrames", fall);
    }

    static SpriteRenderer Art(string name, Transform parent, Sprite sprite, Vector2 position, int order)
    {
        var renderer = Child<SpriteRenderer>(name, parent);
        renderer.sprite = sprite;
        renderer.sortingOrder = order;
        renderer.transform.localPosition = position;
        return renderer;
    }

    static NumberSprites BuildUi(Transform root, CommuteRun run, GameConfig config, LicensePlate prefab)
    {
        var canvas = CreateCanvas("Hud", root);

        var items = Child<Canvas>("Items", canvas.transform);
        items.gameObject.AddComponent<GraphicRaycaster>();
        var combo = run.GetComponent<ComboMeter>();
        var plates = items.gameObject.AddComponent<PlateSpawner>();
        Wire(plates, ("config", config), ("run", run), ("prefab", prefab), ("combo", combo));
        BuildSign(items.transform, run, config);
        BuildSlot(items.transform, run, config);
        BuildPlayer(items.transform, run, config);
        BuildForeground(items.transform, run, config);

        (RectTransform marker, Image fill) = BuildTrack(canvas.transform);

        var luck = Child<RectTransform>("Luck", canvas.transform);
        luck.anchorMin = new Vector2(0.02f, 0.86f);
        luck.anchorMax = new Vector2(0.22f, 0.98f);
        luck.sizeDelta = Vector2.zero;
        luck.anchoredPosition = new Vector2(0.2f, -0.2f);

        Sprite[] digits = LoadFrames(HudDigitPath, 10);
        HudSprite("Panel", luck, ImportSprite(HudPanelPath), -6f);
        Image flash = HudSprite("Flash", luck, ImportSprite(HudFlashPath), -6f);
        flash.enabled = false;
        HudSprite("Title", luck, ImportSprite(HudLuckPath), 0f);
        var luckDigits = new Image[3];
        for (int i = 0; i < luckDigits.Length; i++)
            luckDigits[i] = HudSprite("Digit", luck, digits[0], 53f + i * 12f);

        Wire(canvas.gameObject.AddComponent<CommuteHud>(), ("run", run), ("marker", marker), ("fill", fill));

        var board = luck.gameObject.AddComponent<LuckScoreboard>();
        Wire(board, ("config", config), ("run", run), ("flash", flash));
        WireArray(board, "digits", luckDigits);
        WireArray(board, "digitSprites", digits);

        Sprite bread = ImportSprite(BreadPath);
        PickupFx pickupFx = BuildPickupFx(canvas.transform, config);
        Inventory inventory = BuildInventory(canvas.transform);
        // The clover badge sits just right of the five inventory slots.
        Vector2 center = Vector2.one * 0.5f;
        Sprite cloverSprite = ImportSprite(CloverPath);
        var badge = Ui<Image>("Clover", inventory.transform, center, center, cloverSprite.rect.size);
        badge.sprite = cloverSprite;
        badge.rectTransform.anchoredPosition = new Vector2(94f, -163f);
        badge.enabled = false;
        BuildCloverTooltip(badge);

        var clover = run.gameObject.AddComponent<CloverCharm>();
        Wire(clover, ("config", config), ("badge", badge), ("fx", pickupFx));
        Wire(run, ("clover", clover), ("inventory", inventory));
        BuildPedestrian(items.transform, run, config, inventory, bread, pickupFx);
        BuildChest(items.transform, run, config, inventory, clover);
        BuildFortuneTeller(items.transform, run, config);
        BuildMotorbike(items.transform, run, config);
        BuildMagpie(items.transform, run, config);
        Wire(plates, ("rain", BuildRain(canvas.transform, items.transform, run, config)));
        BuildComboHud(canvas.transform, combo, config);
        BuildLuckPopups(canvas.transform, run, config);
        pickupFx.transform.SetAsLastSibling();
        return BuildDayHud(canvas.transform);
    }

    static NumberSprites BuildDayHud(Transform parent)
    {
        var topRight = Vector2.one;
        Sprite panelSprite = ImportSprite(HudDayPanelPath);
        var panel = Ui<Image>("Day", parent, topRight, topRight, panelSprite.rect.size);
        panel.sprite = panelSprite;
        panel.raycastTarget = false;
        panel.rectTransform.pivot = topRight;
        panel.rectTransform.anchoredPosition = new Vector2(-12f, -40f);

        HudSprite("Title", panel.transform, ImportSprite(HudDayPath), 6f);
        // The digit slot spans x 46-56 of the 63px panel; NumberSprites centers on its transform.
        NumberSprites day = BuildNumber("Number", panel.transform, LoadFrames(HudDigitPath, 10), 1, 12f, new Vector2(20f, 0f), null);
        day.Show(1);
        return day;
    }

    static (RectTransform marker, Image fill) BuildTrack(Transform parent)
    {
        var top = new Vector2(0.5f, 1f);
        var center = Vector2.one * 0.5f;
        var root = (RectTransform)Child<Canvas>("Track", parent).transform;
        root.anchorMin = root.anchorMax = top;
        root.anchoredPosition = new Vector2(0f, -24f);
        root.sizeDelta = Vector2.zero;

        Sprite barSprite = ImportSprite(TrackBarPath);
        var bar = Ui<Image>("Bar", root, center, center, barSprite.rect.size);
        bar.sprite = barSprite;
        bar.raycastTarget = false;

        Sprite fillSprite = ImportSprite(TrackFillPath);
        var fill = Ui<Image>("Fill", bar.transform, center, center, fillSprite.rect.size);
        fill.sprite = fillSprite;
        fill.type = Image.Type.Filled;
        fill.fillMethod = Image.FillMethod.Horizontal;
        fill.fillAmount = 0f;
        fill.raycastTarget = false;

        float half = barSprite.rect.width * 0.5f;
        TrackIcon("Home", root, ImportSprite(TrackHomePath), center, -half - 12f, 0f);
        TrackIcon("Office", root, ImportSprite(TrackOfficePath), center, half + 14f, -2f);

        var marker = TrackIcon("Marker", fill.transform, ImportSprite(TrackPlayerPath), new Vector2(0f, 0.5f), 0f, 0f);
        return (marker.rectTransform, fill);
    }

    static Image TrackIcon(string name, Transform parent, Sprite sprite, Vector2 anchor, float x, float y)
    {
        var icon = Ui<Image>(name, parent, anchor, anchor, sprite.rect.size);
        icon.sprite = sprite;
        icon.raycastTarget = false;
        // Odd-sized sprites need a half-pixel offset to land on the pixel grid.
        icon.rectTransform.anchoredPosition = new Vector2(x + sprite.rect.width % 2 * 0.5f, y + sprite.rect.height % 2 * 0.5f);
        return icon;
    }

    static Image HudSprite(string name, Transform parent, Sprite sprite, float x)
    {
        var anchor = new Vector2(0f, 0.5f);
        var image = Ui<Image>(name, parent, anchor, anchor, sprite.rect.size);
        image.sprite = sprite;
        image.raycastTarget = false;
        image.rectTransform.pivot = anchor;
        image.rectTransform.anchoredPosition = new Vector2(x, 0f);
        return image;
    }

    static void BuildSign(Transform parent, CommuteRun run, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        var root = Child<RectTransform>("SignStand", parent);

        Sprite signSprite = ImportSprite(SignPath);
        var sign = Ui<Image>("Sign", root, center, center, signSprite.rect.size);
        sign.sprite = signSprite;

        Sprite stainSprite = ImportSprite(StainPath);
        var stainImage = Ui<Image>("Stain", sign.transform, center, center, stainSprite.rect.size);
        stainImage.sprite = stainSprite;
        stainImage.rectTransform.anchoredPosition = new Vector2(0f, 14f);
        var stain = stainImage.gameObject.AddComponent<SignStain>();
        Wire(stain, ("config", config), ("run", run));
        sign.gameObject.SetActive(false);

        Wire(root.gameObject.AddComponent<SignStand>(),
            ("config", config), ("run", run), ("sign", sign.rectTransform), ("stain", stain));
    }

    static void BuildSlot(Transform parent, CommuteRun run, GameConfig config)
    {
        Vector2 center = Vector2.one * 0.5f;
        var standRoot = Child<RectTransform>("SlotStand", parent);

        var machine = Ui<Image>("Machine", standRoot, center, center, Vector2.zero);
        machine.sprite = ImportSprite(SlotPath);
        machine.rectTransform.sizeDelta = machine.sprite.rect.size;
        var machineButton = machine.gameObject.AddComponent<Button>();
        machine.gameObject.SetActive(false);

        var panel = Ui<Image>("SlotGame", parent, center, center, new Vector2(150f, 84f));
        panel.color = new Color32(36, 26, 48, 240);
        panel.rectTransform.anchoredPosition = new Vector2(0f, 60f);
        var stopButton = panel.gameObject.AddComponent<Button>();

        var reels = Child<RectTransform>("Reels", panel.transform);
        for (int i = 0; i < 3; i++)
        {
            var digit = Ui<Image>("Digit", reels, center, center, new Vector2(22f, 36f));
            digit.rectTransform.anchoredPosition = new Vector2(i * 36f - 36f, 9f);
            digit.raycastTarget = false;
        }

        var message = Ui<TextMeshProUGUI>("Message", panel.transform, center, center, new Vector2(140f, 18f));
        message.rectTransform.anchoredPosition = new Vector2(0f, -28f);
        message.fontSize = 11f;
        message.alignment = TextAlignmentOptions.Center;

        var game = panel.gameObject.AddComponent<SlotGame>();
        Wire(game, ("config", config), ("run", run), ("stopButton", stopButton), ("reels", reels), ("message", message));
        WireArray(game, "digitSprites", LoadFrames(HudDigitPath, 10));
        panel.gameObject.SetActive(false);

        Wire(standRoot.gameObject.AddComponent<SlotStand>(),
            ("config", config), ("run", run), ("game", game), ("button", machineButton), ("machine", machine.rectTransform));
    }

    static T Child<T>(string name, Transform parent) where T : Component
    {
        var component = new GameObject(name).AddComponent<T>();
        component.transform.SetParent(parent, false);
        return component;
    }

    static T Ui<T>(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 size) where T : Graphic
    {
        var graphic = Child<T>(name, parent);
        graphic.rectTransform.anchorMin = anchorMin;
        graphic.rectTransform.anchorMax = anchorMax;
        graphic.rectTransform.sizeDelta = size;
        graphic.rectTransform.anchoredPosition = Vector2.zero;
        return graphic;
    }

    static void WireArray(Object target, string field, Object[] values)
    {
        var serialized = new SerializedObject(target);
        SerializedProperty list = serialized.FindProperty(field);
        list.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
            list.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    static void Wire(Object target, params (string field, Object value)[] references)
    {
        var serialized = new SerializedObject(target);
        foreach ((string field, Object value) in references)
            serialized.FindProperty(field).objectReferenceValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }
}
