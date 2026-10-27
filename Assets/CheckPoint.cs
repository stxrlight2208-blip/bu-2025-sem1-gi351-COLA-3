using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector3 lastCheckpointPosition;
    public static bool hasCheckpoint = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // บันทึกตำแหน่ง Checkpoint ล่าสุด
            lastCheckpointPosition = transform.position;
            hasCheckpoint = true;
            Debug.Log("Checkpoint Saved: " + lastCheckpointPosition);
        }
    }
}