using UnityEngine;
using UnityEngine.AI;

public class BatAI : MonoBehaviour
{
    [Header("Detection & Movement")]
    public float detectRange = 18f;      // how far it can see you
    public float stopDistance = 6f;      // how close it gets before just shooting

    [Header("Attack")]
    public float shootCooldown = 2.0f;   // seconds between shots
    public Transform shootPoint;         // mouth / front
    public GameObject projectilePrefab;  // your arrow / fireball / whatever
    public float projectileSpeed = 15f;

    NavMeshAgent agent;
    Transform player;
    Animator anim;
    float nextShotTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (!player)
            Debug.LogError("BatAI: No object tagged 'Player' found!");
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectRange)
        {
            // Face the player (keep bat level)
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }

            if (dist > stopDistance)
            {
                // Move towards player
                if (agent)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }

                if (anim) anim.SetBool("isMoving", true);
            }
            else
            {
                // Stop & shoot
                if (agent) agent.isStopped = true;
                if (anim) anim.SetBool("isMoving", false);

                TryShoot();
            }
        }
        else
        {
            // Out of range → idle
            if (agent) agent.isStopped = true;
            if (anim) anim.SetBool("isMoving", false);
        }
    }

    void TryShoot()
    {
        if (Time.time < nextShotTime) return;
        nextShotTime = Time.time + shootCooldown;

        // trigger attack animation if you have one
        if (anim) anim.SetTrigger("Attack");

        ShootProjectile();
    }

    // You can also call this from an animation event
    public void ShootProjectile()
    {
        if (!shootPoint || !projectilePrefab) return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        if (proj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = shootPoint.forward * projectileSpeed;
        }

        // optional: auto-destroy after 5s
        Destroy(proj, 5f);
    }
}
