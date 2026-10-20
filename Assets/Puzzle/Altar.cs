using UnityEngine;
using UnityEngine.UI;

public class Altar : MonoBehaviour
{
    [Header("Charge Settings")]
    public float chargeTime = 10f; // เวลาชาร์จ (วินาที)
    private float currentCharge = 0f;

    [Header("UI & State")]
    public Slider progressBar; // ลาก UI Slider มาใส่
    public bool isFullyCharged = false;
    private bool isCharging = false;

    private void Update()
    {
        if (isCharging && !isFullyCharged)
        {
            currentCharge += Time.deltaTime;
            if (progressBar != null) progressBar.value = currentCharge / chargeTime;

            if (currentCharge >= chargeTime)
            {
                isFullyCharged = true;
                isCharging = false;
                GameManager.Instance.OnAltarCharged(); // แจ้งตัวจัดการเกมว่าชาร์จเสร็จแล้ว
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // เมื่อนำลูกแก้วมาวางบนแท่น
        if (other.CompareTag("Orb") && !isFullyCharged)
        {
            isCharging = true;
            other.transform.SetParent(transform); // ล็อคลูกแก้วไว้กับแท่น
            other.transform.localPosition = new Vector3(0, 1f, 0); // วางไว้เหนือแท่น
        }
    }
}