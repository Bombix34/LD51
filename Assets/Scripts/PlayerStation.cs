using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerStation : MonoBehaviour
{
    [SerializeField] private List<IngredientSlot> stationSlots;
    
    [field: SerializeField]
    public StationScriptableObject StationSo { get; set; }

    void Start()
    {
        stationSlots = GetComponentsInChildren<IngredientSlot>().ToList();
        Timer.Instance.OnStartTurn += StationSo.HandleNewTurn;
        foreach (var slot in stationSlots)
        {
            slot.OnAddIngredient += StationSo.AddIngredient;
            slot.OnRemoveIngredient += StationSo.RemoveIngredient;
            StationSo.OnCleanStation += slot.CleanIngredient;
        }
    }

    public void AddIngredientToStation(Transform ingredient)
    {
        foreach(var slot in stationSlots)
        {
            if(slot.IngredientOnSlot==null)
            {
                slot.IngredientOnSlot = ingredient;
                slot.PutIngredientOnSlot();
                return;
            }
        }
    }

    public void RemoveIngredientReferences()
    {
        foreach(var slot in stationSlots)
        {
            slot.IngredientOnSlot = null;
        }
    }

    public bool IsEmptySlot
    {
        get 
        {
            return stationSlots.Find(x => x.IngredientOnSlot == null) != null;
        }
    }

    public List<Transform> StationIngredients
    {
        get
        {
            List<Transform> toReturn = new List<Transform>();
            foreach(var slot in stationSlots)
            {
                if(!slot.IsEmpty)
                {
                    toReturn.Add(slot.IngredientOnSlot);
                    Debug.Log(slot.IngredientOnSlot);
                }
            }
            return toReturn;
        }
    }
}
