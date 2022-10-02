using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KitchenCounter : Singleton<KitchenCounter>
{
    [field: SerializeField]
    public List<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();
    private readonly IngredientDraw _ingredientDraw = new IngredientDraw();

    void Start()
    {
        Timer.Instance.OnStartTurn += NewIngredientsDraw;
    }

    [Button("Draw")]
    public void NewIngredientsDraw()
    {
        foreach (var ingredient in Ingredients)
        {
            ingredient.DestroyImmediate();
        }
        Ingredients.Clear();
        var ingredientsDraw = _ingredientDraw.DrawIngredients();
        var ingredientsTransform = new List<Transform>();
        
        foreach (var ingredient in ingredientsDraw)
        {
            ingredientsTransform.Add(GenerateIngredient(ingredient).transform);
        }
        //PlayerIngredientsSlots.Instance.FillSlots(ingredientsTransform);
    }

    public Ingredient GenerateIngredient(IngredientScriptableObject ingredientSo)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<Ingredient>(@"Assets\Prefabs\Ingredient.prefab");
        var ingredient = Instantiate(prefab, transform);
        ingredient.transform.SetParent(transform);
        var ingredientComponent = ingredient.GetComponent<Ingredient>();
        ingredientComponent.IngredientSo = ingredientSo;
        Ingredients.Add(ingredientComponent);
        PlayerIngredientsSlots.Instance.FillSlots(new List<Transform> { ingredient.transform });
        return ingredientComponent;
    }
}
