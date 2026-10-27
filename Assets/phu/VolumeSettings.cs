using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("UI Slider")]
    public Slider volumeSlider;

    void Start()
    {
        // ตรวจสอบว่ามีการบันทึกค่าเสียงเดิมไว้ไหม ถ้าไม่มีให้เริ่มที่ค่า 1 (สูงสุด)
        float currentVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        
        if (volumeSlider != null)
        {
            volumeSlider.value = currentVolume;
            // ผูกฟังก์ชันเข้ากับ Slider เมื่อมีการเลื่อน
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // ตั้งค่าเสียงเริ่มต้น
        AudioListener.volume = currentVolume;
    }

    public void SetVolume(float sliderValue)
    {
        // ปรับระดับเสียงทั้งหมดในเกม (ค่าอยู่ระหว่าง 0 ถึง 1)
        AudioListener.volume = sliderValue;
        
        // บันทึกค่าเสียงเก็บไว้ใช้ต่อแม้จะปิดเกม
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        PlayerPrefs.Save();
    }
}