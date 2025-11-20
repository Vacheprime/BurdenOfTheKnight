using UnityEngine;

public class BatAI : MonoBehaviour
{
    public float detectRange = 18f;
    public float stopDistance = 6f;
    public float moveSpeed = 5f;
    public float hoverHeight = 3f;
    public float verticalLerpSpeed = 5f;

    public float shootCooldown = 2.0f;
    public Transform shootPoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 15f;

    Transform player;
    Animator anim;
    float nextShotTime;

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!player) return;

        Vector3 toPlayer = player.position - transform.position;
        Vector3 flatDir = new Vector3(toPlayer.x, 0f, toPlayer.z);
        float distXZ = flatDir.magnitude;

        if (distXZ <= detectRange && flatDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(flatDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
        }

        bool shouldMove = distXZ > stopDistance && distXZ <= detectRange;

        if (shouldMove)
        {
            Vector3 targetPos = player.position - flatDir.normalized * stopDistance;
            targetPos.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (anim) anim.SetBool("isMoving", true);
        }
        else
        {
            if (anim) anim.SetBool("isMoving", false);
        }

        float targetY = player.position.y + hoverHeight;
        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, targetY, verticalLerpSpeed * Time.deltaTime);
        transform.position = pos;

        if (distXZ <= stopDistance)
        {
            TryShoot();
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
        if (!shootPoint || !projectilePrefab || !player) return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        if (proj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 dir = (player.position - shootPoint.position).normalized;
            rb.linearVelocity = dir * projectileSpeed;
        }
    }
}
