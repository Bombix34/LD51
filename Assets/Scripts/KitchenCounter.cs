using System.Collections.Generic;
using UnityEngine;

public class KitchenCounter : MonoBehaviour
{
    public List<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateIngredient(IngredientScriptableObject ingredientSo)
    {
        var ingredient = new GameObject(ingredientSo.Name);
        ingredient.transform.SetParent(transform);
        var ingredientComponent = ingredient.AddComponent<Ingredient>();
        ingredientComponent.IngredientSo = ingredientSo;
        Ingredients.Add(ingredientComponent);
    }
}
