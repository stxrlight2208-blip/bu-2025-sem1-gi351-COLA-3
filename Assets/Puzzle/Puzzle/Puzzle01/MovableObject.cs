using System.Collections;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactRadius = 2f;

    [Header("Movement Settings")]
    [SerializeField] private Transform targetPosition; // จุดปลายทางที่จะให้วัตถุขยับไป
    [SerializeField] private float moveSpeed = 3f;      // ความเร็วในการเลื่อน
    [SerializeField] private bool autoReturn = false;   // ติ๊กถูกถ้าต้องการให้ขยับกลับจุดเดิมเอง
    [SerializeField] private float returnDelay = 5f;    // เวลาที่รอก่อนจะเลื่อนกลับ (ถ้า autoReturn = true)

    private Vector3 startPosition;
    private Transform player;
    private bool isMoving = false;
    private bool isAtTarget = false;

    private void Start()
    {
        startPosition = transform.position;

        // ค้นหา Player อัตโนมัติจาก Tag "Player"
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
    }

    private void Update()
    {
        if (player == null || isMoving) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // เช็กว่าผู้เล่นอยู่ในระยะและกดปุ่ม E หรือไม่
        if (distance <= interactRadius && Input.GetKeyDown(interactKey))
        {
            StartCoroutine(MoveRoutine());
        }
    }

    private IEnumerator MoveRoutine()
    {
        isMoving = true;

        // กำหนดจุดหมายปลายทาง (ถ้ารวมถึงตอนขยับกลับจุดเริ่มต้น)
        Vector3 destination = isAtTarget ? startPosition : targetPosition.position;

        // เคลื่อนที่ไปยังจุดหมายอย่างนุ่มนวล
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = destination; // ล็อกตำแหน่งปลายทางให้เป๊ะ
        isAtTarget = !isAtTarget;
        isMoving = false;

        // ระบบเลื่อนกลับอัตโนมัติหลังหมดเวลา (ถ้าติ๊กเปิดไว้)
        if (autoReturn && isAtTarget)
        {
            yield return new WaitForSeconds(returnDelay);
            StartCoroutine(MoveRoutine());
        }
    }

    // ยึด Player ติดกับวัตถุเวลาลอย/ขยับ เพื่อไม่ให้ตัวละครสไลด์ตกจากลิฟต์
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // แสดงวงกลมระยะกด E ในหน้า Scene
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadius);

        // แสดงเส้นเชื่อมไปยังจุดปลายทางในหน้า Scene
        if (targetPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetPosition.position);
        }
    }
}