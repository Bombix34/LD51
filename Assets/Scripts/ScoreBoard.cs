using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using Tools.Managers;

public class ScoreBoard : Singleton<ScoreBoard>
{
    public int Score { get; set; } = 0;
    [SerializeField] private int life = 3;

    private const string INGREDIENTS_PATH = "ScriptableObjects/Ingredients/";
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

    public List<IngredientScriptableObject> UnlockedDishes { get; private set; } = new List<IngredientScriptableObject>();

    public ScoreLevel NextScoreLevel { get; private set; }
    public Action<IngredientScriptableObject> OnUnlockNewDish { get; set; }

    private readonly List<IngredientScriptableObject> newlyAddedIngredients = new List<IngredientScriptableObject>();

    public Action<int> OnAddScore;
    public Action<ScoreLevel> OnReachingScoreStep;
    public Action<int> OnGameOver;

    void Start()
    {
        UnlockIngredient("Tomate crue");
        UnlockStation("CuttingBoard");
        IngredientDrawSize = 1;
        NextScoreLevel = ScoreLevels.First();
    }

    public void AddScore(int score)
    {
        Debug.Log(Timer.Instance.Level);
        if(score <= 0 )
        {
            if(Timer.Instance.Level <= 1)
                return;
            SoundManager.Instance.PlaySound(AudioFieldEnum.SFX10_APRECIATION_BAD);
            TakeDamage();
            return;
        }
        if(score > 400)
            SoundManager.Instance.PlaySound(AudioFieldEnum.SFX11_APRECIATION_GOOD);
        else
            SoundManager.Instance.PlaySound(AudioFieldEnum.SFX12_APRECIATION_GREAT);
        Score += score;
        OnAddScore?.Invoke(score);
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
            OnReachingScoreStep?.Invoke(NextScoreLevel);
        }
    }

    private void UnlockIngredient(string ingredientName)
    {
        var ingredient = Resources.Load<IngredientScriptableObject>(Path.Combine(INGREDIENTS_PATH, ingredientName));
        UnlockedIngredients.Add(ingredient);
        newlyAddedIngredients.Add(ingredient);
        AddToUnlockedDishes(ingredient);
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
        --life;
        if(life <= 0)
        {
            RegisterHighScore();
            OnGameOver?.Invoke(Score);
        }
    }
    
    private void RegisterHighScore()
    {
        if(Score==0)
            return;
        if(PlayerPrefs.HasKey("HIGH_SCORE"))
        {
            int highScoreRegistered = PlayerPrefs.GetInt("HIGH_SCORE");
            if(Score > highScoreRegistered)
                PlayerPrefs.SetInt("HIGH_SCORE", Score);
        }
        else
        {
            PlayerPrefs.SetInt("HIGH_SCORE", Score);
        }
    }

    public void AddToUnlockedDishes(IngredientScriptableObject dish)
    {
        if (UnlockedDishes.Contains(dish))
        {
            return;
        }

        UnlockedDishes.Add(dish);

        if (PlayerPrefs.HasKey(dish.Name))
        {
            return;
        }
        PlayerPrefs.SetInt(dish.Name, default);
        SoundManager.Instance.PlaySound(AudioFieldEnum.SFX09_UNLOCK_INGREDIENT);
        OnUnlockNewDish?.Invoke(dish);
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

