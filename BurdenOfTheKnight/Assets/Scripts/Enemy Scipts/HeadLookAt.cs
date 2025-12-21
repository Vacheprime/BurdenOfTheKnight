using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
    [Header("Bones")]
    public Transform headBone;   // mixamorig:Head
    public Transform headAim;    // HeadAim (child)

    [Header("Target")]
    public string targetTag = "Player";
    public Transform target;

    [Header("Tuning")]
    public float turnSpeed = 6f;
    public float maxYaw = 70f;

    Quaternion initialLocalRot;

    void Awake()
    {
        if (!target)
        {
            GameObject p = GameObject.FindGameObjectWithTag(targetTag);
            if (p) target = p.transform;
        }

        if (headBone)
            initialLocalRot = headBone.localRotation;
    }

    void LateUpdate()
    {
        if (!headBone || !headAim || !target) return;

        // 1️⃣ Aim the helper
        Vector3 dir = target.position - headAim.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;

        headAim.rotation = Quaternion.LookRotation(dir, Vector3.up);

        // 2️⃣ Convert helper rotation into head-local space
        Quaternion parentRot = headBone.parent.rotation;
        Quaternion desiredLocal =
            Quaternion.Inverse(parentRot) * headAim.rotation;

        // 3️⃣ Clamp yaw (prevents creepy twisting)
        Vector3 e = desiredLocal.eulerAngles;
        float yaw = e.y > 180f ? e.y - 360f : e.y;
        yaw = Mathf.Clamp(yaw, -maxYaw, maxYaw);

        Quaternion finalRot =
            Quaternion.Euler(0f, yaw, 0f) * initialLocalRot;

        // 4️⃣ Smooth apply
        headBone.localRotation =
            Quaternion.Slerp(headBone.localRotation, finalRot, Time.deltaTime * turnSpeed);
    }
}
