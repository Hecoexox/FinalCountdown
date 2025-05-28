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
    public TextMeshProUGUI dayTransition;

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

    private bool isFirstCustomer = true;
    private bool isDayTransition = false;

    [Header("Day Timer UI")]
    public Image dayTimerCircle;

    [Header("Transition Hide Objects")]
    public GameObject[] objectsToHideDuringTransition;

    [Header("Game Over Stats UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI statsDayText;
    public TextMeshProUGUI statsCustomerText;
    public TextMeshProUGUI statsPressSpaceText;

    private int successfulCustomerCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dayTimer = dayDuration;
        ShowDayText();
        ResetCrosses();
        isFirstCustomer = true;
        if (timerCircle != null) timerCircle.gameObject.SetActive(false);
        if (timerText != null) timerText.gameObject.SetActive(false);
        // İlk müşteri için timer başlatma
    }

    void Update()
    {
        // Gün sayacı
        if (!dayTextVisible && !isDayTransition && !isFirstCustomer)
        {
            dayTimer -= Time.deltaTime;
            if (dayTimer <= 0f)
            {
                NextDay();
            }
        }
        if (dayTimerCircle != null)
            dayTimerCircle.fillAmount = Mathf.Clamp01(dayTimer / dayDuration);

        // Müşteri zamanlayıcı (ilk müşteri hariç)
        if (customerActive && !isFirstCustomer)
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
        float duration = 1.0f;
        float elapsed = 0f;
        Color c = dayText.color;

        // Fade Out eski gün
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            c.a = Mathf.Lerp(1f, 0f, t);
            dayText.color = c;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // Yeni gün textini ayarla
        dayText.text = $"Day {currentDay}";
        elapsed = 0f;

        // Fade In yeni gün
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            c.a = Mathf.Lerp(0f, 1f, t);
            dayText.color = c;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        c.a = 1f;
        dayText.color = c;
        dayTextVisible = false;
    }

    void NextDay()
    {
        StartCoroutine(DayTransitionSequence());
    }

    IEnumerator DayTransitionSequence()
    {
        isDayTransition = true;
        // Geçişte objeleri kapat
        if (objectsToHideDuringTransition != null)
        {
            foreach (var obj in objectsToHideDuringTransition)
                if (obj != null) obj.SetActive(false);
        }
        // Fade to black
        if (blackFadeImage != null)
        {
            blackFadeImage.gameObject.SetActive(true);
            Color c = blackFadeImage.color;
            c.a = 0f;
            blackFadeImage.color = c;
            yield return StartCoroutine(FadeToBlack(blackFadeImage, 1.2f));
        }
        // 3 saniye tam siyah bekle ve dayTransition animasyonu başlat
        if (dayTransition != null)
        {
            dayTransition.text = $"Day {currentDay + 1}";
            yield return StartCoroutine(DayTransitionMoveAnim(dayTransition, 3f));
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }

        // Yeni gün başlat
        currentDay++;
        dayTimer = dayDuration;
        ShowDayText();

        // Fade from black
        if (blackFadeImage != null)
        {
            yield return StartCoroutine(FadeFromBlack(blackFadeImage, 1.2f));
            blackFadeImage.gameObject.SetActive(false);
        }
        // Geçiş bittiğinde objeleri tekrar aç
        if (objectsToHideDuringTransition != null)
        {
            foreach (var obj in objectsToHideDuringTransition)
                if (obj != null) obj.SetActive(true);
        }
        isDayTransition = false;
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
    }

    IEnumerator DayTransitionMoveAnim(TextMeshProUGUI text, float totalDuration)
    {
        text.gameObject.SetActive(true);
        RectTransform rect = text.GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;
        Vector2 downPos = originalPos + new Vector2(0, -500f);
        Vector2 upPos = originalPos + new Vector2(0, 500f);
        float moveDuration = totalDuration * 0.4f;
        float waitDuration = totalDuration * 0.2f;
        float elapsed = 0f;
        Color c = text.color;
        c.a = 0f;
        text.color = c;
        // Aşağıya inme + fade in
        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            rect.anchoredPosition = Vector2.Lerp(originalPos, downPos, t);
            c.a = Mathf.Lerp(0f, 1f, t);
            text.color = c;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        rect.anchoredPosition = downPos;
        c.a = 1f;
        text.color = c;
        yield return new WaitForSeconds(waitDuration);
        // Yukarı çıkma + fade out
        elapsed = 0f;
        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            rect.anchoredPosition = Vector2.Lerp(downPos, upPos, t);
            c.a = Mathf.Lerp(1f, 0f, t);
            text.color = c;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        rect.anchoredPosition = originalPos;
        c.a = 0f;
        text.color = c;
        text.gameObject.SetActive(false);
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
                // Oyun bitti, fade to black ve menüye dönmeden önce stats göster
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
        if (timerCircle != null) timerCircle.gameObject.SetActive(true);
        if (timerText != null) timerText.gameObject.SetActive(true);
    }

    // Müşteri zamanlayıcıyı sıfırla (doğru sipariş verildiğinde çağrılacak)
    public void ResetCustomerTimer()
    {
        if (isFirstCustomer)
        {
            // İlk müşteri tamamlandı, timer başlasın
            isFirstCustomer = false;
            StartCustomerTimer();
        }
        else
        {
            customerTimer = customerTime;
            customerActive = true;
        }
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
        if (isFirstCustomer)
        {
            isFirstCustomer = false;
            StartCustomerTimer();
        }
        AddCross();
        // Yeni müşteri oluştur
        if (Customer.Instance != null)
            Customer.Instance.CreateNewOrder();
        StartCustomerTimer();
    }

    // Doğru siparişten de çağrılacak
    public void OnCorrectOrder()
    {
        if (isFirstCustomer)
        {
            isFirstCustomer = false;
            StartCustomerTimer();
        }
        successfulCustomerCount++;
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
        // Stats ekranı göster
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (statsDayText != null)
                statsDayText.text = $"Days Survived: {currentDay}";
            if (statsCustomerText != null)
                statsCustomerText.text = $"Customers Served: {successfulCustomerCount}";
            if (statsPressSpaceText != null)
                statsPressSpaceText.text = "Press SPACE to go back to menu";
        }
        // Space tuşunu bekle
        bool waiting = true;
        while (waiting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                waiting = false;
            }
            yield return null;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Main_Menu");
    }
}
