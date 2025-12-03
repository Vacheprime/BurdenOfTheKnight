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

    // ======TEST============
    public RectTransform healthFill;
    public RectTransform spellsFill;
    public float currentHealthFill = 0f;
    public float currentSpellsFill = 300f;
    public float minFill = 300f;
    //=======================

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
        SetMagicSpell(0, -300);
    }

    void Update()
    {
        if (currentHealthFill >= 300f)
        {
            currentHealthFill = 0f;
            ReduceHealthTest(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReduceHealthTest(0, 45);
        }

    
        CalculateMagicPoints();
        AugmentMagicPoints();
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

    public void ReduceHealthTest(float left, float right)
    {
        currentHealthFill += right;

        Vector2 min = healthFill.offsetMin;
        Vector2 max = healthFill.offsetMax;

        healthFill.offsetMin = new Vector2(left, min.y);
        healthFill.offsetMax = new Vector2(-currentHealthFill, max.y);
    }

    public void SetMagicSpell(float left, float right)
    {
        
        Vector2 min = spellsFill.offsetMin;
        Vector2 max = spellsFill.offsetMax;

        spellsFill.offsetMin = new Vector2(left, min.y);
        spellsFill.offsetMax = new Vector2(right, max.y);
    }

    public void AugmentMagicPoints()
    {
        Debug.Log(currentSpellsFill);
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensitivity;

        if (loudness < threshold) loudness = 0;
        else if (loudness > 20) loudness = 20;

        currentSpellsFill -= loudness / 1500f;
        // if (currentSpellsFill < 0) currentSpellsFill = 0;

        Vector2 min = spellsFill.offsetMin;
        Vector2 max = spellsFill.offsetMax;

        spellsFill.offsetMin = new Vector2(0, min.y);
        spellsFill.offsetMax = new Vector2(-currentSpellsFill, max.y);
    }
}
