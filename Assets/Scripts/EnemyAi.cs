using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [Header("이동 속도")]
    public float patrolSpeed = 1f;
    public float chaseSpeed = 3f;

    [Header("순찰 영역")]
    public Vector2 patrolCenter;
    public float patrolRadius = 5f;

    [Header("추적 설정")]
    public float chaseRange = 4f;
    public float returnRange = 6f;

    [Header("순찰 설정")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;
    public float arriveDistance = 0.3f;

    [Header("장애물 감지")]
    public float obstacleDetectRange = 1f;
    public LayerMask obstacleLayer;

    private Transform player;
    private Vector2 patrolTarget;
    private bool isChasing = false;
    private bool isWaiting = false;
    private float currentSpeed;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        if (patrolCenter == Vector2.zero)
        {
            patrolCenter = transform.position;
        }

        SetNewPatrolTarget();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
            isWaiting = false;
        }
        else if (distanceToPlayer > returnRange && isChasing)
        {
            isChasing = false;
            SetNewPatrolTarget();
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else if (!isWaiting)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        currentSpeed = patrolSpeed;
        Vector2 direction = (patrolTarget - (Vector2)transform.position).normalized;

        // 장애물 감지
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleDetectRange, obstacleLayer);

        if (hit.collider != null)
        {
            // 장애물 발견 → 새 목표 설정
            SetNewPatrolTarget();
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, currentSpeed * Time.deltaTime);
        FlipSprite(direction.x);

        float distance = Vector2.Distance(transform.position, patrolTarget);
        if (distance <= arriveDistance)
        {
            StartCoroutine(WaitAndSetNewTarget());
        }
    }

    void ChasePlayer()
    {
        currentSpeed = chaseSpeed;
        Vector2 direction = (player.position - transform.position).normalized;

        // 추적 중에도 장애물 감지
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleDetectRange, obstacleLayer);

        if (hit.collider == null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }

        FlipSprite(direction.x);
    }

    void SetNewPatrolTarget()
    {
        // 최대 10번 시도해서 장애물 없는 지점 찾기
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
            Vector2 testTarget = patrolCenter + randomPoint;

            Vector2 direction = (testTarget - (Vector2)transform.position).normalized;
            float distance = Vector2.Distance(transform.position, testTarget);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleLayer);

            if (hit.collider == null)
            {
                patrolTarget = testTarget;
                return;
            }
        }

        // 찾지 못하면 그냥 무작위 지점
        patrolTarget = patrolCenter + Random.insideUnitCircle * patrolRadius;
    }

    System.Collections.IEnumerator WaitAndSetNewTarget()
    {
        isWaiting = true;
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        SetNewPatrolTarget();
        isWaiting = false;
    }

    void FlipSprite(float directionX)
    {
        if (directionX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (directionX < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, returnRange);

        if (Application.isPlaying && !isChasing)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(patrolTarget, 0.2f);
            Gizmos.DrawLine(transform.position, patrolTarget);
        }
    }
}