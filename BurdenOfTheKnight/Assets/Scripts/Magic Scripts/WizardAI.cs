using UnityEngine;
using UnityEngine.AI;

public class WizardAI2 : MonoBehaviour
{
    public float detectRange = 18f;
    public float attackRange = 12f;
    public float shootCooldown = 2.0f;

    public Transform spellSpawn;     
    public GameObject spellPrefab;   

    NavMeshAgent agent;
    Transform player;
    Animator anim;
    float nextShot;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
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
                if (anim) anim.SetFloat("Speed", agent.velocity.magnitude);
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

                if (anim) anim.SetTrigger("Cast");
            }
        }
        else
        {
            agent.isStopped = true;
            if (anim) anim.SetFloat("Speed", 0f);
        }
    }

    void ShootSpell()
    {
        if (!spellPrefab || !spellSpawn || !player) return;

        GameObject spell = Instantiate(spellPrefab, spellSpawn.position, spellSpawn.rotation);

        Vector3 target = new Vector3(player.position.x, spellSpawn.position.y, player.position.z);
        Vector3 dir = (target - spellSpawn.position).normalized;

        Rigidbody rb = spell.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = dir * 20f;
    }
}
