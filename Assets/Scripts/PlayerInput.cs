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
                cookStation.StationSo.Cook(cookStation.transform.position);
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
                cutStation.StationSo.Cook(cutStation.transform.position);
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
                mixStation.StationSo.Cook(mixStation.transform.position);
            }
            else
            {
                UpdateStation(mixStation);
            }
        }
        if(Input.GetKeyDown(inputData.deliveryStationInput))
        {
            if(IsTryingToUseStation)
            {
                //deliveryStation.StationSo.Craft(deliveryStation.transform.position);
            }
            else
            {
                UpdateStation(deliveryStation);
            }
        }
        if(Input.GetKeyDown(inputData.frigdeStationInput))
        {
            if(IsTryingToUseStation)
            {
              //  fridgeStation.StationSo.Craft(fridgeStation.transform.position);
            }
            else
            {
                UpdateStation(fridgeStation);
            }
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
            if (station.IsEmptySlot && station.StationSo.CanAddIngredient())
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
