using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PlayerInput : MonoBehaviour
{
    public IngredientSlot IngredientSlotSelected { get; set; }
    [SerializeField] private SlotInputData inputData;
    [SerializeField] private PlayerIngredientsSlots playerIngredientsSlots;
    [SerializeField] private PlayerStation cookStation;
    [SerializeField] private PlayerStation cutStation;
    [SerializeField] private PlayerStation mixStation;
    [SerializeField] private PlayerStation deliveryStation;
    [SerializeField] private PlayerStation fridgeStation;

    public static Action OnPlayerPressTab;
    public static Action<SlotInputData> OnPlayerSetInputData;

    public bool IsTryingToUseStation = false;

    private void Start()
    {
        OnPlayerSetInputData?.Invoke(inputData);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            OnPlayerPressTab?.Invoke();
        }
        PlayerIngredientSlotInputUpdate();
        PlayerStationInputUpdate();
        IsTryingToUseStation = Input.GetKey(inputData.useStationInput);
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
                cookStation.StationSo.Cook(cookStation.transform.position, true);
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
                cutStation.StationSo.Cook(cutStation.transform.position, true);
            }
            else
            {
                UpdateStation(cutStation);
            }
        }
        if(Input.GetKeyDown(inputData.mixStationInput))
        {
            if(IsTryingToUseStation)
            {
                mixStation.StationSo.Cook(mixStation.transform.position, true);
            }
            else
            {
                UpdateStation(mixStation);
            }
        }
        if(Input.GetKeyDown(inputData.deliveryStationInput))
        {
            UpdateStation(deliveryStation);
        }
        if(Input.GetKeyDown(inputData.frigdeStationInput))
        {
            UpdateStation(fridgeStation);
        }
    }

    public void PlayerStationUseButton(PlayerStation stationToUse)
    {
        stationToUse.StationSo.Cook(stationToUse.transform.position);
    }

    private void UpdateStation(PlayerStation station)
    {
        if(IngredientSlotSelected)
        {
            if (station.HasEmptySlot && station.StationSo.CanAddIngredient())
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
            if (station.StationSo.CanRemoveIngredient())
            {
                playerIngredientsSlots.FillSlots(station.StationIngredients, false);
                station.RemoveIngredientReferences();
            }
        }
    }
}
