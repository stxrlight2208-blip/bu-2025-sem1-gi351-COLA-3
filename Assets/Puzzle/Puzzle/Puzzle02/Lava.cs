using UnityEngine;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 100f;
    [SerializeField] private bool isInstantKill = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. เช็กกรณี Orb ชน
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

        // 2. เช็กกรณี Player ชน
        if (other.CompareTag("Player"))
        {
            if (Checkpoint.hasCheckpoint)
            {
                // ย้ายตำแหน่ง Player ไปยัง Checkpoint ล่าสุด
                other.transform.position = Checkpoint.lastCheckpointPosition;

                // รีเซ็ตความเร็วของ Rigidbody2D ไม่ให้ตัวละครพุ่งต่อตอน Respawn
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
            else
            {
                // ถ้ายังไม่เคยผ่าน Checkpoint ให้ Reload Scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }
}