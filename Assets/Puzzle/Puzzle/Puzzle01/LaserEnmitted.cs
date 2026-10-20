using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private int maxReflections = 10;
    [SerializeField] private LayerMask mirrorLayer;
    [SerializeField] private LayerMask targetLayer;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        CastLaser();
    }

    private void CastLaser()
    {
        Vector2 position = transform.position;
        Vector2 direction = transform.up; // ยิงออกตามทิศทาง Up ของตัวยิง

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, position);

        for (int i = 0; i < maxReflections; i++)
        {
            // ยิง Raycast หา Mirror หรือ Target
            RaycastHit2D hit = Physics2D.Raycast(position, direction, maxDistance, mirrorLayer | targetLayer);

            if (hit.collider != null)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                // ชนเป้าหมาย (LaserReceiver)
                LaserReceiver receiver = hit.collider.GetComponent<LaserReceiver>();
                if (receiver != null)
                {
                    receiver.OnHitByLaser();
                    break;
                }

                // ชนกระจก (Mirror)
                if (((1 << hit.collider.gameObject.layer) & mirrorLayer) != 0)
                {
                    // สะท้อนตาม Normal ของ Collider ที่ชน
                    direction = Vector2.Reflect(direction, hit.normal);

                    // ดันจุดเริ่มต้นยิงใหม่ออกมาจากจุดชน 0.01f เพื่อไม่ให้ชน Collider ตัวเองซ้ำ
                    position = hit.point + (direction * 0.01f);
                }
                else
                {
                    break;
                }
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, position + (direction * maxDistance));
                break;
            }
        }
    }
}