using UnityEngine;
using UnityEngine.SceneManagement;

public class OrbHp : MonoBehaviour
{
    [Header("HP Settings")]
    public float maxHp = 100f;
    public float currentHp;

    private Rigidbody2D rb;

    private void Start()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();

        // ป้องกัน Warning เรื่อง isKinematic ใน Unity 6
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    // ฟังก์ชันคำนวณ % เลือดสำหรับนำไปใช้กับ Slider
    public float GetHealthPercent()
    {
        return currentHp / maxHp;
    }

    // ฟังก์ชันโดนความเสียหาย
    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);

        Debug.Log($"Player Take Damage: {amount} | Current HP: {currentHp}");

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // เมื่อเลือดหมด ให้รีโหลดฉากใหม่
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}