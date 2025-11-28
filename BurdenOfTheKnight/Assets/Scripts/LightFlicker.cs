using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light lightSource;
    public float minIntensity = 1f;
    public float maxIntensity = 4f;
    public float flickerDelay = 0.3f;

    private float timer;

    void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light>();
        timer = flickerDelay;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            lightSource.intensity = Random.Range(minIntensity, maxIntensity);
            timer = flickerDelay;
        }
    }
}
