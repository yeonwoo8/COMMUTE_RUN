using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotGame : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] Button stopButton;
    [SerializeField] Transform reels;
    [SerializeField] TMP_Text message;
    [SerializeField] Sprite[] digitSprites;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip spinTick;
    [SerializeField] AudioClip reelStop;
    [SerializeField] AudioClip jackpot;

    readonly int[] positions = new int[3];
    Image[] reelImages;
    int stopped;
    float timer;

    public bool IsOpen => gameObject.activeSelf;

    void Awake()
    {
        reelImages = reels.GetComponentsInChildren<Image>(true);
        stopButton.onClick.AddListener(StopNextReel);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        stopped = 0;
        timer = 0f;
        message.SetText("CLICK TO STOP");

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = Random.Range(0, config.slotStrip.Length);
            ShowReel(i);
        }
    }

    void Update()
    {
        if (!run.Running)
        {
            gameObject.SetActive(false);
            return;
        }

        timer += Time.deltaTime;
        if (stopped == positions.Length)
        {
            if (timer >= config.slotResultSeconds) gameObject.SetActive(false);
            return;
        }

        if (timer < config.slotSpinInterval) return;

        timer -= config.slotSpinInterval;
        sfx.PlayOneShot(spinTick);
        for (int i = stopped; i < positions.Length; i++)
        {
            positions[i] = (positions[i] + 1) % config.slotStrip.Length;
            ShowReel(i);
        }
    }

    void StopNextReel()
    {
        if (stopped == positions.Length) return;

        stopped++;
        sfx.PlayOneShot(reelStop);
        if (stopped < positions.Length) return;

        timer = 0f;
        int points = Score();
        if (points == config.slotPointsByThrees[^1]) sfx.PlayOneShot(jackpot);
        run.AddLuck(points);
        message.SetText(points > 0 ? "+{0}" : "{0}", points);
    }

    int Score()
    {
        int threes = 0, fours = 0;
        foreach (int position in positions)
        {
            char symbol = config.slotStrip[position];
            if (symbol == '3') threes++;
            else if (symbol == '4') fours++;
        }

        return fours == 3 ? config.slotTrapPoints : config.slotPointsByThrees[threes];
    }

    void ShowReel(int index)
    {
        reelImages[index].sprite = digitSprites[config.slotStrip[positions[index]] - '0'];
    }
}
