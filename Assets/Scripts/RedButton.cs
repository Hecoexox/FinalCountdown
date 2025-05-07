using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour, IInteractable
{
    public GameObject Box;
    public Transform spawnPosition;
    public bool Boxmax = false;
    public GameObject Red;

    public void Interact(PlayerInteraction player)
    {
        if (!Boxmax)
        {
            Debug.Log("K�rm�z�");
            // Red olan objenin Ysini azalt
            Instantiate(Box, spawnPosition.position, spawnPosition.rotation);
            Boxmax = true;

            if (Red != null)
            {
                StartCoroutine(AnimateRedYMove());
            }

        }
        else
        {
            Debug.Log("Kutu zaten spawn edildi.");
        }
    }

    private IEnumerator AnimateRedYMove()
    {
        Vector3 start = Red.transform.localPosition;
        Vector3 down = new Vector3(start.x, start.y - 0.05f, start.z);
        Vector3 up = start;
        float duration = 0.25f; // A�a�� ini� s�resi
        float elapsed = 0f;

        // A�a�� in
        while (elapsed < duration)
        {
            Red.transform.localPosition = Vector3.Lerp(start, down, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Red.transform.localPosition = down;

        // Bekle
        yield return new WaitForSeconds(0.1f);

        // Yukar� ��k
        elapsed = 0f;
        while (elapsed < duration)
        {
            Red.transform.localPosition = Vector3.Lerp(down, up, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Red.transform.localPosition = up;
    }
}
