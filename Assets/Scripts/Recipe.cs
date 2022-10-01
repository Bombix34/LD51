using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    [field: SerializeField]
    public List<Ingredient> StrictIngredients { get; set; }

    [field: SerializeField]
    public List<Category> InterchangeableIngredientsCategory { get; set; }

    [field: SerializeField]
    public Ingredient IngredientProduced { get; set; }
    public int IngredientsNecessaryCount => StrictIngredients.Count + InterchangeableIngredientsCategory.Count;

    /// <summary>
    /// CanBeDoneWith
    /// </summary>
    /// <param name="ingredients"></param>
    /// <returns>-1 si pas possible sinon le nombre représente le nombre d'interchangeable utilisé</returns>
    public int CanBeDoneWith(List<Ingredient> ingredients)
    {
        if(ingredients.Count != IngredientsNecessaryCount)
        {
            return -1;
        }

        if (!StrictIngredients.All(ingredient => ingredients.Contains(ingredient)))
        {
            return -1;
        }

        var ingredientsCopy = new List<Ingredient>(ingredients);

        foreach (var ingredient in StrictIngredients)
        {
            ingredientsCopy.Remove(ingredient);
        }

        if (!InterchangeableIngredientsCategory.Any())
        {
            return 0;
        }

        foreach (var category in InterchangeableIngredientsCategory)
        {
            var ingredient = ingredientsCopy.FirstOrDefault(ingredient => ingredient.Categories.Contains(category));
            if (ingredient == null)
            {
                return -1;
            }

            ingredientsCopy.Remove(ingredient);
        }

        return InterchangeableIngredientsCategory.Count;
    }

}
