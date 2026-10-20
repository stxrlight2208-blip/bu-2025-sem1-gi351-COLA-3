using UnityEngine;
using System;

public class LaserReceiver : MonoBehaviour
{
    [Header("Status")]
    public bool isHit = false;

    // Event สำหรับส่งสัญญาณบอกประตูเปิด/ปิด
    public Action<bool> OnHitStateChanged;

    private bool previousHitState = false;

    private void LateUpdate()
    {
        // ส่งสัญญาณเมื่อสถานะเปลี่ยน (ยิงโดน / ไม่โดน)
        if (isHit != previousHitState)
        {
            OnHitStateChanged?.Invoke(isHit);
            previousHitState = isHit;
        }

        // รีเซ็ตเป็น false ทุกเฟรม
        isHit = false;
    }

    // ฟังก์ชันนี้ถูกเรียกโดยสคริปต์ LaserEnmitted.cs
    public void OnHitByLaser()
    {
        isHit = true;
    }

    // รองรับชื่อเดิมไว้ด้วยกันบัค
    public void ReceiveLaser()
    {
        isHit = true;
    }
}