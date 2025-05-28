using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class DaySystem : MonoBehaviour
{
    public static DaySystem Instance;

    [Header("UI")]
    public TextMeshProUGUI dayText;

    [Header("Crosses")]
    public GameObject[] crossObjects; // 3D çarpı objeleri (3 adet)
    public Material crossRed;
    public Material crossWhite;
    public AudioSource buzzerAudio; // çarpı yandığında çalacak ses
    public Image blackFadeImage; // Inspector'dan atanacak

    [Header("Day Settings")]
    public float dayDuration = 180f; // 3 dakika
    private int currentDay = 1;
    private float dayTimer;
    private bool dayTextVisible = false;

    [Header("Customer Timer")]
    public float customerTime = 30f;
    private float customerTimer;
    private bool customerActive = false;

    private int currentCross = 0;

    [Header("Customer Timer UI")]
    public Image timerCircle;
    public TextMeshProUGUI timerText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dayTimer = dayDuration;
        ShowDayText();
        ResetCrosses();
        StartCustomerTimer();
    }

    void Update()
    {
        // Gün sayacı
        if (!dayTextVisible)
        {
            dayTimer -= Time.deltaTime;
            if (dayTimer <= 0f)
            {
                NextDay();
            }
        }

        // Müşteri zamanlayıcı
        if (customerActive)
        {
            customerTimer -= Time.deltaTime;
            if (timerCircle != null)
                timerCircle.fillAmount = Mathf.Clamp01(customerTimer / customerTime);
            if (timerText != null)
                timerText.text = Mathf.CeilToInt(customerTimer).ToString();

            if (customerTimer <= 0f)
            {
                customerActive = false;
                OnCustomerTimeout();
            }
        }
    }

    void ShowDayText()
    {
        dayTextVisible = true;
        dayText.text = $"Day {currentDay}";
        dayText.gameObject.SetActive(true);
        StartCoroutine(DayTextFadeInOut());
    }

    IEnumerator DayTextFadeInOut()
    {
        // Orijinal pozisyonu kaydet
        RectTransform rect = dayText.GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;
        Vector2 startPos = originalPos + new Vector2(0, 200f); // yukarıdan başlasın
        Vector2 endPos = originalPos + new Vector2(0, 200f);   // yukarıya çıksın
        float duration = 1.0f;
        float elapsed = 0f;
        Color c = dayText.color;
        c.a = 0f;
        dayText.color = c;
        rect.anchoredPosition = startPos;

        // Fade In: Yukarıdan orijinal pozisyona inerken alpha 0->1
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rect.anchoredPosition = Vector2.Lerp(startPos, originalPos, t);
            c.a = Mathf.Lerp(0f, 1f, t);
            dayText.color = c;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        rect.anchoredPosition = originalPos;
        c.a = 1f;
        dayText.color = c;

        // Bekleme süresi
        yield return new WaitForSeconds(1.3f);

        // Fade Out: Orijinalden yukarıya çıkarken alpha 1->0
        duration = 0.7f;
        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rect.anchoredPosition = Vector2.Lerp(originalPos, endPos, t);
            c.a = Mathf.Lerp(1f, 0f, t);
            dayText.color = c;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        rect.anchoredPosition = originalPos;
        c.a = 0f;
        dayText.color = c;
        dayText.gameObject.SetActive(false);
        dayTextVisible = false;
    }

    void NextDay()
    {
        currentDay++;
        dayTimer = dayDuration;
        ShowDayText();
    }

    public void AddCross()
    {
        if (currentCross < crossObjects.Length)
        {
            SetMaterial(crossObjects[currentCross], crossRed);
            if (buzzerAudio != null)
                buzzerAudio.Play();
            currentCross++;
            if (currentCross >= crossObjects.Length)
            {
                // Oyun bitti, fade to black ve menüye dön
                StartCoroutine(GameOverSequence());
            }
        }
    }

    void ResetCrosses()
    {
        foreach (var obj in crossObjects)
        {
            SetMaterial(obj, crossWhite);
        }
        currentCross = 0;
    }

    void SetMaterial(GameObject obj, Material mat)
    {
        var rend = obj.GetComponent<Renderer>();
        if (rend != null)
            rend.material = mat;
    }

    // Müşteri zamanlayıcıyı başlat
    public void StartCustomerTimer()
    {
        customerTimer = customerTime;
        customerActive = true;
    }

    // Müşteri zamanlayıcıyı sıfırla (doğru sipariş verildiğinde çağrılacak)
    public void ResetCustomerTimer()
    {
        customerTimer = customerTime;
        customerActive = true;
    }

    // Müşteri süresi dolduğunda
    void OnCustomerTimeout()
    {
        AddCross();
        // Yeni müşteri oluştur
        if (Customer.Instance != null)
            Customer.Instance.CreateNewOrder();
        StartCustomerTimer();
    }

    // Yanlış siparişten de çağrılacak
    public void OnWrongOrder()
    {
        AddCross();
        // Yeni müşteri oluştur
        if (Customer.Instance != null)
            Customer.Instance.CreateNewOrder();
        StartCustomerTimer();
    }

    // Doğru siparişten de çağrılacak
    public void OnCorrectOrder()
    {
        ResetCustomerTimer();
    }

    IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(2f); // Birkaç saniye bekle
        if (blackFadeImage != null)
        {
            blackFadeImage.gameObject.SetActive(true);
            Color c = blackFadeImage.color;
            c.a = 0f;
            blackFadeImage.color = c;
            yield return StartCoroutine(FadeToBlack(blackFadeImage, 2f));
        }
        SceneManager.LoadScene("Main_Menu");
    }

    IEnumerator FadeToBlack(Image img, float duration)
    {
        float elapsed = 0f;
        Color c = img.color;
        c.a = 0f;
        img.color = c;
        while (elapsed < duration)
        {
            c.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            img.color = c;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        c.a = 1f;
        img.color = c;
    }
}
