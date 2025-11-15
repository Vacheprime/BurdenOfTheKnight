using UnityEngine;
using UnityEngine.AI;

public class SkeletonEnemyAI : MonoBehaviour
{
    [Header("Detection")]
    public float detectRange = 12f;
    public float loseSightRange = 16f;     
    public float stopDistance = 1.5f;

    [Header("Movement")]
    public float patrolRadius = 5f;     
    public float idleRepathTime = 3f;

    NavMeshAgent agent;
    Transform player;
    Animator anim;
    float idleTimer;
    bool chasing;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float d = Vector3.Distance(transform.position, player.position);

        if (!chasing && d <= detectRange)  chasing = true;
        if (chasing  && d >= loseSightRange) chasing = false;

        if (chasing)
        {
            agent.stoppingDistance = stopDistance;
            agent.SetDestination(player.position);
        }
        else
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f)
            {
                Vector3 random = transform.position + Random.insideUnitSphere * patrolRadius;
                if (NavMesh.SamplePosition(random, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    agent.SetDestination(hit.position);
                idleTimer = idleRepathTime;
            }
            agent.stoppingDistance = 0f;
        }

        if (anim) anim.SetFloat("Speed", agent.velocity.magnitude);
    }
}
