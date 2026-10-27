using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private Slider healthSlider;

    [Header("Settings")]
    [SerializeField] private float smoothSpeed = 10f;

    private Orb orb;

    private void Start()
    {
        // ดึงสคริปต์ Orb จาก GameObject แม่ (ORb) อัตโนมัติ
        orb = GetComponentInParent<Orb>();

        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
        }

        // ตั้งค่าสูงสุดของ Slider เป็น 1 (เพราะ GetHealthPercent รีเทิร์นค่า 0.0 ถึง 1.0)
        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = 1f;
        }
    }

    private void Update()
    {
        if (orb != null && healthSlider != null)
        {
            // ดึงค่า % เลือด (0-1) มาปรับค่า Slider แบบนุ่มนวล
            float targetValue = orb.GetHealthPercent();
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetValue, smoothSpeed * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        // ล็อกไม่ให้หลอดเลือดหมุนตามตัว Orb ใน World Space
        transform.rotation = Quaternion.identity;
    }
}