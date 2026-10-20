using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private Orb orb;

    private void Start()
    {
        orb = GetComponentInParent<Orb>();

        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
        }
    }

    private void Update()
    {
        if (orb != null && healthSlider != null)
        {
            healthSlider.value = orb.GetHealthPercent();
        }
    }

    private void LateUpdate()
    {
        // บังคับให้ HealthBar ตั้งตรง 0 องศาใน World Space เสมอ
        transform.rotation = Quaternion.identity;
    }
}