using UnityEngine;
using UnityEngine.AI;

public class BatAI : MonoBehaviour
{
    [Header("Detection & Movement")]
    public float detectRange = 18f;      
    public float stopDistance = 6f;      

    [Header("Attack")]
    public float shootCooldown = 2.0f;   
    public Transform shootPoint;       
    public GameObject projectilePrefab;
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
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }

            if (dist > stopDistance)
            {
                if (agent)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }

                if (anim) anim.SetBool("isMoving", true);
            }
            else
            {
                if (agent) agent.isStopped = true;
                if (anim) anim.SetBool("isMoving", false);

                TryShoot();
            }
        }
        else
        {
            if (agent) agent.isStopped = true;
            if (anim) anim.SetBool("isMoving", false);
        }
    }

    void TryShoot()
    {
        if (Time.time < nextShotTime) return;
        nextShotTime = Time.time + shootCooldown;

        if (anim) anim.SetTrigger("Attack");

        ShootProjectile();
    }

    public void ShootProjectile()
    {
        if (!shootPoint || !projectilePrefab) return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        if (proj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = shootPoint.forward * projectileSpeed;
        }

        Destroy(proj, 5f);
    }
}
