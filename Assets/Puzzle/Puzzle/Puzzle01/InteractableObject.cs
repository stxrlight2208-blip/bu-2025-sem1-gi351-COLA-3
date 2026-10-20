using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float scrollSensitivity = 15f;

    [Header("Status (Read Only)")]
    public bool isBeingControlled = false;

    private void Awake()
    {
        // บังคับปิดสถานะและปิดสคริปต์ทันทีตั้งแต่โหลด Scene
        isBeingControlled = false;
        this.enabled = false;
    }

    private void Start()
    {
        // ย้ำปิดสคริปต์ซ้ำอีกรอบใน Start เพื่อป้องกันการโดนเปิดจาก Event อื่นตอนเริ่มเกม
        isBeingControlled = false;
        this.enabled = false;
    }

    private void Update()
    {
        // สคริปต์นี้จะถูกเรียกทำงานเฉพาะตอนที่ถูกสั่ง this.enabled = true เท่านั้น
        if (isBeingControlled)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                transform.Rotate(0, 0, scroll * scrollSensitivity * 10f);
            }
        }
    }

    public void StartControl()
    {
        isBeingControlled = true;
        this.enabled = true; // เปิดให้ Update() ทำงานเฉพาะตอนกด E
    }

    public void StopControl()
    {
        isBeingControlled = false;
        this.enabled = false; // ปิด Update() ทันทีเมื่อยกเลิกการคุม
    }
}