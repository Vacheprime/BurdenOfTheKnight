using UnityEngine;
using UnityEngine.AI;

public class WizardAI : MonoBehaviour
{
    public float detectRange = 18f;
    public float attackRange = 12f;
    public float shootCooldown = 2.0f;

    public Transform spellSpawn;
    public GameObject spellPrefab;
    public Animator animator;

    NavMeshAgent agent;
    Transform player;
    float nextShot;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectRange)
        {
            if (dist > attackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);

                if (animator)
                    animator.SetFloat("Speed", agent.velocity.magnitude);
            }
            else
            {
                agent.isStopped = true;

                Vector3 look = player.position - transform.position;
                look.y = 0f;
                if (look.sqrMagnitude > 0.001f)
                    transform.rotation = Quaternion.LookRotation(look);

                if (Time.time >= nextShot)
                {
                    ShootSpell();
                    nextShot = Time.time + shootCooldown;
                }

                if (animator)
                    animator.SetTrigger("Cast");
            }
        }
        else
        {
            agent.isStopped = true;

            if (animator)
                animator.SetFloat("Speed", 0f);
        }
    }

    void ShootSpell()
    {
        if (!spellPrefab || !spellSpawn || !player) return;

        Vector3 target = player.position + Vector3.up * 1.5f;
        Vector3 dir = (target - spellSpawn.position).normalized;

        GameObject spell = Instantiate(
            spellPrefab,
            spellSpawn.position,
            Quaternion.LookRotation(dir)
        );

        WizardSpellProjectile proj = spell.GetComponent<WizardSpellProjectile>();
        if (proj != null)
            proj.Fire(dir);
    }
}
