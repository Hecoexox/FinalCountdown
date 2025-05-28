using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;
    public GameObject settingsPanel;
    public Button resumeButton;
    public Button settingsButton;
    public Button quitButton;
    public Slider musicSlider;
    public Slider soundSlider;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource soundSource;
    public AudioClip clickSound;

    [Header("Camera Control")]
    public MonoBehaviour cameraController; // Inspector'dan atanacak

    [Header("Screen Fade")]
    public Image blackFadeImage; // Inspector'dan atanacak

    private bool isPaused = false;
    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";

    void Start()
    {
        // Music
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        if (musicSource != null) musicSource.volume = savedMusicVolume;
        if (musicSlider != null)
        {
            musicSlider.value = savedMusicVolume;
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        // Sound
        float savedSoundVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 0.5f);
        if (soundSource != null) soundSource.volume = savedSoundVolume;
        if (soundSlider != null)
        {
            soundSlider.value = savedSoundVolume;
            soundSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
        }

        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (quitButton != null) quitButton.onClick.AddListener(QuitToMenu);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

        if (blackFadeImage != null)
        {
            Color c = blackFadeImage.color;
            c.a = 1f;
            blackFadeImage.color = c;
            StartCoroutine(FadeFromBlack(blackFadeImage, 4.0f));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                Pause();
        }
    }

    void PlayClickSound()
    {
        if (clickSound != null && soundSource != null)
            soundSource.PlayOneShot(clickSound);
    }

    public void Pause()
    {
        PlayClickSound();
        Time.timeScale = 0f;
        isPaused = true;
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        if (cameraController != null) cameraController.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        isPaused = false;
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (cameraController != null) cameraController.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenSettings()
    {
        PlayClickSound();
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            CanvasGroup cg = settingsPanel.GetComponent<CanvasGroup>();
            if (cg != null)
                StartCoroutine(FadeIn(cg, 0.3f));
        }
    }

    public void CloseSettings()
    {
        PlayClickSound();
        if (settingsPanel != null)
        {
            CanvasGroup cg = settingsPanel.GetComponent<CanvasGroup>();
            if (cg != null)
                StartCoroutine(FadeOut(cg, 0.3f));
            else
                settingsPanel.SetActive(false);
        }
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup, float duration)
    {
        float elapsed = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }

    IEnumerator FadeFromBlack(Image img, float duration)
    {
        float elapsed = 0f;
        Color c = img.color;
        c.a = 1f;
        img.color = c;
        while (elapsed < duration)
        {
            c.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            img.color = c;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        c.a = 0f;
        img.color = c;
        img.gameObject.SetActive(false);
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (musicSource != null) musicSource.volume = value;
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void OnSoundVolumeChanged(float value)
    {
        if (soundSource != null) soundSource.volume = value;
        PlayerPrefs.SetFloat(SoundVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void QuitToMenu()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_Menu"); // Ana menü sahne adını kendi projenize göre değiştirin
    }
}
