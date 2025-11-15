using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SkeletonArcherAI : MonoBehaviour
{
    public float detectRange = 14f, loseSightRange = 18f, shootRange = 12f, shootCooldown = 1.5f;
    public GameObject arrowPrefab;     // ArrowProjectile
    public Transform arrowSpawn;       // ArrowSpawn under bow
    NavMeshAgent agent; Animator anim; Transform player; float nextShot;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent.stoppingDistance = 1.2f;
    }

    void Update() {
        if (!player) return;
        float d = Vector3.Distance(transform.position, player.position);
        bool chase = d <= detectRange ? true : d < loseSightRange;

        if (chase) {
            if (d > shootRange) { agent.isStopped = false; agent.SetDestination(player.position); }
            else {
                agent.isStopped = true;
                Vector3 look = player.position - transform.position; look.y = 0; if (look.sqrMagnitude>0.001f) transform.rotation = Quaternion.LookRotation(look);
                if (Time.time >= nextShot) { Shoot(); nextShot = Time.time + shootCooldown; }
            }
        } else {
            agent.isStopped = false; // optional wander omitted for brevity
        }
        if (anim) anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    void Shoot()
    {
        if (!arrowPrefab || !arrowSpawn) return;

        // Aim at player chest
        Vector3 aimPoint = player.position + Vector3.up * 1.2f;
        Vector3 dir = (aimPoint - arrowSpawn.position).normalized;

        // Visual check in Game view: should point from bow to player
        Debug.DrawRay(arrowSpawn.position, dir * 5f, Color.red, 0.5f);

        var go = Instantiate(arrowPrefab,
                            arrowSpawn.position,
                            Quaternion.LookRotation(dir)); // faces forward
        go.GetComponent<ArrowProjectile>()?.Fire(dir);

        // anim?.SetTrigger("RightAttack"); // only if you added that trigger
    }

}
