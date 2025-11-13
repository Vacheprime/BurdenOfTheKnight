using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq.Expressions;
using System;

public class HelpController : MonoBehaviour
{
    public GameObject helpPanel;
    // Start button options
    public Button startButton;
    public Button optionsButton;
    public Button helpButton;
    public Button quitButton;
    public TMP_Text title;

    // Help button options
    public Button storyButton;

    public List<TMP_Text> texts;

    // Update is called once per frame
    void Update()
    {
       
    }

    public void StoryButtonClicked()
    {
        activeState("PrologueText");
    }

    public void MovementButtonClicked()
    {
        activeState("MovementText");
    }

    public void CombatButtonClicked()
    {
        activeState("CombatText");
    }

    public void EquipmentButtonClicked()
    {
        activeState("EquipmentText");
    }

    public void SpellsButtonClicked()
    {
        activeState("SpellsText");
    }
    
    public void CancelButtonClicked()
    {
        helpPanel.SetActive(false);
        startButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);
        helpButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
    }

    public void HelpButton()
    {
        startButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        helpButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        helpPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(storyButton.gameObject);
        StoryButtonClicked();
    }

    public void activeState(string activeText)
    {
        foreach(TMP_Text text in texts)
        {
            
            if (text.name == activeText)
            {
                text.gameObject.SetActive(true);
            } else
            {
                text.gameObject.SetActive(false);
            }
        }
    }

}
