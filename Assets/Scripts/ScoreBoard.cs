using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class ScoreBoard : Singleton<ScoreBoard>
{
    public int Score { get; set; } = 0;

    private const string INGREDIENTS_PATH = "Assets/ScriptableObjects/Ingredients/";
    public List<IngredientScriptableObject> UnlockedIngredients = new List<IngredientScriptableObject>();
    public List<StationScriptableObject> UnlockedStations = new List<StationScriptableObject>();
    public int IngredientDrawSize { get; private set; }

    public List<ScoreLevel> ScoreLevels { get; set; } = new List<ScoreLevel>
    {
        new ScoreLevel(50, new List<string>() { "Patate" }, new List<string>() { "Fridge" }, 2),
    };

    public ScoreLevel NextScoreLevel { get; private set; }

    private List<IngredientScriptableObject> newlyAddedIngredients = new List<IngredientScriptableObject>();

    void Start()
    {
        UnlockIngredient("Tomate crue");
        UnlockStation("CuttingBoard");
        IngredientDrawSize = 1;
    }

    public void AddScore(int score)
    {
        if(score <= 0)
        {
            TakeDamage();
            return;
        }

        var newScore = Score + score;
        while (newScore >= NextScoreLevel.Score)
        {
            foreach (var ingredientName in NextScoreLevel.IngredientsName)
            {
                UnlockIngredient(ingredientName);
            }
            
            foreach (var stationName in NextScoreLevel.StationsNames)
            {
                UnlockStation(stationName);
            }
            
            NextScoreLevel = ScoreLevels[ScoreLevels.IndexOf(NextScoreLevel) + 1];
        }
    }

    private void UnlockIngredient(string ingredientName)
    {
        var ingredient = AssetDatabase.LoadAssetAtPath<IngredientScriptableObject>(INGREDIENTS_PATH
            + ingredientName + ".asset");
        UnlockedIngredients.Add(ingredient);
        newlyAddedIngredients.Add(ingredient);
    }

    private void UnlockStation(string stationName)
    {

    }

    public List<IngredientScriptableObject> GetNewlyAdded()
    {
        var newlyAddedIngredientsToReturn = newlyAddedIngredients.ToList();

        newlyAddedIngredients.Clear();

        return newlyAddedIngredientsToReturn;
    }

    internal void TakeDamage()
    {
        //TODO LATER
    }
}

public class ScoreLevel
{
    public int Score { get; private set; }
    public List<string> IngredientsName { get; private set; }
    public List<string> StationsNames { get; private set; }
    public int IngredientDrawSize { get; }

    public ScoreLevel(int score, List<string> ingredientsName, List<string> stationsNames, int ingredientDrawSize)
    {
        Score = score;
        IngredientsName = ingredientsName;
        StationsNames = stationsNames;
        IngredientDrawSize = ingredientDrawSize;
    }

}

