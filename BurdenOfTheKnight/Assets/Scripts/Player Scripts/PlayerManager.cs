using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public AudioSource source;
    public AudioLoudnessDetection detector;
    private float loudnessSensitivity = 100;
    private float threshold = 0.1f;

    private float magicPoints;
    public Slider magicSlider;

    public float maxHealth = 100f;
    private float currentHealth;

    public Slider healthSlider;

    public static PlayerManager Instance { get; private set; }

    private bool isDead = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        magicPoints = 100;
        SetMagicPoints(magicPoints);

        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    void Update()
    {
        CalculateMagicPoints();
    }

    public float GetMagicPoints() => magicPoints;

    public bool CastTestMagic()
    {
        if (magicPoints > 15)
        {
            magicPoints -= 15;
            return true;
        }
        return false;
    }

    public void SetMagicPoints(float points)
    {
        magicPoints = Mathf.Clamp(points, 0, 100);
        if (magicSlider != null)
            magicSlider.value = magicPoints;
    }

    public void CalculateMagicPoints()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensitivity;

        if (loudness < threshold) loudness = 0;
        else if (loudness > 20) loudness = 20;

        magicPoints += loudness / 1500f;
        if (magicPoints > 100) magicPoints = 100;

        SetMagicPoints(magicPoints);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SetHealth(currentHealth);

        if (currentHealth <= 0f)
        {
            isDead = true;
            Die();
        }
    }

    public void SetHealth(float points)
    {
        currentHealth = Mathf.Clamp(points, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
