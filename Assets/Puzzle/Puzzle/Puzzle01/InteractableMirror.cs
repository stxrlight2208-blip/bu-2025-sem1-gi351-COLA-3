using UnityEngine;

public class InteractableMirror : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private string playerTag = "Player";

    [Header("Rotation Settings")]
    [SerializeField] private float scrollSensitivity = 15f; // ความไวในการหมุนองศาเมื่อเลื่อน Scroll Wheel

    [Header("Status (Read Only)")]
    [SerializeField] private bool isPlayerInRange = false;
    [SerializeField] private bool isControlling = false;

    private GameObject playerObj;

    private void Update()
    {
        // กด E เพื่อเข้าหรือออกจากโหมดควบคุมกระจก
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isControlling = !isControlling;

            // ล็อค/ปลดล็อคการเคลื่อนที่ของผู้เล่น (ถ้ามีสคริปต์ PlayerMovement)
            if (playerObj != null)
            {
                MonoBehaviour movementScript = playerObj.GetComponent<MonoBehaviour>(); // หรือใส่ชื่อสคริปต์เดินของคุณ เช่น PlayerController
            }
        }

        // เมื่ออยู่ในโหมดควบคุม ให้รับค่า Scroll Wheel เพื่อหมุนกระจก
        if (isControlling)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                // หมุนตามค่า Scroll Wheel
                transform.Rotate(0, 0, scroll * scrollSensitivity * 10f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;
            playerObj = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
            isControlling = false; // หลุดระยะแล้วยกเลิกการควบคุมทันที
            playerObj = null;
        }
    }
}