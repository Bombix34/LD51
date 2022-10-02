using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public IngredientSlot IngredientSlotSelected { get; set; }
    [SerializeField] private SlotInputData inputData;
    [SerializeField] private PlayerIngredientsSlots playerIngredientsSlots;
    [SerializeField] private PlayerStation cookStation;

    private void Update()
    {
        PlayerIngredientSlotInputUpdate();
        PlayerStationInputUpdate();
    }

    private void PlayerIngredientSlotInputUpdate()
    {
        for(int i = 0; i < inputData.ingredientSlotInputs.Count; ++i)
        {
            if(Input.GetKeyDown(inputData.ingredientSlotInputs[i]))
            {
                SelectIngredientSlot(playerIngredientsSlots.GetSlotAtIndex(i));
            }
        }
        for(int i = 0; i < inputData.ingredientSlotPadNumInputs.Count; ++i)
        {
            if(Input.GetKeyDown(inputData.ingredientSlotPadNumInputs[i]))
            {
                SelectIngredientSlot(playerIngredientsSlots.GetSlotAtIndex(i));
            }
        }
    }

    private void SelectIngredientSlot(IngredientSlot newSlot)
    {
        if(newSlot.IsEmpty)
            return;
        if(IngredientSlotSelected != null)
        {
            IngredientSlotSelected.PutIngredientOnSlot();
        }
        IngredientSlotSelected = newSlot;
        IngredientSlotSelected.SelectIngredient();
    }

    private void PlayerStationInputUpdate()
    {
        if(Input.GetKeyDown(inputData.cookStationInput))
        {
            UpdateStation(cookStation);
        }
        //AJOUTER LES AUTRES STATIONS ICI
    }

    private void UpdateStation(PlayerStation station)
    {
        if(IngredientSlotSelected)
        {
            if(station.IsEmptySlot)
            {
                station.AddIngredientToStation(IngredientSlotSelected.IngredientOnSlot);
                IngredientSlotSelected.IngredientOnSlot=null;
                IngredientSlotSelected = null;
            }
            else
            {
                IngredientSlotSelected.PutIngredientOnSlot();
                IngredientSlotSelected = null;
            }
        }
        else
        {
            playerIngredientsSlots.FillSlots(station.StationIngredients);
            station.RemoveIngredientReferences();
        }
    }
}
