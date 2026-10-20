using UnityEngine;

public class Altar : MonoBehaviour
{
    [Header("Door & Level Progress")]
    [SerializeField] private DoorController firstDoor; // ประตูบานแรก
    [SerializeField] private int requiredOrbsForFinalDoor = 3; // จำนวน Orb รวมทั้งหมดที่ต้องใช้เปิดประตูสุดท้าย (1 ลูกแรก + อีก 2 ลูก)

    [Header("Altar Snap Settings")]
    [SerializeField] private Transform snapPoint; // จุดล็อคตำแหน่ง Orb บนแท่น

    private int currentOrbCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckAndActivate(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckAndActivate(collision);
    }

    private void CheckAndActivate(Collider2D collision)
    {
        Orb orb = collision.GetComponent<Orb>();

        // ต้องเป็น Orb ที่ยังไม่ถูกแท่นล็อค และผู้เล่นไม่ได้อุ้มอยู่
        if (orb != null && !orb.IsCarried() && orb.enabled)
        {
            PlaceOrbOnAltar(orb);
        }
    }

    private void PlaceOrbOnAltar(Orb orb)
    {
        // 1. เพิ่มจำนวนลูกแก้วที่นำมาวาง
        currentOrbCount++;

        // 2. ล็อคตำแหน่ง Orb ให้อยู่บนแท่น
        Vector3 targetPos = snapPoint != null ? snapPoint.position : transform.position;
        orb.transform.position = targetPos;

        // 3. ปิดฟิสิกส์และการรับปุ่มของ Orb เพื่อไม่ให้เก็บได้อีก
        Rigidbody2D orbRb = orb.GetComponent<Rigidbody2D>();
        if (orbRb != null)
        {
            orbRb.bodyType = RigidbodyType2D.Kinematic;
            orbRb.linearVelocity = Vector2.zero;
        }

        Collider2D orbCol = orb.GetComponent<Collider2D>();
        if (orbCol != null) orbCol.enabled = false;

        // สั่งหยุดระบบ Decay (เวลานับถอยหลัง) ของ Orb
        orb.enabled = false;

        // 4. เงื่อนไขการเปิดประตู
        // วาง Orb ลูกแรกสำเร็จ -> เปิดประตูแรก
        if (currentOrbCount == 1 && firstDoor != null)
        {
            firstDoor.OpenDoor();
        }

        // วาง Orb ครบตามจำนวนทั้งหมด (เช่น 3 ลูก) -> เปิดประตูสุดท้าย / ชนะผ่านด่าน
        if (currentOrbCount >= requiredOrbsForFinalDoor)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteLevel();
            }
        }
    }
}