using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Components")]
    [SerializeField] private Collider2D doorCollider; // PolygonCollider2D ของประตู

    private void Awake()
    {
        if (doorCollider == null)
            doorCollider = GetComponent<Collider2D>();
    }

    public void OpenDoor()
    {
        // ปิด Collider ของประตูเพื่อให้ Player เดินผ่านได้ทันที
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        Debug.Log(gameObject.name + " Opened!");
    }
}