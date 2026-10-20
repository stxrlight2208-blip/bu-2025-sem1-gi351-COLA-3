using UnityEngine;

public class SubDoor : MonoBehaviour
{
    [Header("Target Receiver")]
    [Tooltip("ลาก LaserReceiver ที่ควบคุมประตูนี้มาใส่ช่องนี้")]
    [SerializeField] private LaserReceiver targetReceiver;

    [Header("Door Movement Settings")]
    [Tooltip("ระยะและทิศทางที่ประตูจะขยับไป เช่น Y = 3 คือเลื่อนขึ้นข้างบน 3 หน่วย")]
    [SerializeField] private Vector3 openOffset = new Vector3(0, 3f, 0);

    [Tooltip("ความเร็วในการค่อยๆ เลื่อนเปิด/ปิด")]
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool shouldOpen = false;

    private void Awake()
    {
        // บันทึกตำแหน่งประตูปัจจุบันเป็นตำแหน่งปิด
        closedPosition = transform.position;
        // คำนวณตำแหน่งเปิดจากตำแหน่งเดิม + ระยะ offset
        openPosition = closedPosition + openOffset;
    }

    private void OnEnable()
    {
        if (targetReceiver != null)
        {
            // ผูก Event คอยฟังสัญญาณยิงโดนจาก Receiver
            targetReceiver.OnHitStateChanged += ToggleDoor;
        }
    }

    private void OnDisable()
    {
        if (targetReceiver != null)
        {
            // ยกเลิกการฟังสัญญาณเมื่อปิดวัตถุ
            targetReceiver.OnHitStateChanged -= ToggleDoor;
        }
    }

    private void Update()
    {
        // กำหนดจุดหมายปลายทางตามสถานะเลเซอร์
        Vector3 targetPosition = shouldOpen ? openPosition : closedPosition;

        // ค่อยๆ เลื่อนตำแหน่งเข้าหาเป้าหมายอย่างเรียบเนียนนุ่มนวล
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void ToggleDoor(bool isOpen)
    {
        shouldOpen = isOpen;
    }
}