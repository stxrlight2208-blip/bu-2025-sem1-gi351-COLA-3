using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 9f; // ความเร็วตอนกด Shift วิ่งไว
    [SerializeField] private float jumpForce = 8f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Interact Settings")]
    [SerializeField] private Transform interactCheck;
    [SerializeField] private float interactRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Impact / Fall Damage Settings")]
    [SerializeField] private float damageThreshold = 8f; // ความเร็วในการพุ่งชน/ตกกระแทกขั้นต่ำที่เริ่มเจ็บ
    [SerializeField] private float damageMultiplier = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isSprinting;
    private bool isCarryingOrb = false;
    private Orb carriedOrb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // 1. ตรวจจับการกดปุ่ม Shift เพื่อวิ่งไว
        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // 2. ระบบเคลื่อนที่พื้นฐาน (Walk / Sprint & Jump)
        HandleMovement();

        // 3. ตรวจสอบการกดปุ่ม Interact (E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void HandleMovement()
    {
        // ตรวจสอบว่าอยู่บนพื้นหรือไม่
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // คำนวณความเร็ว (กด Shift ใช้ sprintSpeed ไม่กดใช้ walkSpeed)
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // เดิน / วิ่ง
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        // กลับด้านตัวละคร (Flip Sprite)
        if (moveInput > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // กระโดด
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void TryInteract()
    {
        // กรณีที่ 1: กำลังอุ้ม Orb อยู่บนหัว -> กด E เพื่อวางลงพื้นทันที
        if (carriedOrb != null)
        {
            carriedOrb.OnInteract();
            carriedOrb = null;
            SetCarryingOrb(false);
            return;
        }

        // กรณีที่ 2: ไม่ได้ถือลูกแก้ว -> สแกนหาวัตถุรอบๆ เพื่อเก็บ
        Transform checkPoint = interactCheck != null ? interactCheck : transform;
        Collider2D[] hits = Physics2D.OverlapCircleAll(checkPoint.position, interactRadius, interactableLayer);

        foreach (var hit in hits)
        {
            Orb orb = hit.GetComponent<Orb>();
            if (orb != null)
            {
                carriedOrb = orb;
                orb.OnInteract();
                SetCarryingOrb(true);
                return;
            }
        }
    }

    public void SetCarryingOrb(bool state)
    {
        isCarryingOrb = state;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // คำนวณ Impact Damage เฉพาะตอนที่ "อุ้มลูกแก้วอยู่" เท่านั้น
        if (isCarryingOrb && carriedOrb != null)
        {
            // relativeVelocity คำนวณแรงกระแทกทั้งแนวตั้ง (ตกลงมาจากที่สูง) และแนวนอน (วิ่งพุ่งชนกำแพงแรงๆ)
            float impactVelocity = collision.relativeVelocity.magnitude;

            if (impactVelocity > damageThreshold)
            {
                float damage = (impactVelocity - damageThreshold) * damageMultiplier;

                // 1. หักเลือด Player
                TakeDamage(damage);

                // 2. หักเลือด/เวลาของ Orb โดยตรง (ทำให้ Health Bar ยุบทันที)
                carriedOrb.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Player Take Damage: " + amount + " | Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        // ถ้า Player ตาย ให้ปล่อยลูกแก้วหลุดออกจากมือ
        if (carriedOrb != null)
        {
            carriedOrb.OnInteract();
            carriedOrb = null;
            SetCarryingOrb(false);
        }
    }

    public float GetHealthPercent() => currentHealth / maxHealth;

    private void OnDrawGizmosSelected()
    {
        Transform checkPoint = interactCheck != null ? interactCheck : transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkPoint.position, interactRadius);

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}