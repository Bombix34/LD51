using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Station : MonoBehaviour
{
    [field: SerializeField]
    public StationScriptableObject StationSo { get; set; }
    private List<IngredientSlot> ingredientSlots { get; set; }

    void Start()
    {
        ingredientSlots = GetComponentsInChildren<IngredientSlot>().ToList();
        Timer.Instance.OnStartTurn += StationSo.HandleNewTurn;
        foreach (var slot in ingredientSlots)
        {
            slot.OnAddIngredient += StationSo.AddIngredient;
            slot.OnRemoveIngredient += StationSo.RemoveIngredient;
            StationSo.OnCleanStation += slot.CleanIngredient;
        }
    }
}
