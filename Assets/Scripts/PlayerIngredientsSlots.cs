using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerIngredientsSlots : Singleton<PlayerIngredientsSlots>
{
    [SerializeField] private List<IngredientSlot> slots;
    
    [field: SerializeField]
    public List<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();
    private readonly IngredientDraw _ingredientDraw = new IngredientDraw();
    [field: SerializeField]
    public GameObject DefaultIngredient { get; private set; }
    [field: SerializeField]
    public PlayerStation Fridge { get; set; }

    private void Start()
    {
        Timer.Instance.OnStartTurn += NewIngredientsDraw;
        foreach (var slot in slots)
        {
            slot.OnAddIngredient += AddIngredient;
            slot.OnRemoveIngredient += RemoveIngredient;
        }
    }

    private void AddIngredient(Ingredient ingredient)
    {
        Ingredients.Add(ingredient);
    }

    private void RemoveIngredient(Ingredient ingredient)
    {
        Ingredients.Remove(ingredient);
    }

    public void FillSlots(List<Transform> newIngredients)
    {
        StartCoroutine(FillSlotsCoroutine(newIngredients));
    }

    private IEnumerator FillSlotsCoroutine(List<Transform> newIngredients)
    {
        var slotsToRefresh = new List<IngredientSlot>();
        while (Fridge.gameObject.activeInHierarchy && Fridge.HasEmptySlot && newIngredients.Any())
        {
            IngredientSlot curSlot = Fridge.stationSlots.Find(x => x.IsEmpty);
            var newIngredient = newIngredients.First();
            newIngredients.Remove(newIngredient);
            curSlot.IngredientOnSlot = newIngredient;
            slotsToRefresh.Add(curSlot);
        }

        for (int i = 0; i < newIngredients.Count; ++i)
        {
            IngredientSlot curSlot = slots.Find(x => x.IsEmpty);
            curSlot.IngredientOnSlot = newIngredients[i];
            slotsToRefresh.Add(curSlot);
        }

        foreach(var slot in slotsToRefresh)
        {
            slot.PutIngredientOnSlot(false);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IngredientSlot GetSlotAtIndex(int index)
    {
        if(index >= slots.Count)
            return null;
        return slots[index];
    }

    [Button("Draw")]
    public void NewIngredientsDraw()
    {
        foreach (var ingredient in Ingredients)
        {
            ingredient.DestroyImmediate();
        }
        Ingredients.Clear();
        foreach (var slot in slots)
        {
            slot.CleanIngredient();
        }
        var ingredientsDraw = _ingredientDraw.DrawIngredients();
        var ingredientsTransform = new List<Transform>();

        foreach (var ingredient in ingredientsDraw)
        {
            ingredientsTransform.Add(GenerateIngredient(ingredient, new Vector2(0, -15)).transform);
        }
    }

    public Ingredient GenerateIngredient(IngredientScriptableObject ingredientSo, Vector2 spawnPosition)
    {
        var ingredient = Instantiate(DefaultIngredient, transform);
        ingredient.transform.SetParent(transform);
        ingredient.transform.position = spawnPosition;
        var ingredientComponent = ingredient.GetComponent<Ingredient>();
        ingredientComponent.IngredientSo = ingredientSo;
        ingredient.GetComponent<SpriteRenderer>().sprite = ingredientSo.Sprite;
        ingredient.GetComponentInChildren<SpriteOutline>().SetupSprite(ingredientSo.Sprite, ingredientSo.Rarity);
        FillSlots(new List<Transform> { ingredient.transform });
        return ingredientComponent;
    }
}
