using UnityEngine;
using UnityEngine.UI;

public class OrbHP : MonoBehaviour
{
    [Header("Orb HP Settings")]
    public float maxHP = 100f;
    public float currentHP;
    public Slider hpBar; // ลาก Slider UI ของลูกแก้วมาใส่ (ถ้ามี)

    [Header("Fragile / Fall Damage Settings")]
    public float safeFallVelocity = -5f; // ความเร็วตกที่ไม่เจ็บ (ต่ำกว่านี้ปลอดภัย)
    public float fallDamageMultiplier = 8f; // ตัวคูณความเสียหายจากการตกแรง

    [Header("Respawn")]
    public Transform spawnPoint;

    private Rigidbody2D rb;
    [HideInInspector] public bool isHeld = false;
    private float lastYVelocity; // เก็บความเร็วแกน Y ก่อนกระทบพื้น

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        UpdateUI();
    }

    private void Update()
    {
        // เก็บค่าความเร็วตกถอยหลังไว้เช็กตอนกระทบพื้น
        if (!rb.isKinematic)
        {
            lastYVelocity = rb.linearVelocity.y; // สำหรับ Unity 6 (ถ้าเวอร์ชันเก่าใช้ rb.velocity.y)
        }
    }

    // ฟังก์ชันเรียกใช้อุ้มลูกแก้วลอยบนหัว
    public void PickUp(Transform headHoldPoint)
    {
        isHeld = true;
        rb.isKinematic = true;
        transform.SetParent(headHoldPoint);
        transform.localPosition = Vector3.zero; // ปรับให้ไปอยู่ตรงจุดลอยเหนือหัวพอดี
    }

    // ฟังก์ชันวางลูกแก้ว
    public void Drop()
    {
        isHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;
    }

    // ฟังก์ชันรับ Damage (ทั้งจากตกแรง และโดนมอนตี)
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        UpdateUI();

        if (currentHP <= 0)
        {
            BreakOrb();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // คำนวณความเสียหายจากการตกกระทบพื้น
        if (lastYVelocity < safeFallVelocity)
        {
            float impactForce = Mathf.Abs(lastYVelocity - safeFallVelocity);
            float damage = impactForce * fallDamageMultiplier;
            TakeDamage(damage);
        }

        // โดนมอนสเตอร์ชน/ตี
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20f); // มอนตีลดครั้งละ 20 HP
        }
    }

    public void BreakOrb()
    {
        // ลูกแก้วแตก -> รีเซ็ต HP แล้ววาร์ปกลับจุดเกิด
        currentHP = maxHP;
        UpdateUI();
        Drop();

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void UpdateUI()
    {
        if (hpBar != null)
        {
            hpBar.value = currentHP / maxHP;
        }
    }
}