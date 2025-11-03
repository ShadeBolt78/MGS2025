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
using System;
using System.Collections;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ComboCounterVisuals : MonoBehaviour
{
    [SerializeField]
    private TMP_Text comboCountDisplay;
    [SerializeField]
    private Animator comboCounterAnimator; // Top Right Counter

    // testing stuff
    public int currentComboCount = 0;
    [field: SerializeField] public GameObject perfectCombo;
    [field: SerializeField] public GameObject goodCombo;
    [field: SerializeField] public GameObject missCombo;
    public int timeBeforeFade;
    IEnumerator currentPopUp;


    // animation name Constants

    // counter (top right corner)
    const string COUNT_DISPLAY_MISS = "";
    const string COUNT_DISPLAY_GOOD = "";
    const string COUNT_DISPLAY_PERFECT = "PerfectCOMBO";

    // Pop-ups (shown near player, uses object on player object)
    const string POP_UP_MISS = "";
    const string POP_UP_GOOD = "";
    const string POP_UP_PERFECT = "POPUP_PERFECT";
    const string FADE_PERFECT = "FADE_PERFECT";
    const string ONGOING_COMBO = "OngoingCombo";

    private void Start()
    {
        perfectCombo.GetComponent<Button>().onClick.AddListener(HandleComboVisualsPerfect);
        goodCombo.GetComponent<Button>().onClick.AddListener(HandleComboVisualsGood);
        missCombo.GetComponent<Button>().onClick.AddListener(HandleComboVisualsMiss);

        comboCountDisplay.text = currentComboCount.ToString();
    }

    //testing method

    public void PopUpComboTimeTester()
    {
        if (currentPopUp != null)
        { 
            StopCoroutine(currentPopUp);
        }

        currentPopUp = PopUpCooldown();
        StartCoroutine(currentPopUp);
    }

    IEnumerator PopUpCooldown()
    {
        comboCounterAnimator.SetBool(ONGOING_COMBO, true);
        comboCounterAnimator.SetTrigger(POP_UP_PERFECT);
        yield return new WaitForSeconds(timeBeforeFade);
        comboCounterAnimator.SetTrigger(FADE_PERFECT);


    }

    public void SetOngoingBoolFalse() // Animation event!!!!!
    {
        comboCounterAnimator.SetBool(ONGOING_COMBO, false);
    }

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
        HandleComboVisuals(0, ComboType.Perfect, currentComboCount);

        Debug.Log("PERFECT");

    }

    public void HandleComboVisuals(int playerID, ComboType comboType, int currentComboCount)
    {
        AnimateCombo(comboType);

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
                    comboCounterAnimator.SetTrigger(COUNT_DISPLAY_PERFECT);
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