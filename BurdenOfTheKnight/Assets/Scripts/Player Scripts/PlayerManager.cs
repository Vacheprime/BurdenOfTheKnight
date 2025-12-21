using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Diagnostics;

public class PlayerManager : MonoBehaviour
{
    public AudioSource source;
    public AudioLoudnessDetection detector;
    private float loudnessSensitivity;
    private float threshold = 0.1f;

    private float magicPoints;

    public float maxHealth = 100f;
    private float currentHealth;

    public AudioSource audioSource;
    public AudioClip clip;
    public AudioClip clip2;
    public TMP_Text levelTitle;

    public static bool playerJustRespawned = false;



    public static PlayerManager Instance { get; private set; }

    private bool isDead = false;

    // ======TEST============
    public RectTransform staminaFill;
    public RectTransform healthFill;
    public RectTransform spellsFill;
    public float currentStaminaFill = 0f;
    public float currentHealthFill = 0f;
    public float currentSpellsFill = -300f;
    public float minFill = 300f;
    //=======================
    public Slider micSlider;
    public TMP_Text micText;

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
        loudnessSensitivity = 100;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        TakeDamage(0);
        SetMagicSpell(0, currentSpellsFill);
        CalculateStamina(currentHealthFill);
        // Play respawn sound
        if (playerJustRespawned)
        {
            audioSource.PlayOneShot(clip2);
            playerJustRespawned = false;
        }

        micText.text = micSlider.value.ToString("F2");
        micSlider.value = loudnessSensitivity;
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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            levelTitle.text = "Level 1/5 ";
        } else if (scene.name == "Level2")
        {
            levelTitle.text = "Level 2/5 ";
        }
        else if (scene.name == "Level3")
        {
            levelTitle.text = "Level 3/5 ";
        }
        else if (scene.name == "Level4")
        {
            levelTitle.text = "Level 4/5";
        }
        else if (scene.name == "FinalLevelScene")
        {
            levelTitle.text = "Level 5/5";
        }
        
    }

     public void SetHealth(float points)
    {
        /*
        currentHealth = Mathf.Clamp(points, 0, maxHealth);
        Debug.Log(healthSlider);
        if (healthSlider != null)
            healthSlider.value = currentHealth;
        */

        currentHealthFill = Mathf.Clamp(currentHealthFill - points, 0, 300);
        Vector2 min = healthFill.offsetMin;
        Vector2 max = healthFill.offsetMax;

        healthFill.offsetMin = new Vector2(0, min.y);
        healthFill.offsetMax = new Vector2(-currentHealthFill, max.y);
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
            return;
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
        loudnessSensitivity = Mathf.Clamp(value, 10f, 300f);
        micText.text = micSlider.value.ToString("F2");
    }

    public float GetLoudnessSensitivity()
    {
        return loudnessSensitivity;
    }

    public void CalculateStamina(float right)
    {
        currentStaminaFill = Mathf.Clamp(currentStaminaFill - right, 0f, 300f);
        Vector2 min = staminaFill.offsetMin;
        Vector2 max = staminaFill.offsetMax;

        staminaFill.offsetMin = new Vector2(0, min.y);
        staminaFill.offsetMax = new Vector2(-currentStaminaFill, max.y);
    }

    public float GetCurrentStaminaFill()
    {
        return currentStaminaFill;
    }
}
