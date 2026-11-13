using UnityEngine;
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour
{
    const string Key = "MusicVolume";

    [SerializeField] Slider slider;
    [SerializeField] AudioSource music;

    void Awake()
    {
        slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(Key, music.volume));
        music.volume = slider.value;
        slider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float value)
    {
        music.volume = value;
        PlayerPrefs.SetFloat(Key, value);
    }
}
