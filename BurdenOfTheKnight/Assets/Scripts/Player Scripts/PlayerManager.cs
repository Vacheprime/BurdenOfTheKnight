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

    public AudioSource audioSource;
    public AudioClip clip;
    public AudioClip clip2;

    public static bool playerJustRespawned = false;



    public static PlayerManager Instance { get; private set; }

    private bool isDead = false;

    // ======TEST============
    public RectTransform healthFill;
    public RectTransform spellsFill;
    public float currentHealthFill = 0f;
    public float currentSpellsFill = -300f;
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
        TakeDamage(0);
        SetMagicSpell(0, currentSpellsFill);
        // Play respawn sound
        if (playerJustRespawned)
        {
            audioSource.PlayOneShot(clip2);
            playerJustRespawned = false;
        }
    }


    void Update()
    {    
        CalculateMagicPoints();
    }

    public float GetMagicPoints() => magicPoints;

    public bool CastTestMagic()
    {
        if (currentSpellsFill > -250)
        {
            currentSpellsFill -= 15;
            return true;
        }
        return false;
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
        AudioManager.Instance.PlayClip(clip2);
    }

    public void TakeDamage(float right)
    {
        if (currentHealthFill >= 300f)
        {
            isDead = true;
            Die();
        }

        audioSource.PlayOneShot(clip);
        currentHealthFill += right;

        Vector2 min = healthFill.offsetMin;
        Vector2 max = healthFill.offsetMax;

        healthFill.offsetMin = new Vector2(0, min.y);
        healthFill.offsetMax = new Vector2(-currentHealthFill, max.y);
    }

    public void SetMagicSpell(float left, float right)
    {
        
        Vector2 min = spellsFill.offsetMin;
        Vector2 max = spellsFill.offsetMax;

        spellsFill.offsetMin = new Vector2(left, min.y);
        spellsFill.offsetMax = new Vector2(-right, max.y);
    }

    public void CalculateMagicPoints()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensitivity;

        if (loudness < threshold) loudness = 0;
        else if (loudness > 20) loudness = 20;

        currentSpellsFill += loudness / 8;
        if (currentSpellsFill >= 0) currentSpellsFill = 0;

        Vector2 min = spellsFill.offsetMin;
        Vector2 max = spellsFill.offsetMax;

        spellsFill.offsetMin = new Vector2(0, min.y);
        spellsFill.offsetMax = new Vector2(currentSpellsFill, max.y);
    }

    public void IncreaseHealth(float healthAmount)
    {
        SetHealth(currentHealth + healthAmount);
    }

    public void SetMicSensitivity(float value)
    {
        loudnessSensitivity = Mathf.Clamp(value, 10f, 200f);
    }

    public float GetLoudnessSensitivity()
    {
        return loudnessSensitivity;
    }
}
