using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonSpawner : MonoBehaviour
{
    [Header("Prefab & Area")]
    public GameObject skeletonPrefab;      // assign your Skeleton prefab
    public BoxCollider spawnArea;          // assign the BoxCollider on this object

    [Header("Counts & Timing")]
    public int initialCount = 5;
    public int maxAlive = 15;
    public float respawnEvery = 8f;

    readonly List<GameObject> alive = new();

    void Start()
    {
        if (spawnArea == null) spawnArea = GetComponent<BoxCollider>();
        // initial wave
        for (int i = 0; i < initialCount; i++) TrySpawnOne();
        StartCoroutine(RespawnLoop());
    }

    IEnumerator RespawnLoop()
    {
        var wait = new WaitForSeconds(respawnEvery);
        while (true)
        {
            // purge nulls (killed/despawned)
            alive.RemoveAll(go => go == null);
            if (alive.Count < maxAlive) TrySpawnOne();
            yield return wait;
        }
    }

    void TrySpawnOne()
    {
        if (skeletonPrefab == null || spawnArea == null) return;

        // pick a random point inside the BoxCollider
        Vector3 local = new Vector3(
            Random.Range(-spawnArea.size.x * 0.5f, spawnArea.size.x * 0.5f),
            Random.Range(-spawnArea.size.y * 0.5f, spawnArea.size.y * 0.5f),
            Random.Range(-spawnArea.size.z * 0.5f, spawnArea.size.z * 0.5f)
        );
        Vector3 world = spawnArea.transform.TransformPoint(local + spawnArea.center);

        // project to a valid NavMesh position near that point
        if (NavMesh.SamplePosition(world, out NavMeshHit hit, 6f, NavMesh.AllAreas))
        {
            var go = Instantiate(skeletonPrefab, hit.position, Quaternion.identity);
            alive.Add(go);
        }
    }

    // visualize spawn volume
    void OnDrawGizmosSelected()
    {
        var col = spawnArea ? spawnArea : GetComponent<BoxCollider>();
        if (!col) return;
        Gizmos.color = new Color(0f, 1f, 0.3f, 0.25f);
        Matrix4x4 prev = Gizmos.matrix;
        Gizmos.matrix = col.transform.localToWorldMatrix;
        Gizmos.DrawCube(col.center, col.size);
        Gizmos.matrix = prev;
    }
}
