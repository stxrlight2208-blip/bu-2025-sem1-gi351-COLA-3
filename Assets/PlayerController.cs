using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    // Components
    private Rigidbody2D rb;
    private Animator anim;

    // States
    private float horizontalInput;
    private bool isGrounded;
    private bool facingRight = true;

    private void Awake()
    {
        // ดึง Component Rigidbody2D และ Animator มาใช้งาน
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 1. รับค่าการกดปุ่ม เดินซ้าย-ขวา (A/D หรือ ปุ่มลูกศร)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. เช็คว่าตัวละครยืนอยู่บนพื้นหรือไม่
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 3. ระบบกระโดด (กด Spacebar)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 4. กลับด้านสไปรท์ (Flip) ตามทิศทางการเดิน
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }

        // 5. ส่งค่าไปยัง Animator เพื่อเล่น Animation
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        // คำนวณการเคลื่อนที่เชิงฟิสิกส์
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void UpdateAnimation()
    {
        if (anim != null)
        {
            // ตั้งค่า Parameter ใน Animator Controller
            // - isRunning: true เมื่อมีการเคลื่อนที่ซ้าย-ขวา
            // - isGrounded: ส่งค่าสถานะการอยู่บนพื้น
            // - yVelocity: ส่งค่าความเร็วแกน Y เพื่อใช้เช็คการกระโดด/ร่วงหล่น
            anim.SetBool("isRunning", Mathf.Abs(horizontalInput) > 0);
            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
        }
    }

    // แสดงวงกลมตรวจจับพื้นในหน้า Scene (ช่วยในการตั้งค่า)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}