using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PlateSpawner : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] LicensePlate prefab;
    [SerializeField] ComboMeter combo;
    [SerializeField] RainDay rain;

    readonly List<LicensePlate> active = new();
    ObjectPool<LicensePlate> pool;
    float[] laneFreeAt;
    float timer;
    float wheelTimer;
    int wheelFrame;

    void Awake()
    {
        laneFreeAt = new float[config.itemLaneBottoms.Length];
        pool = new ObjectPool<LicensePlate>(
            Create,
            plate => plate.gameObject.SetActive(true),
            plate => plate.gameObject.SetActive(false));
        pool.Prewarm(config.platePoolSize);
        run.Started += ReleaseAll;
    }

    void ReleaseAll()
    {
        for (int i = active.Count - 1; i >= 0; i--) Release(i);
        timer = 0f;
    }

    void Update()
    {
        if (!run.Running) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = config.itemInterval * (rain.Raining ? config.rainItemIntervalScale : 1f);
            Spawn();
        }

        wheelTimer += Time.deltaTime;
        bool turnWheels = wheelTimer >= config.wheelFrameSeconds;
        if (turnWheels)
        {
            wheelTimer -= config.wheelFrameSeconds;
            wheelFrame++;
        }

        float step = config.itemSpeed * Time.deltaTime;
        for (int i = active.Count - 1; i >= 0; i--)
        {
            if (turnWheels) active[i].ShowWheelFrame(wheelFrame);
            RectTransform rect = active[i].Rect;
            rect.anchoredPosition += Vector2.left * step;
            if (rect.anchoredPosition.x < -config.vehicleEdgeX) Release(i);
        }
    }

    LicensePlate Create()
    {
        LicensePlate plate = Instantiate(prefab, transform);
        foreach (Button cell in plate.Cells)
            cell.onClick.AddListener(() => Collect(cell));
        return plate;
    }

    void Spawn()
    {
        int lane = PickFreeLane();
        if (lane < 0) return;

        LicensePlate plate = pool.Get();
        plate.Roll(config, combo.Fever);
        plate.Rect.anchoredPosition = new Vector2(config.vehicleEdgeX, config.itemLaneBottoms[lane]);
        Reserve(lane, plate.Rect.sizeDelta.x);
        active.Add(plate);
    }

    public bool TryReserveLane(float clearance, out float laneBottom)
    {
        laneBottom = 0f;
        int lane = PickFreeLane();
        if (lane < 0) return false;

        Reserve(lane, clearance);
        laneBottom = config.itemLaneBottoms[lane];
        return true;
    }

    void Reserve(int lane, float length) =>
        laneFreeAt[lane] = Time.time + (length + config.vehicleGap) / config.itemSpeed;

    int PickFreeLane()
    {
        int start = Random.Range(0, laneFreeAt.Length);
        for (int i = 0; i < laneFreeAt.Length; i++)
        {
            int lane = (start + i) % laneFreeAt.Length;
            if (Time.time >= laneFreeAt[lane]) return lane;
        }
        return -1;
    }

    void Collect(Button cell)
    {
        if (!run.Running) return;

        cell.image.enabled = false;
        run.AddLuck(config.cellPoints);
    }

    void Release(int index)
    {
        pool.Release(active[index]);
        active[index] = active[^1];
        active.RemoveAt(active.Count - 1);
    }
}
