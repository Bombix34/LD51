using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerIngredientsSlots : Singleton<PlayerIngredientsSlots>
{
    [SerializeField] private List<IngredientSlot> slots;
    
    [field: SerializeField]
    public List<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();
    private readonly IngredientDraw _ingredientDraw = new IngredientDraw();


    private void Start()
    {
        Timer.Instance.OnStartTurn += NewIngredientsDraw;
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

    [Button("Draw")]
    public void NewIngredientsDraw()
    {
        //foreach (var ingredient in Ingredients)
        //{
        //    ingredient.DestroyImmediate();
        //}
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
        var prefab = AssetDatabase.LoadAssetAtPath<Ingredient>(@"Assets\Prefabs\Ingredient.prefab");
        var ingredient = Instantiate(prefab, transform);
        ingredient.transform.SetParent(transform);
        ingredient.transform.position = spawnPosition;
        var ingredientComponent = ingredient.GetComponent<Ingredient>();
        ingredientComponent.IngredientSo = ingredientSo;
        ingredient.GetComponent<SpriteRenderer>().sprite = ingredientSo.Sprite;
        Ingredients.Add(ingredientComponent);
        FillSlots(new List<Transform> { ingredient.transform });
        return ingredientComponent;
    }
}
