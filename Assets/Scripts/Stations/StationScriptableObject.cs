using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Tools.Managers;

public abstract class StationScriptableObject : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; set; }
    [field: SerializeField]
    protected List<Ingredient> Ingredients { get; private set; }
    [field: SerializeField]
    protected List<Recipe> Recipes { get; set; }
    public Action OnCleanStation { get; internal set; }

    [SerializeField] private AudioFieldEnum audioOnCook;

    public virtual void OnEnable()
    {
        Ingredients = new List<Ingredient>();
    }

    public virtual void AddIngredient(Ingredient ingredient)
    {
        if (!CanAddIngredient())
        {
            return;
        }
        Ingredients.Add(ingredient);
        OnAddIngredient();
    }
    
    public void RemoveIngredient(Ingredient ingredient)
    {
        if (!CanRemoveIngredient())
        {
            return;
        }
        Ingredients.Remove(ingredient);
        OnAddIngredient();
    }

    public void CleanIngredients()
    {
        foreach (var ingredient in Ingredients)
        {
            ingredient.DestroyImmediate();
        }
        Ingredients.Clear();
        OnCleanStation?.Invoke();
    }

    public abstract bool CanAddIngredient();
    public virtual bool CanRemoveIngredient()
    {
        return true;
    }

    public virtual void OnAddIngredient() { }
    public virtual void OnRemoveIngredient() { }

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

    public virtual void Cook(Vector2 spawnPosition, bool playSound=true)
    {
        if (!Ingredients.Any())
        {
            return;
        }
        var recipe = GetBestRecipe();

        var ingredient = recipe?.IngredientProduced ?? Resources.Load<IngredientScriptableObject>("ScriptableObjects/QuestionnableDish");
        PlayerIngredientsSlots.Instance.GenerateIngredient(ingredient, spawnPosition);
        ScoreBoard.Instance.AddToUnlockedDishes(ingredient);

        if(playSound)
        {
            SoundManager.Instance.PlaySound(audioOnCook);
        }
        CleanIngredients();
    }

    public virtual void HandleNewTurn()
    {
        CleanIngredients();
    }
#if (UNITY_EDITOR)
    public void CleanRecipes()
    {
        Recipes.Clear();
    }
    public void AddRecipe(Recipe recipe)
    {
        Recipes.Add(recipe);
    }
#endif
}
