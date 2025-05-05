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

    public void NewGame()
    {
        Debug.Log("New Game");
        //SceneManager.LoadScene("GameScene"); // sahne adýný kendi sahnene göre deðiþtir
    }

    public void ContinueGame()
    {
        Debug.Log("Continue Game");
        //SceneManager.LoadScene(savedSceneIndex);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        SlideUIElementsDown();
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        SlideUIElementsUp();
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        SlideUIElementsDown();
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
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

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
