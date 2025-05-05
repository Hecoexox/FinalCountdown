using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GrainPulse : MonoBehaviour
{
    private PostProcessVolume volume;
    private Grain grain;

    [Header("Intensity Ayarları")]
    [Range(0f, 1f)] public float minIntensity = 0.1f;
    [Range(0f, 1f)] public float maxIntensity = 0.4f;

    [Header("Size Ayarları")]
    [Range(0.3f, 3f)] public float minSize = 1f;
    [Range(0.3f, 3f)] public float maxSize = 2f;

    public float speed = 1f;

    void Start()
    {
        volume = GetComponent<PostProcessVolume>();

        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGetSettings(out grain);
        }

        if (grain == null)
        {
            Debug.LogWarning("Grain bulunamadı! Volume profilinde Grain efekti olduğundan emin ol.");
        }
    }

    void Update()
    {
        if (grain != null)
        {
            float t = Mathf.PingPong(Time.time * speed, 1f);

            grain.intensity.value = Mathf.Lerp(minIntensity, maxIntensity, t);
            grain.size.value = Mathf.Lerp(minSize, maxSize, t);
        }
    }
}