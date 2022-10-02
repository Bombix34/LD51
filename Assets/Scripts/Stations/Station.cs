using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Station : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; set; }
    [field: SerializeField]
    protected List<Ingredient> Ingredients { get; private set; }
    [field: SerializeField]
    protected List<Recipe> Recipes { get; set; }

    public void AddIngredient(Ingredient ingredient)
    {
        Ingredients.Add(ingredient);
        OnAddIngredient();
    }

    public void CleanIngredients()
    {
        foreach (var ingredient in Ingredients)
        {
            DestroyImmediate(ingredient);
        }
        Ingredients.Clear();
    }

    public abstract bool CanAddIngredient();

    public virtual void OnAddIngredient() { }

    private List<RecipeWithPriority> GetCraftableRecipes()
    {
        var recipesWithPriority = new List<RecipeWithPriority>();
        foreach (var recipe in Recipes)
        {
            var recipePriority = recipe.CanBeDoneWith(Ingredients.Select(q => q.IngredientSo).ToList());
            if (recipePriority == -1)
            {
                continue;
            }

            recipesWithPriority.Add((recipe, recipePriority));
        }
        
#if (UNITY_EDITOR)
        if (recipesWithPriority.Count > 1)
        {
            var duplicateRecipesList = recipesWithPriority.GroupBy(q => q.priority).Where(g => g.Count() > 1).Select(g => g.ToList()).ToList();
            foreach (var duplicateRecipes in duplicateRecipesList)
            {
                Debug.LogWarning($"Station {Name} has duplicate recipes : {string.Join(", ", duplicateRecipes.Select(recipeWithPriority => recipeWithPriority.recipe.IngredientProduced.Name))}");
            }
        }
#endif

        return recipesWithPriority;
    }

    [Button("GetBestRecipe")]
    private Recipe GetBestRecipe()
    {
        var recipesWithPriority = GetCraftableRecipes();
        
        if (recipesWithPriority.Count == 0)
        {
#if (UNITY_EDITOR)
            Debug.LogWarning($"Station {Name} has no best recipe");
#endif
            return null;
        }
        
        RecipeWithPriority? bestRecipesWithPriority = null;
        foreach (var recipeWithPriority in recipesWithPriority)
        {
            if (bestRecipesWithPriority == null || recipeWithPriority.priority < bestRecipesWithPriority?.priority)
            {
                bestRecipesWithPriority = recipeWithPriority;
            }
        }

#if (UNITY_EDITOR)
        Debug.Log($"Station {Name} has best recipe {bestRecipesWithPriority.Value.recipe.IngredientProduced.Name}");
#endif
        return bestRecipesWithPriority?.recipe;
    }

    public void Craft()
    {
        var recipe = GetBestRecipe();

        //TODO GENERATE ITEM

        CleanIngredients();
    }

    public virtual void HandleNewTurn()
    {
        CleanIngredients();
    }
}
