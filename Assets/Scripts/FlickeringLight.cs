using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light spotLight; // Fenerin ucundaki Spot Light
    public float minFlickerTime = 0.05f;
    public float maxFlickerTime = 0.3f;

    private void Start()
    {
        if (spotLight == null)
            spotLight = GetComponent<Light>();

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            spotLight.enabled = !spotLight.enabled; // Işığı aç/kapat
            float waitTime = Random.Range(minFlickerTime, maxFlickerTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}