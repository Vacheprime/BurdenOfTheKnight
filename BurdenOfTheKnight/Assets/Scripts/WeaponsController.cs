using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WeaponsController : MonoBehaviour
{
    public GameObject weaponsPanel;
    public GameObject spellsPanel;
    
    public void changeToWeaponsPanel() {
        spellsPanel.SetActive(false);
        weaponsPanel.SetActive(true);
    }

    public void changeToSpellsPanel() {
        spellsPanel.SetActive(true);
        weaponsPanel.SetActive(false);
    }
}
