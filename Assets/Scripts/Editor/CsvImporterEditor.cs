using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using UnityEditor;
using UnityEngine;

public class CsvImporterEditor : EditorWindow
{
    private const string INGREDIENTS_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vSJz0yhsCZ_koHUICcono0vbQhEIOzLH3u44UatbJCtUqZFxnK8Vgqyw_f_jTIY2JcJNLwW7hnAMV89/pub?gid=1476279386&single=true&output=csv";

    private const string PATH_TO_INGREDIENTS = "Assets/ScriptableObjects/Ingredients/";

    [MenuItem("Window/CsvImporterEditor")]

    public static void ShowWindow()
    {
        GetWindow(typeof(CsvImporterEditor));
    }

    void OnGUI()
    {
        if(GUILayout.Button("Import Ingredients CSV"))
        {
            ImportIngredientsCsv();
        }
    }

    public void ImportIngredientsCsv()
    {
        var httpClient = new HttpClient();
        var responseStream = httpClient.GetStreamAsync(INGREDIENTS_URL).Result;
        
        using var reader = new StreamReader(responseStream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        };
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<IngredientCsvModelClassMap>();
        var records = csv.GetRecords<IngredientCsvModel>();
        var ingredients = records.Where(q => q.Nomdelingrédient != "").ToList();

        foreach (var ingredient in ingredients)
        {
            var ingredientScriptableObject = CreateInstance<Ingredient>();
            
            ingredientScriptableObject.SetName(ingredient.Nomdelingrédient);
            //ingredientScriptableObject.SetCategories(ingredient.Catégories.Split(',').Select(q => (Category)System.Enum.Parse(typeof(Category), q)).ToList());
            //TODO CHANGE LATER
            ingredientScriptableObject.SetCategories(new List<Category> { Category.VEGETABLE });
            
            switch (ingredient.Qualité)
            {
                case "Commun":
                    ingredientScriptableObject.SetQuality(Rarity.COMMON);
                    break;
                case "Atypique":
                    ingredientScriptableObject.SetQuality(Rarity.UNCOMMON);
                    break;
                case "Rare":
                    ingredientScriptableObject.SetQuality(Rarity.RARE);
                    break;
                case "Epique":
                    ingredientScriptableObject.SetQuality(Rarity.EPIC);
                    break;
                case "Légendaire":
                    ingredientScriptableObject.SetQuality(Rarity.LEGENDARY);
                    break;
                default:
                    Debug.LogWarning($"Unknown quality : {ingredient.Qualité}");
                    break;
            }
            
            if (int.TryParse(ingredient.Score, out var score))
            {
                ingredientScriptableObject.SetScore(score);
            }
            else
            {
                ingredientScriptableObject.SetScore(-1);
            }


            AssetDatabase.CreateAsset(ingredientScriptableObject, Path.Combine(PATH_TO_INGREDIENTS, $"{ingredientScriptableObject.Name}.asset"));
        }
        
    }
}