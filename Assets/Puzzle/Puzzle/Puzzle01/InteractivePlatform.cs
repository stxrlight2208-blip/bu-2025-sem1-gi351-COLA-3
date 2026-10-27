using System.Collections;
using UnityEngine;

public class InteractivePlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform targetPosition; // จุดปลายทาง
    [SerializeField] private float moveSpeed = 3f;      // ความเร็วในการเลื่อน
    [SerializeField] private float waitTimeAtEnds = 1f; // เวลาหยุดพักสั้นๆ ตรงปลายทางแต่ละฝั่ง

    [Header("Loop Control")]
    [SerializeField] private bool isLooping = true;    // ติ๊กถูกถ้าต้องการให้มันลูปตั้งแต่เริ่มเกมเลย

    private Vector3 startPosition;
    private Vector3 currentDestination;
    private bool isMoving = false;
    private Coroutine loopCoroutine;

    private void Start()
    {
        startPosition = transform.position;

        if (targetPosition != null)
        {
            currentDestination = targetPosition.position;
        }

        // ถ้าตั้งค่าให้ลูปตั้งแต่เริ่มเกม ให้เริ่มทำงานทันที
        if (isLooping)
        {
            StartLooping();
        }
    }

    // ฟังก์ชันสั่งเปิด/ปิด การลูป (เรียกใช้จาก ObjectSwitch หรือสคริปต์อื่นได้)
    public void ToggleLoop()
    {
        isLooping = !isLooping;

        if (isLooping)
        {
            StartLooping();
        }
        else
        {
            StopLooping();
        }
    }

    // ฟังก์ชันบังคับเปิดลูป
    public void StartLooping()
    {
        isLooping = true;
        if (loopCoroutine == null)
        {
            loopCoroutine = StartCoroutine(LoopRoutine());
        }
    }

    // ฟังก์ชันบังคับหยุดลูป
    public void StopLooping()
    {
        isLooping = false;
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }
    }

    private IEnumerator LoopRoutine()
    {
        while (isLooping)
        {
            if (targetPosition == null) yield break;

            // 1. เคลื่อนที่ไปหาจุดหมายปลายทางปัจจุบัน
            while (Vector3.Distance(transform.position, currentDestination) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentDestination, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = currentDestination; // ล็อกตำแหน่งให้เป๊ะ

            // 2. หยุดพักสั้นๆ ตรงปลายทาง
            yield return new WaitForSeconds(waitTimeAtEnds);

            // 3. สลับจุดหมายระหว่างจุดเริ่มต้นกับจุดเป้าหมาย
            currentDestination = (currentDestination == targetPosition.position) ? startPosition : targetPosition.position;
        }

        loopCoroutine = null;
    }

    // ยึด Player ติดกับวัตถุเวลาลอย/ขยับ เพื่อไม่ให้ตัวละครสไลด์ตกจากแท่น
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
        if (targetPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetPosition.position);
        }
    }
}