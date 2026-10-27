using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitch : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactRadius = 2f;

    [Header("Connected Target Platforms")]
    [Tooltip("ลากวัตถุที่มีสคริปต์ InteractivePlatform ทั้งหมดมาใส่ที่นี่")]
    [SerializeField] private List<InteractivePlatform> connectedPlatforms = new List<InteractivePlatform>();

    [Header("Switch Behavior")]
    [Tooltip("ถ้าติ๊กถูก: กด E แล้วจะสลับเปิด/หยุดลูป | ถ้าไม่ติ๊ก: กด E แล้วจะบังคับให้เริ่มวิ่งลูปทันที")]
    [SerializeField] private bool toggleMode = true;

    private Transform player;

    private void Start()
    {
        // ค้นหา Player อัตโนมัติจาก Tag "Player"
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // เช็กระยะห่างจากปุ่มและกดปุ่ม E
        if (distance <= interactRadius && Input.GetKeyDown(interactKey))
        {
            TriggerPlatforms();
        }
    }

    public void TriggerPlatforms()
    {
        foreach (InteractivePlatform platform in connectedPlatforms)
        {
            if (platform != null)
            {
                if (toggleMode)
                {
                    // สลับระหว่าง "เริ่มลูป" กับ "หยุดลูป"
                    platform.ToggleLoop();
                }
                else
                {
                    // บังคับให้เริ่มทำงานลูปทันที
                    platform.StartLooping();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // วาดวงกลมระยะกดปุ่มในหน้า Scene
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);

        // โยงเส้นไปหา Platform ทั้งหมดที่เชื่อมกับสวิตช์นี้
        Gizmos.color = Color.cyan;
        foreach (InteractivePlatform platform in connectedPlatforms)
        {
            if (platform != null)
            {
                Gizmos.DrawLine(transform.position, platform.transform.position);
            }
        }
    }
}