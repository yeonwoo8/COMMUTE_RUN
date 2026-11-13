using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] Button openButton;
    [SerializeField] Button closeButton;
    [SerializeField] GameObject panel;

    void Awake()
    {
        openButton.onClick.AddListener(() => panel.SetActive(true));
        closeButton.onClick.AddListener(() => panel.SetActive(false));
    }
}
