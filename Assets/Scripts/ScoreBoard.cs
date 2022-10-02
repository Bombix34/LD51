using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ScoreBoard : Singleton<ScoreBoard>
{
    public int Score { get; set; } = 0;

    private const string INGREDIENTS_PATH = "Assets/ScriptableObjects/Ingredients/";
    public List<IngredientScriptableObject> UnlockedIngredients = new List<IngredientScriptableObject>();
    public List<StationScriptableObject> UnlockedStations = new List<StationScriptableObject>();
    [field: SerializeField]
    public List<GameObject> Stations { get; set; }
    public int IngredientDrawSize { get; private set; }

    public List<ScoreLevel> ScoreLevels { get; set; } = new List<ScoreLevel>
    {
        new ScoreLevel(50, new List<string>() { "Patate crue" }, new List<string>() { "Fridge" }, 2),
        new ScoreLevel(90, new List<string>() { "Oeuf cru" }, new List<string>(), 2),
        new ScoreLevel(100, new List<string>() { "Poulet cru" }, new List<string>() { "MixingStation" }, 4),
        new ScoreLevel(150, new List<string>() { "Boeuf cru" }, new List<string>() { "CookStation" }, 5),
    };

    public ScoreLevel NextScoreLevel { get; private set; }

    private readonly List<IngredientScriptableObject> newlyAddedIngredients = new List<IngredientScriptableObject>();

    void Start()
    {
        UnlockIngredient("Tomate crue");
        UnlockStation("CuttingBoard");
        IngredientDrawSize = 1;
        NextScoreLevel = ScoreLevels.First();
    }

    public void AddScore(int score)
    {
        if(score <= 0)
        {
            TakeDamage();
            return;
        }

        Score += score;
        while (NextScoreLevel != null && Score >= NextScoreLevel.Score)
        {
            
            foreach (var ingredientName in NextScoreLevel.IngredientsName)
            {
                UnlockIngredient(ingredientName);
            }
            
            foreach (var stationName in NextScoreLevel.StationsNames)
            {
                UnlockStation(stationName);
            }
            IngredientDrawSize = NextScoreLevel.IngredientDrawSize;

            var nextScoreLevelIndex = ScoreLevels.IndexOf(NextScoreLevel) + 1;
            if (ScoreLevels.Count <= nextScoreLevelIndex)
            {
                NextScoreLevel = null;
                return;
            }

            NextScoreLevel = ScoreLevels[nextScoreLevelIndex];
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
        Stations.Where(s => s.name == stationName).Single().SetActive(true);
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

