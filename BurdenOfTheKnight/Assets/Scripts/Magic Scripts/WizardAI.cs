using UnityEngine;
using UnityEngine.AI;

public class WizardAI : MonoBehaviour
{
    [Header("Detection & Attack")]
    public float detectRange = 18f;
    public float attackRange = 12f;
    public float shootCooldown = 2.0f;
    public float turnSpeed = 10f;

    [Header("Fireball Prefab")]
    public Transform spellSpawn;
    public GameObject spellPrefab;

    [Header("Animation")]
    public Animator animator;

    private NavMeshAgent agent;
    private Transform player;
    private float nextShotTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // IMPORTANT: we rotate manually when needed

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!player || !agent) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > detectRange)
        {
            // Idle
            agent.isStopped = true;
            if (animator) animator.SetFloat("Speed", 0f);
            return;
        }

        // In detection range
        if (dist > attackRange)
        {
            // Chase
            agent.isStopped = false;
            agent.SetDestination(player.position);

            if (animator)
                animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            // Attack
            agent.isStopped = true;

            // Face player smoothly
            FacePlayer();

            if (animator)
                animator.SetFloat("Speed", 0f);

            // Shoot only when cooldown is ready
            if (Time.time >= nextShotTime)
            {
                if (animator) animator.SetTrigger("Cast");
                ShootSpell();
                nextShotTime = Time.time + shootCooldown;
            }
        }
    }

    private void FacePlayer()
    {
        Vector3 look = player.position - transform.position;
        look.y = 0f;

        if (look.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
    }

    private void ShootSpell()
    {
        if (!spellPrefab || !spellSpawn || !player) return;

        // Aim slightly above player's center
        Vector3 target = player.position + Vector3.up * 1.5f;
        Vector3 dir = (target - spellSpawn.position).normalized;

        // Spawn facing the target direction
        GameObject spell = Instantiate(spellPrefab, spellSpawn.position, Quaternion.LookRotation(dir));

        // Fire using FireballProjectile
        FireballProjectile proj = spell.GetComponent<FireballProjectile>();
        if (proj != null)
        {
            proj.Fire(dir);
        }
        else
        {
            // Helpful debug if prefab is missing the script
            Debug.LogWarning("spellPrefab is missing FireballProjectile on the ROOT object.");
        }
    }
}
