using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq.Expressions;
using System;

public class OptionsController : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject keyboardPanel;
    public Button startButton;
    public Button optionsButton;
    public Button helpButton;
    public Button quitButton;
    public Button backButton;
    public Button keyboardButton;
    public TMP_Text title;
    public Button controlsButton;
    public GameObject controlsCenter;
    public Slider mouseSlider;
    public TMP_Text sensitivity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sensitivity.text = mouseSlider.value + "";
        
    }

    // Update is called once per frame
    void Update()
    {
        sensitivity.text = mouseSlider.value.ToString("F2") + "";
    }

    public void Back()
    {
        startButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);
        helpButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        optionsPanel.SetActive(false);
    }
    public void ConrolsCenterButton()
    {
        controlsCenter.SetActive(true);
        keyboardPanel.SetActive(false);
    }

    public void OptionsButton()
    {
        startButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        helpButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        optionsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(controlsButton.gameObject);
        ConrolsCenterButton();
    }

    public void KeyboardCenterButton()
    {
        controlsCenter.SetActive(false);
        keyboardPanel.SetActive(true);
    }

     public void Quit()
    {
        Application.Quit();
    }
    
}
