using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenButton : MonoBehaviour, IInteractable
{
    public MoveBox moveBox;
    public RedButton redButton;
    public BoxAnim boxAnim;

    public GameObject Green;

    void Update()
    {
        if (moveBox == null)
        {
            moveBox = FindObjectOfType<MoveBox>();
            if (moveBox == null) return; 
        }

        if (boxAnim == null)
        {
            boxAnim = FindObjectOfType<BoxAnim>();
            if (boxAnim == null) return;
        }
    }

    public void Interact(PlayerInteraction player)
    {
        Debug.Log("Yeþil");
        if (!moveBox.isMoving & Green != null & !moveBox.readyToGo)
        {
            StartCoroutine(ActivateBoxSequence());
            StartCoroutine(AnimateGreenYMove());
        }
    }

    private IEnumerator ActivateBoxSequence()
    {
        boxAnim.Animate = true;
        moveBox.readyToGo = true;

        yield return new WaitForSeconds(1f); // 2 saniye bekle
        
        redButton.Boxmax = false;
    }

    private IEnumerator AnimateGreenYMove()
    {
        Vector3 start = Green.transform.localPosition;
        Vector3 down = new Vector3(start.x, start.y - 0.05f, start.z);
        Vector3 up = start;
        float duration = 0.25f; // Aþaðý iniþ süresi
        float elapsed = 0f;

        // Aþaðý in
        while (elapsed < duration)
        {
            Green.transform.localPosition = Vector3.Lerp(start, down, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Green.transform.localPosition = down;

        // Bekle
        yield return new WaitForSeconds(0.1f);

        // Yukarý çýk
        elapsed = 0f;
        while (elapsed < duration)
        {
            Green.transform.localPosition = Vector3.Lerp(down, up, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Green.transform.localPosition = up;
    }
}