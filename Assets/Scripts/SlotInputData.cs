using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Inputs/Slot input data")]
public class SlotInputData : ScriptableObject
{
    public List<KeyCode> ingredientSlotInputs;
    public List<KeyCode> ingredientSlotPadNumInputs;

    public KeyCode cutStationInput;
    public KeyCode cookStationInput;
    public KeyCode mixStationInput;
    public KeyCode frigdeStationInput;
    public KeyCode deliveryStationInput;

    public KeyCode useStationInput;
}

public enum InputType
{
    ingredientSlot1,
    ingredientSlot2,
    ingredientSlot3,
    ingredientSlot4,
    ingredientSlot5,
    ingredientSlot6,
    cutStation,
    cookStation,
    mixStation,
    fridgeStation,
    deliveryStation
}
