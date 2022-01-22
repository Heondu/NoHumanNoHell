using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    private GameManager gameManager;
    private bool isOpen = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();
        bgmSlider.value = SoundManager.Instance.GetBGMVolume();
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpen)
                Open();
            else
                Close();
        }
    }

    private void UpdateSFXVolume(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

    private void UpdateBGMVolume(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    public void Quit()
    {
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Open()
    {
        isOpen = true;
        gameManager.Stop(true);
        optionPanel.SetActive(true);
        pausePanel.SetActive(true);
        settingPanel.SetActive(false);
    }

    public void Close()
    {
        isOpen = false;
        gameManager.Stop(false);
        optionPanel.SetActive(false);
        pausePanel.SetActive(true);
        settingPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    public void Restart()
    {
        Close();
        GameManager.Instance.Restart();
    }
}
