using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject title;
    public GameObject continueButton;
    public GameObject newGameButton;
    public GameObject settingsButton;
    public GameObject creditsButton;
    public GameObject quitButton;

    public GameObject settingsPanel;
    public GameObject creditsPanel;

    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void NewGame()
    {
        Debug.Log("New Game");
        PlayClickSound();
        //SceneManager.LoadScene("GameScene"); // sahne adýný kendi sahnene göre deðiþtir
    }

    public void ContinueGame()
    {
        Debug.Log("Continue Game");
        PlayClickSound();
        //SceneManager.LoadScene(savedSceneIndex);
    }

    public void OpenSettings()
    {
        PlayClickSound();
        settingsPanel.SetActive(true);
        StartCoroutine(FadeIn(settingsPanel.GetComponent<CanvasGroup>(), 0.3f));
        SlideUIElementsDown();
    }

    public void CloseSettings()
    {
        PlayClickSound();
        StartCoroutine(FadeOut(settingsPanel.GetComponent<CanvasGroup>(), 0.3f));
        SlideUIElementsUp();
    }

    public void OpenCredits()
    {
        PlayClickSound();
        creditsPanel.SetActive(true);
        StartCoroutine(FadeIn(creditsPanel.GetComponent<CanvasGroup>(), 0.3f));
        SlideUIElementsDown();
    }

    public void CloseCredits()
    {
        PlayClickSound();
        StartCoroutine(FadeOut(creditsPanel.GetComponent<CanvasGroup>(), 0.3f));
        SlideUIElementsUp();
    }

    public void SlideUIElementsDown()
    {
        StartCoroutine(SlideObjectsDownSmoothly());
    }

    IEnumerator SlideObjectsDownSmoothly()
    {
        float duration = 0.5f; // hýzlý ama smooth
        float distance = -500f; // ne kadar aþaðý kayacak
        float elapsed = 0f;

        // Baþlangýç pozisyonlarýný kaydet
        Vector3 titleStart = title.transform.localPosition;
        Vector3 continueStart = continueButton.transform.localPosition;
        Vector3 newGameStart = newGameButton.transform.localPosition;
        Vector3 settingsStart = settingsButton.transform.localPosition;
        Vector3 creditsStart = creditsButton.transform.localPosition;
        Vector3 quitStart = quitButton.transform.localPosition;

        // Hedef pozisyonlar
        Vector3 offset = new Vector3(0, distance, 0);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0, 1, t); // yumuþak geçiþ

            title.transform.localPosition = Vector3.Lerp(titleStart, titleStart + offset, t);
            continueButton.transform.localPosition = Vector3.Lerp(continueStart, continueStart + offset, t);
            newGameButton.transform.localPosition = Vector3.Lerp(newGameStart, newGameStart + offset, t);
            settingsButton.transform.localPosition = Vector3.Lerp(settingsStart, settingsStart + offset, t);
            creditsButton.transform.localPosition = Vector3.Lerp(creditsStart, creditsStart + offset, t);
            quitButton.transform.localPosition = Vector3.Lerp(quitStart, quitStart + offset, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Pozisyonlarý kesin olarak hedefe oturt
        title.transform.localPosition = titleStart + offset;
        continueButton.transform.localPosition = continueStart + offset;
        newGameButton.transform.localPosition = newGameStart + offset;
        settingsButton.transform.localPosition = settingsStart + offset;
        creditsButton.transform.localPosition = creditsStart + offset;
        quitButton.transform.localPosition = quitStart + offset;
    }

    public void SlideUIElementsUp()
    {
        StartCoroutine(SlideObjectsUpSmoothly());
    }

    IEnumerator SlideObjectsUpSmoothly()
    {
        float duration = 0.5f; // hýzlý ama smooth
        float distance = +500f; // ne kadar aþaðý kayacak
        float elapsed = 0f;

        // Baþlangýç pozisyonlarýný kaydet
        Vector3 titleStart = title.transform.localPosition;
        Vector3 continueStart = continueButton.transform.localPosition;
        Vector3 newGameStart = newGameButton.transform.localPosition;
        Vector3 settingsStart = settingsButton.transform.localPosition;
        Vector3 creditsStart = creditsButton.transform.localPosition;
        Vector3 quitStart = quitButton.transform.localPosition;

        // Hedef pozisyonlar
        Vector3 offset = new Vector3(0, distance, 0);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0, 1, t); // yumuþak geçiþ

            title.transform.localPosition = Vector3.Lerp(titleStart, titleStart + offset, t);
            continueButton.transform.localPosition = Vector3.Lerp(continueStart, continueStart + offset, t);
            newGameButton.transform.localPosition = Vector3.Lerp(newGameStart, newGameStart + offset, t);
            settingsButton.transform.localPosition = Vector3.Lerp(settingsStart, settingsStart + offset, t);
            creditsButton.transform.localPosition = Vector3.Lerp(creditsStart, creditsStart + offset, t);
            quitButton.transform.localPosition = Vector3.Lerp(quitStart, quitStart + offset, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Pozisyonlarý kesin olarak hedefe oturt
        title.transform.localPosition = titleStart + offset;
        continueButton.transform.localPosition = continueStart + offset;
        newGameButton.transform.localPosition = newGameStart + offset;
        settingsButton.transform.localPosition = settingsStart + offset;
        creditsButton.transform.localPosition = creditsStart + offset;
        quitButton.transform.localPosition = quitStart + offset;
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
            elapsed += Time.deltaTime;
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
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        PlayClickSound();
        Application.Quit();
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // Editörde çalýþmayý durdurur
#else
        Application.Quit(); // Build'de oyunu kapatýr
#endif
    }
}
