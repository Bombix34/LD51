using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class InputTextUI : MonoBehaviour
{
    private bool isDisplayed=false;
    [SerializeField] private InputType concernedInput;
    [SerializeField] private SlotInputData inputData;
    [SerializeField] private TextMeshProUGUI textUI;

    private void Start()
    {
        switch(concernedInput)
        {
            case InputType.ingredientSlot1:
                textUI.text = inputData.ingredientSlotInputs[0].ToString();
                break;
            case InputType.ingredientSlot2:
                textUI.text = inputData.ingredientSlotInputs[1].ToString();
                break;
            case InputType.ingredientSlot3:
                textUI.text = inputData.ingredientSlotInputs[2].ToString();
                break;
            case InputType.ingredientSlot4:
                textUI.text = inputData.ingredientSlotInputs[3].ToString();
                break;
            case InputType.ingredientSlot5:
                textUI.text = inputData.ingredientSlotInputs[4].ToString();
                break;
            case InputType.ingredientSlot6:
                textUI.text = inputData.ingredientSlotInputs[5].ToString();
                break;
            case InputType.cookStation:
                textUI.text = inputData.cookStationInput.ToString();
                break;
            case InputType.cutStation:
                textUI.text = inputData.cutStationInput.ToString();
                break;
            case InputType.deliveryStation:
                textUI.text = inputData.deliveryStationInput.ToString();
                break;
            case InputType.fridgeStation:
                textUI.text = inputData.frigdeStationInput.ToString();
                break;
        }
        textUI.gameObject.transform.localScale = Vector3.zero;
        PlayerInput.OnPlayerPressTab += OnPlayerPressDisplayInput;
    }

    private void OnPlayerPressDisplayInput()
    {
        isDisplayed = !isDisplayed;
        if(isDisplayed)
        {
            textUI.gameObject.transform.DOScale(1f,0.2f);
        }
        else
        {
            textUI.gameObject.transform.DOScale(0f,0.2f);
        }
    }
}
