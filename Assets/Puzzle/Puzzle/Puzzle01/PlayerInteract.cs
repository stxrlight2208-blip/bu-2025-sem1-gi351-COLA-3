using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Player Movement Script To Freeze")]
    [SerializeField] private MonoBehaviour playerMovementScript;

    private InteractableObject currentTarget = null;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // บังคับให้เริ่มเกมมาไม่มีวัตถุใดถูกเลือกอยู่เลย
        if (currentTarget != null)
        {
            currentTarget.StopControl();
        }
        currentTarget = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget == null)
            {
                TryStartInteraction();
            }
            else
            {
                StopInteraction();
            }
        }

        // ล็อคความเร็วผู้เล่นเมื่ออยู่ในโหมดควบคุม
        if (currentTarget != null && rb != null)
        {
            rb.linearVelocity = Vector2.zero; // หากใช้ Unity 2022 ลงไป ให้เปลี่ยนเป็น rb.velocity = Vector2.zero;
        }
    }

    private void TryStartInteraction()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactableLayer);
        if (hits.Length == 0) return;

        InteractableObject closestObj = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            InteractableObject obj = hit.GetComponent<InteractableObject>();
            if (obj != null)
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestObj = obj;
                }
            }
        }

        if (closestObj != null)
        {
            currentTarget = closestObj;
            currentTarget.StartControl();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = false;
            }
        }
    }

    private void StopInteraction()
    {
        if (currentTarget != null)
        {
            currentTarget.StopControl();
            currentTarget = null;
        }

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}