using UnityEngine;

public class Orb : MonoBehaviour
{
    [Header("Orb Health & Timer Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float decayDuration = 20f; // เวลา 20 วินาทีก่อน Orb จะดับแล้ว Respawn
    private float currentHealth;

    private bool isCarried = false;
    private bool hasBeenPickedUpOnce = false; // ตรวจจับว่าถูกอุ้มครั้งแรกแล้วหรือยัง
    private Transform carrier;
    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 initialPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // ตั้งค่าเริ่มต้นให้ลอยค้างกลางอากาศ (Kinematic) และทะลุผ่านได้ (IsTrigger)
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.isTrigger = true;

        currentHealth = maxHealth;
        initialPosition = transform.position;
    }

    private void Update()
    {
        // 1. ถ้าถูกถืออยู่ ให้ตำแหน่งลอยนิ่งอยู่เหนือหัว Player
        if (isCarried && carrier != null)
        {
            transform.position = carrier.position + new Vector3(0f, 1.2f, 0f);
            transform.rotation = Quaternion.identity; // ล็อคองศานิ่งตรง
        }
        // 2. ถ้าไม่ได้ถือ ให้ลอยนิ่งๆ อยู่ ณ ตำแหน่งปัจจุบัน
        else if (!isCarried)
        {
            transform.rotation = Quaternion.identity; // ล็อคองศานิ่งตรง
        }

        // 3. ระบบนับถอยหลัง 20 วินาที (เริ่มนับหลังจากถูกหยิบครั้งแรกแล้ววางลง)
        if (hasBeenPickedUpOnce && !isCarried)
        {
            float decayRate = maxHealth / decayDuration;
            TakeDamage(decayRate * Time.deltaTime);
        }
    }

    public void OnInteract()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player == null) return;

        if (!isCarried)
        {
            // === 1. ยกขึ้นหัว (Pick Up) ===
            isCarried = true;
            hasBeenPickedUpOnce = true;
            carrier = player.transform;

            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            col.enabled = false;
        }
        else
        {
            // === 2. วางลงพื้น/อากาศ (Drop) ===
            isCarried = false;
            carrier = null;

            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            col.enabled = true;
            col.isTrigger = true;

            // วางลงข้างๆ ตัวละครตามทิศทางที่ Player หันไป
            float offsetDir = player.transform.localScale.x > 0 ? 0.8f : -0.8f;
            transform.position = player.transform.position + new Vector3(offsetDir, 0.2f, 0f);
            transform.rotation = Quaternion.identity;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            RespawnOrb();
        }
    }

    public void RespawnOrb()
    {
        currentHealth = maxHealth;
        isCarried = false;
        hasBeenPickedUpOnce = false;
        carrier = null;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        col.enabled = true;
        col.isTrigger = true;

        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
    }

    public float GetHealthPercent()
    {
        return Mathf.Clamp01(currentHealth / maxHealth);
    }

    public bool IsCarried() => isCarried;
}