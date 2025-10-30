/**
* Programmed by: Knox Fouladi
*
* SUMMARY
* This script acts as a visual handler for all achieved combo display needs
*
* CURRENTLY
* Working to make overall counter (located top right) work as intended at a base level
*
* TO DO
* - Further polish overall counter
* - integrate pop up combo feedback in the future when we get there
*/
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class ComboCounterVisuals : MonoBehaviour
{
    [SerializeField]
    private TMP_Text comboCountDisplay;

    // testing stuff
    public int currentComboCount = 0;
    [field: SerializeField] public GameObject perfectCombo;
    [field: SerializeField] public GameObject goodCombo;
    [field: SerializeField] public GameObject missCombo;

    // animation name Constants

    // counter (top right corner)
    const string COUNT_DISPLAY_MISS = "";
    const string COUNT_DISPLAY_GOOD = "";
    const string COUNT_DISPLAY_PERFECT = "";

    // Pop-ups (shown near player, uses object on player object)
    const string POP_UP_MISS = "";
    const string POP_UP_GOOD = "";
    const string POP_UP_PERFECT = "";

    private void Start()
    {
        perfectCombo.GetComponent<Button>().onClick.AddListener(HandleComboVisualsPerfect);
        goodCombo.GetComponent<Button>().onClick.AddListener(HandleComboVisualsGood);
        missCombo.GetComponent<Button>().onClick.AddListener(HandleComboVisualsMiss);

        comboCountDisplay.text = currentComboCount.ToString();
    }

    //testing method
    public void HandleComboVisualsMiss()
    {
        currentComboCount = 0;
        UpdateCount(currentComboCount, ComboType.Miss);
        AnimateCombo(ComboType.Miss);

        Debug.Log("MISS");
    }

    public void HandleComboVisualsGood()
    {
        currentComboCount++;
        UpdateCount(currentComboCount, ComboType.Good);
        AnimateCombo(ComboType.Good);

        Debug.Log("GOOD");

    }

    public void HandleComboVisualsPerfect()
    {
        currentComboCount++;
        UpdateCount(currentComboCount, ComboType.Perfect);
        AnimateCombo(ComboType.Perfect);

        Debug.Log("PERFECT");

    }

    public void HandleComboVisuals(int playerID, ComboType comboType, int currentComboCount)
    {
        switch (comboType)
        {
            case ComboType.Miss:
                {
                    UpdateCount(currentComboCount, comboType);
                    break;
                }
            case ComboType.Good:
                {
                    UpdateCount(currentComboCount, comboType);
                    break;
                }
            case ComboType.Perfect:
                {
                    UpdateCount(currentComboCount, comboType);
                    break;
                }
            default:
                {
                    return;
                }
        }

        AnimateCombo(comboType);
    }

    private void AnimateCombo(ComboType combo)
    {
        switch (combo)
        {
            case ComboType.Miss:
                {
                    // ANIMATE MISS
                    break;
                }
            case ComboType.Good:
                {
                    // ANIMATE GOOD
                    break;
                }
            case ComboType.Perfect:
                {
                    //ANIMATE PERFECT
                    break;
                }
            default:
                {
                    return;
                }
        }
    }

    private void UpdateCount(int newComboCount, ComboType comboType)
    {
        comboCountDisplay.text = newComboCount.ToString();
    }


    // temporary until more integrated with programming’s mechanics
    public enum ComboType
    {
        Miss,
        Good,
        Perfect
    }

}