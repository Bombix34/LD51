using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public IngredientSlot IngredientSlotSelected { get; set; }
    [SerializeField] private SlotInputData inputData;
    [SerializeField] private PlayerIngredientsSlots playerIngredientsSlots;
    [SerializeField] private PlayerStation cookStation;
    [SerializeField] private PlayerStation cutStation;

    public bool IsTryingToUseStation = false;

    private void Update()
    {
        PlayerIngredientSlotInputUpdate();
        PlayerStationInputUpdate();
        IsTryingToUseStation = Input.GetKey(inputData.useStationInput);
        Debug.Log(IsTryingToUseStation);
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
            if(IsTryingToUseStation)
            {
                cookStation.StationSo.Craft();
            }
            else
            {
                UpdateStation(cookStation);
            }
        }
        if(Input.GetKeyDown(inputData.cutStationInput))
        {
            if(IsTryingToUseStation)
            {
                cutStation.StationSo.Craft();
            }
            else
            {
                UpdateStation(cutStation);
            }
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
