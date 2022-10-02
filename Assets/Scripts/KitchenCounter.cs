using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class KitchenCounter : Singleton<KitchenCounter>
{
    [field: SerializeField]
    public List<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();
    private IngredientDraw ingredientDraw = new IngredientDraw();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button("Draw")]
    public void NewIngredientsDraw()
    {
        foreach (var ingredient in Ingredients)
        {
            DestroyImmediate(ingredient.gameObject);
        }
        Ingredients.Clear();
        var ingredientsDraw = ingredientDraw.DrawIngredients();
        foreach (var ingredient in ingredientsDraw)
        {
            GenerateIngredient(ingredient);
        }
    }

    public Ingredient GenerateIngredient(IngredientScriptableObject ingredientSo)
    {
        var ingredient = new GameObject(ingredientSo.Name);
        ingredient.transform.SetParent(transform);
        var ingredientComponent = ingredient.AddComponent<Ingredient>();
        ingredientComponent.IngredientSo = ingredientSo;
        Ingredients.Add(ingredientComponent);
        return ingredientComponent;
    }
}
