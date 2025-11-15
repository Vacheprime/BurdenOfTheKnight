using UnityEngine;
using UnityEngine.AI;

public class WizardAI : MonoBehaviour
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

        if (player == null)
            Debug.LogError("WizardAI: No object with tag 'Player' found!");
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

        // 1. Spawn spell with neutral rotation (we'll control direction in code)
        GameObject spell = Instantiate(spellPrefab, spellSpawn.position, Quaternion.identity);

        // 2. Aim at the player (slightly up so it doesn't just hit the feet)
        Vector3 target = player.position + Vector3.up * 1.5f; 
        Vector3 dir = (target - spellSpawn.position); // Fire() will normalize

        // 3. Tell the projectile which way to go
        WizardSpellProjectile proj = spell.GetComponent<WizardSpellProjectile>();
        if (proj != null)
            proj.Fire(dir);
    }

}
