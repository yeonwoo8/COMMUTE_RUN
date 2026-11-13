using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Title -> home -> commute -> office -> day card -> home ...
// A failed day is replayed; clearing the last day shows the result screen, then the title.
public class GameFlow : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] GameObject title;
    [SerializeField] GameObject home;
    [SerializeField] GameObject dayCard;
    [SerializeField] Button startButton;
    [SerializeField] Button doorButton;
    [SerializeField] Image door;
    [SerializeField] Sprite doorOpen;
    [SerializeField] TMP_Text hint;
    [SerializeField] NumberSprites tvQuota;
    [SerializeField] NumberSprites dayNumber;
    [SerializeField] NumberSprites hudDay;
    [SerializeField] ResultScreen result;
    [SerializeField] OfficeScene office;
    [SerializeField] Newspaper newspaper;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip click;
    [SerializeField] AudioClip doorSound;

    Sprite doorClosed;

    void Awake()
    {
        doorClosed = door.sprite;
        startButton.onClick.AddListener(ShowNewspaper);
        newspaper.Done += ShowHome;
        doorButton.onClick.AddListener(OpenDoor);
        run.Finished += office.Show;
        office.Done += ShowDayCard;
        result.Done += ShowTitle;
    }

    void ShowNewspaper()
    {
        sfx.PlayOneShot(click);
        title.SetActive(false);
        newspaper.Show();
    }

    void ShowHome()
    {
        dayCard.SetActive(false);
        door.sprite = doorClosed;
        tvQuota.Show(run.Quota);
        hudDay.Show(run.Day);
        hint.SetText("DAY {0:0}  -  LUCK GOAL {1:0}  -  CLICK THE DOOR", run.Day, run.Quota);
        home.SetActive(true);
    }

    void OpenDoor()
    {
        if (door.sprite == doorOpen) return;

        door.sprite = doorOpen;
        sfx.PlayOneShot(doorSound);
        Invoke(nameof(StartCommute), config.doorOpenSeconds);
    }

    void StartCommute()
    {
        home.SetActive(false);
        run.Begin();
    }

    void ShowDayCard()
    {
        if (run.Cleared && run.IsLastDay)
        {
            result.Show();
            return;
        }

        if (run.Cleared) run.NextDay();
        dayNumber.Show(run.Day);
        dayCard.SetActive(true);
        Invoke(nameof(ShowHome), config.dayCardSeconds);
    }

    void ShowTitle()
    {
        run.ResetDays();
        title.SetActive(true);
    }
}
