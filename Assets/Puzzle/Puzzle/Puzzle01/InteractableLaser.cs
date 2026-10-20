using UnityEngine;

public class InteractableLaser : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private string playerTag = "Player";

    [Header("Rotation Settings")]
    [SerializeField] private float scrollSensitivity = 15f;

    [Header("Status (Read Only)")]
    [SerializeField] private bool isPlayerInRange = false;
    [SerializeField] private bool isControlling = false;

    private void Update()
    {
        // กด E เพื่อสลับโหมดควบคุมเฉพาะวัตถุชิ้นนี้
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isControlling = !isControlling;
        }

        // หมุนเฉพาะเมื่อวัตถุชิ้นนี้ถูกเลือกควบคุมอยู่เท่านั้น
        if (isControlling)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                transform.Rotate(0, 0, scroll * scrollSensitivity * 10f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
            isControlling = false; // เมื่อเดินออกจากระยะ ให้ยกเลิกการควบคุมทันที
        }
    }
}