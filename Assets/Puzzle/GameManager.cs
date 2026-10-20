using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Conditions")]
    public int totalAltars = 2;
    private int chargedAltars = 0;

    [Header("UI & Exit")]
    public GameObject exitDoor; // ประตูทางออก
    public GameObject winUI;   // หน้าจอชนะ (Win Screen UI)

    private void Awake()
    {
        Instance = this;
    }

    public void OnAltarCharged()
    {
        chargedAltars++;

        // เมื่อชาร์จครบตามจำนวนแท่นใน map (2 แท่น)
        if (chargedAltars >= totalAltars)
        {
            OpenExitDoor();
        }
    }

    private void OpenExitDoor()
    {
        if (exitDoor != null) exitDoor.SetActive(false); // ปิดประตูที่กั้นทางออก หรือเล่น Animation เปิดประตู
    }

    public void TriggerWin()
    {
        if (winUI != null) winUI.SetActive(true); // แสดงสถานะชนะเมื่อผู้เล่นเดินถึงจุด exit
        Time.timeScale = 0f; // หยุดเกม
    }
}