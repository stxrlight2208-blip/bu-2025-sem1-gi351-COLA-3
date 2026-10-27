
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;

    private Vector3 startPosition;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(
            Vector2.right * direction * moveSpeed * Time.deltaTime
        );

        float distance = transform.position.x - startPosition.x;

        if (distance >= patrolDistance)
        {
            direction = -1;
            Flip();
        }
        else if (distance <= -patrolDistance)
        {
            direction = 1;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
    }
}
