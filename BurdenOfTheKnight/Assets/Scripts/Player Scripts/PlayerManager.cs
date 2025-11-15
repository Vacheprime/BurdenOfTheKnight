using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public AudioSource source;
    public AudioLoudnessDetection detector;
    private float loudnessSensitivity = 100;
    private float threshold = 0.1f;

    private float magicPoints;
    public Slider magicSlider;

    private float health;
    public Slider healthSlider;

    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want it to persist across scenes
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        magicPoints = 100;
        SetMagicPoints(magicPoints);
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMagicPoints();
    }

    public float GetMagicPoints()
    {
        return magicPoints;
    }

    public bool CastTestMagic()
    {
        if (magicPoints > 15)
        {
            magicPoints -= 15;
            return true;
        }
        else { return false; }
    }

    public void SetMagicPoints(float points)
    {
        magicSlider.value = points; 
    }

    public void SetHealth(float points)
    {
        healthSlider.value = points;
    }

    public void CalculateMagicPoints()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensitivity;

        if (loudness < threshold)
        {
            loudness = 0;
        }
        else if (loudness > 20)
        {
            loudness = 20;
        }
        magicPoints += loudness / 1500;
        if (magicPoints > 100)
        {
            magicPoints = 100;
        }
        SetMagicPoints(magicPoints);
        Debug.Log("Loudness: " + loudness);
        Debug.Log("Magic Points: " + magicPoints);
    }

}
