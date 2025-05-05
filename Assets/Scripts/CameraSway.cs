using UnityEngine;

public class CameraSway : MonoBehaviour
{
    public float swayAmount = 0.1f;  // Ne kadar çok hareket etsin
    public float swaySpeed = 5f;     // Hareketin hızı
    public float returnSpeed = 3f;   // Başlangıç noktasına dönüş hızı
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;  // Başlangıç pozisyonunu al
    }

    private void Update()
    {
        // Mouse hareketine göre offset değerini al
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Hareket miktarını belirle (Yukarı-Aşağı ve Sol-Sağ)
        Vector3 swayOffset = new Vector3(-mouseX, -mouseY, 0f) * swayAmount;

        // Kameranın hedef pozisyonu
        Vector3 targetPosition = initialPosition + swayOffset;

        // Kamerayı yavaşça hedef pozisyona hareket ettir
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * swaySpeed);

        // Eğer mouse hareketi yoksa, kamera başlangıç noktasına dönecek
        if (mouseX == 0 && mouseY == 0)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * returnSpeed);
        }
    }
}