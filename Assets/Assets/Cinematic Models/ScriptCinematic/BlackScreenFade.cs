using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlackScreenFade : MonoBehaviour
{
    public Image blackScreen;
    public float fadeDuration = 2f;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Color color = blackScreen.color;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / fadeDuration);
            color.a = Mathf.Lerp(1, 0, normalizedTime);
            blackScreen.color = color;
            yield return null;
        }

        blackScreen.gameObject.SetActive(false);
    }
}
