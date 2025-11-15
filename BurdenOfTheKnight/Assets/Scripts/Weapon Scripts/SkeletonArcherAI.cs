using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SkeletonArcherAI : MonoBehaviour
{
    public float detectRange = 14f, loseSightRange = 18f, shootRange = 12f, shootCooldown = 1.5f;
    public GameObject arrowPrefab;     
    public Transform arrowSpawn;    
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
            agent.isStopped = false;
        }
        if (anim) anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    void Shoot()
    {
        if (arrowPrefab == null || arrowSpawn == null) return;
        Vector3 playerPos = player.position;
        Vector3 flatTarget = new Vector3(playerPos.x, arrowSpawn.position.y, playerPos.z);
        Vector3 dir = (flatTarget - arrowSpawn.position).normalized;
        GameObject arrowObj = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.LookRotation(dir));
        arrowObj.GetComponent<ArrowProjectile>().Fire(dir);

        if (anim) anim.SetTrigger("RightAttack");
    }
}
