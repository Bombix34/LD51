using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIngredientsSlots : Singleton<PlayerIngredientsSlots>
{
    [SerializeField] private List<IngredientSlot> slots;

    [SerializeField] private List<Transform> testIngredients;

    private void Start()
    {
        FillSlots(testIngredients);
    }

    public void FillSlots(List<Transform> newIngredients)
    {
        StartCoroutine(FillSlotsCoroutine(newIngredients));
    }

    private IEnumerator FillSlotsCoroutine(List<Transform> newIngredients)
    {
        for(int i = 0; i < newIngredients.Count; ++i)
        {
            IngredientSlot curSlot = slots.Find(x => x.IsEmpty);
            curSlot.IngredientOnSlot = newIngredients[i];
        }
        foreach(var slot in slots)
        {
            slot.PutIngredientOnSlot();
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IngredientSlot GetSlotAtIndex(int index)
    {
        if(index >= slots.Count)
            return null;
        return slots[index];
    }
}
