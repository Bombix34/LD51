using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerStation : MonoBehaviour
{
    [SerializeField] private List<IngredientSlot> stationSlots;
    [SerializeField] private Button useStationButton;
    [SerializeField] private int nbMinimumToUseStation=1;
    
    [field: SerializeField]
    public StationScriptableObject StationSo { get; set; }

    private void Start()
    {
        useStationButton?.transform.DOScale(0f,0f);
        stationSlots = GetComponentsInChildren<IngredientSlot>().ToList();
        Timer.Instance.OnStartTurn += StationSo.HandleNewTurn;
        foreach (var slot in stationSlots)
        {
            slot.OnAddIngredient += StationSo.AddIngredient;
            slot.OnAddIngredient += DisplayButton;
            slot.OnRemoveIngredient += StationSo.RemoveIngredient;
            slot.OnRemoveIngredient += DisplayButton;
            StationSo.OnCleanStation += slot.CleanIngredient;
            StationSo.OnCleanStation += ResetButton;
        }
    }
    
    private void Update()
    {
        DisplayButton(null);
    }

    public void AddIngredientToStation(Transform ingredient)
    {
        foreach(var slot in stationSlots)
        {
            if(slot.IngredientOnSlot == null)
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
                }
            }
            return toReturn;
        }
    }

    private void DisplayButton(Ingredient ingredient)
    {
        if(useStationButton==null)
            return;
        if(StationIngredients.Count < nbMinimumToUseStation)
        {
            if(useStationButton.transform.localScale.x > 0f)
                useStationButton?.transform.DOScale(0f,0.3f);
        }
        else
        {
            if(useStationButton.transform.localScale.x < 0.1f)
                useStationButton?.transform.DOScale(0.1f,0.3f);
        }
    }

    private void ResetButton()
    {
        useStationButton?.transform.DOScale(0f ,0.3f);
    }
}
