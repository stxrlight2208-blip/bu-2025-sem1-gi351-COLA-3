using UnityEngine;
using UnityEngine.SceneManagement; // ใช้สำหรับการรีโหลดฉาก (Respawn Player)

public class Lava : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 100f;
    [SerializeField] private bool isInstantKill = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. เช็กถ้าเป็น Orb ให้ Respawn กลับจุดเริ่มต้น
        Orb orb = other.GetComponent<Orb>();
        if (orb != null)
        {
            if (isInstantKill)
            {
                orb.RespawnOrb();
            }
            else
            {
                orb.TakeDamage(damageAmount);
            }
            return;
        }

        // 2. เช็กถ้าเป็น Player ให้รีโหลดฉากใหม่ทันที (Instant Respawn)
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // รองรับกรณีที่ Collider2D ไม่ได้ติ๊ก Is Trigger
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }
}