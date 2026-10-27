using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // ฟังก์ชันสำหรับไปหน้าเกม (ใส่ชื่อ Scene หน้าเกมของคุณตรงนี้)
    public void GoToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    // ฟังก์ชันสำหรับไปหน้าตั้งค่า (ใส่ชื่อ Scene หน้าตั้งค่าของคุณตรงนี้)
    public void GoToSettingsScene()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    // ฟังก์ชันสำหรับกลับมาหน้าเมนูหลัก
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // ฟังก์ชันสำหรับออกจากเกม
    public void QuitGame()
    {
        Debug.Log("Quit Game!"); // สำหรับทดสอบในหน้า Editor
        Application.Quit();
    }
}