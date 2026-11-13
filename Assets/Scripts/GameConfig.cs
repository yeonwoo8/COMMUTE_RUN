using UnityEngine;

[System.Serializable]
public struct VehicleType
{
    public Sprite body;
    [Tooltip("Wheel frames; the hubs turn by cycling through them.")]
    public Sprite[] details;
    public Vector2 size;
    public Vector2 platePosition;
}

[CreateAssetMenu(menuName = "Jam/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Days")]
    [Tooltip("Luck needed per day. Its length is the number of days; a failed day is replayed.")]
    public int[] dailyQuotas = { 50, 60, 70 };
    public float dayCardSeconds = 1.6f;
    public int basePay = 30000;
    public int bonusPerLuck = 3000;
    public int failPayPerLuck = 300;
    public float payCountSeconds = 1.2f;
    public float bossFrameSeconds = 0.25f;

    [Header("Opening Newspaper")]
    public float newsZoomSeconds = 0.8f;
    public float newsZoomScale = 3f;
    [Tooltip("Pixels the zoomed article sits above center, clearing the dialogue box.")]
    public float newsZoomLift = 24f;

    [Header("Combo (삼세번)")]
    public int comboStreak = 3;
    public int comboMultiplier = 2;
    public int feverStreak = 9;
    public int feverMultiplier = 3;
    public float feverSeconds = 4f;
    public float comboIdleSeconds = 2.5f;
    public float comboPopSeconds = 0.2f;
    public float comboPopScale = 1.6f;
    public float feverBlinkSeconds = 0.08f;

    [Header("Key and Clover")]
    [Tooltip("Commute progress at which each two-bread trader appears.")]
    public float[] tradeProgresses = { 0.5f, 0.65f };
    public int breadsForKey = 2;
    public Sprite keyItem;
    [Range(0f, 1f)] public float chestProgress = 0.8f;
    public float chestCenterY = 24f;
    public float chestShakeSeconds = 0.25f;
    public float chestShakePixels = 2f;
    [Range(0f, 1f)] public float cloverChance = 0.33f;
    public int cloverMultiplier = 2;
    public float cloverPopSeconds = 0.25f;
    public float cloverPopScale = 1.6f;
    [Tooltip("Day with an extra chest early in the commute. It still needs the key.")]
    public int bonusChestDay = 2;
    [Range(0f, 1f)] public float bonusChestProgress = 0.15f;

    [Header("Clover Asker")]
    [Tooltip("Days on which someone asks to see the golden clover.")]
    public int[] cloverAskDays = { 2, 3 };
    [Tooltip("Comes after the regular chest so the clover can be won the same day.")]
    [Range(0f, 1f)] public float cloverAskProgress = 0.86f;
    public int cloverAskPoints = 10;

    [Header("Rain Day")]
    public int rainDay = 3;
    public float rainFadeSeconds = 0.6f;
    [Range(0f, 1f)] public float rainVolume = 0.25f;
    public Vector2 rainScroll = new(0.8f, 4f);
    [Tooltip("Plate spawn interval multiplier while it rains.")]
    public float rainItemIntervalScale = 1.8f;
    public float rainClearSeconds = 12f;
    public int umbrellaPoints = 5;
    public float umbrellaFirstSeconds = 6f;
    public float umbrellaIntervalSeconds = 10f;
    public float umbrellaY = 20f;

    [Header("Magpie")]
    public float magpieIntervalMin = 8f;
    public float magpieIntervalMax = 14f;
    public float magpieSpeed = 230f;
    public float magpieEdgeX = 340f;
    public float magpieY = 90f;
    public float magpieWave = 14f;
    public float magpieWaveSpeed = 7f;
    public float magpieFlapSeconds = 0.12f;
    public int magpiePoints = 2;

    [Header("Commute")]
    public float runSeconds = 60f;
    [Tooltip("Game speed at the end of the commute; ramps up from 1 as the player nears the office.")]
    public float endSpeedScale = 1.6f;
    public float scrollSpeed = 3f;
    public float walkFrameSeconds = 0.12f;
    public float cityAnimFrameSeconds = 0.12f;
    public float motorbikeInterval = 9f;
    public float doorOpenSeconds = 0.5f;

    [Header("Plates (canvas units, 640x360)")]
    public float itemInterval = 1.2f;
    public float itemSpeed = 160f;
    public float wheelFrameSeconds = 0.1f;
    public float offscreenEdgeX = 420f;
    public Color[] carColors =
    {
        new Color32(214, 69, 65, 255),
        new Color32(58, 123, 213, 255),
        new Color32(244, 196, 48, 255),
        new Color32(47, 163, 107, 255),
        new Color32(240, 240, 235, 255),
        new Color32(28, 30, 38, 255),
        new Color32(240, 130, 50, 255),
    };
    public float[] itemLaneBottoms = { -156f, -76f };
    public float vehicleEdgeX = 460f;
    public float vehicleGap = 40f;
    public VehicleType car;
    public VehicleType bus;
    [Range(0f, 1f)] public float busChance = 0.2f;

    [Tooltip("Index = number of winning cells (0-3). Index 4 = plate showing a 4 next to 1-2 winning cells.")]
    public int[] plateWeights = { 35, 30, 10, 5, 20 };
    public int cellPoints = 1;

    [Header("Slot")]
    public float slotInterval = 15f;
    public float slotCenterY = 38f;
    public float slotSpinInterval = 0.12f;
    public float slotResultSeconds = 1f;
    public string slotStrip = "3541374389";
    // Indexed by how many 3s the reels show: none, one, two, 333.
    public int[] slotPointsByThrees = { 0, 3, 6, 33 };
    public int slotTrapPoints = -5;

    [Header("Pedestrians and Bread")]
    public float pedestrianInterval = 4f;
    public float pedestrianEdgeX = 360f;
    public float pedestrianWalkSpeed = 40f;
    public float pedestrianFeetY = 5f;
    public float bumpX = -156f;
    public Sprite breadItem;
    public int maxBreadDroppers = 4;
    public int maxBreadAskers = 1;
    [Range(0f, 1f)] public float dropChance = 0.6f;
    [Range(0f, 1f)] public float askChance = 0.6f;
    public int breadRewardPoints = 3;
    public float breadDropOffsetX = 80f;
    public float breadY = 12f;

    [Header("Luck Scoreboard")]
    public float luckRollStepSeconds = 0.05f;
    public float luckBounceSeconds = 0.12f;
    public float luckBouncePixels = 3f;
    public float luckShakeSeconds = 0.25f;
    public float luckShakePixels = 2f;
    public float luckMilestoneSeconds = 0.9f;
    public float luckBlinkSeconds = 0.09f;
    public Color luckDropColor = new Color32(255, 90, 100, 255);

    [Header("Luck Popups")]
    public float popupSeconds = 0.7f;
    public float popupRisePixels = 26f;
    public Vector2 popupOffset = new(10f, 14f);
    public float popupJitter = 6f;
    [Range(0f, 1f)] public float popupBlinkFrom = 0.75f;
    public float popupEdgeX = 308f;
    public float popupEdgeY = 150f;
    public Color popupGainColor = new Color32(255, 224, 138, 255);
    [Tooltip("Popup colors for boosted gains: x2 (combo), x3 (fever), x4 and up (clover on top).")]
    public Color[] popupBoostColors =
    {
        new Color32(255, 150, 70, 255),
        new Color32(73, 232, 255, 255),
        new Color32(255, 79, 163, 255),
    };

    [Header("Bump")]
    public float bumpFreezeSeconds = 0.09f;
    public float bumpWarnDistance = 60f;
    public float bumpFlySpeed = 240f;
    public float bumpFlySeconds = 0.35f;
    public float bumpFlyHeight = 28f;
    public float bumpKnockbackPixels = 6f;
    public float bumpStumbleSeconds = 0.2f;
    public float bumpBlinkSeconds = 0.05f;
    public Color bumpFlashColor = new Color32(255, 110, 110, 255);
    public float bumpShakePixels = 3f;
    public float bumpShakeSeconds = 0.15f;
    public float bumpShakeSpeed = 90f;

    [Header("Pool Prewarm")]
    public int platePoolSize = 8;
    public int popupPoolSize = 6;
    public int dustPoolSize = 4;

    [Header("Walk Dust")]
    public float dustFrameSeconds = 0.08f;
    public float dustRisePixels = 3f;
    public Vector2 dustOffset = new(-10f, -20f);

    [Header("Pickup")]
    public float pickupFlySeconds = 0.4f;
    public float pickupArcPixels = 40f;
    public float pickupStartScale = 1.4f;
    public float slotPopSeconds = 0.18f;
    public float slotPopScale = 1.5f;
    public float sparkSeconds = 0.3f;
    public float sparkDistance = 20f;

    [Header("Sign")]
    public float signInterval = 20f;
    public float signCenterY = 48f;
    public int signShakesNeeded = 6;
    public int signPoints = 3;
    public float shakeMinDelta = 2f;
    public float stainWiggle = 2f;
    public float stainFallSpeed = 160f;
    public float stainFallDistance = 60f;

    public const float PixelsPerUnit = 32f;

    public float GroundPixelsPerSecond => scrollSpeed * PixelsPerUnit;

    public VehicleType PickVehicle() => Random.value < busChance ? bus : car;

    public int PickPlateTier()
    {
        int total = 0;
        foreach (int weight in plateWeights) total += weight;

        int roll = Random.Range(0, total);
        for (int i = 0; i < plateWeights.Length; i++)
        {
            roll -= plateWeights[i];
            if (roll < 0) return i;
        }
        return 0;
    }
}
