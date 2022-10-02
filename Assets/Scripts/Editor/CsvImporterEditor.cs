using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using UnityEditor;
using UnityEngine;

public class CsvImporterEditor : EditorWindow
{
    private const string INGREDIENTS_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vSJz0yhsCZ_koHUICcono0vbQhEIOzLH3u44UatbJCtUqZFxnK8Vgqyw_f_jTIY2JcJNLwW7hnAMV89/pub?gid=599060883&single=true&output=csv";
    private const string RECIPES_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vSJz0yhsCZ_koHUICcono0vbQhEIOzLH3u44UatbJCtUqZFxnK8Vgqyw_f_jTIY2JcJNLwW7hnAMV89/pub?gid=517237446&single=true&output=csv";

    private const string PATH_TO_INGREDIENTS = "Assets/ScriptableObjects/Ingredients/";
    private const string PATH_TO_SPRITES = @"Assets\Sprites";

    [MenuItem("Window/CsvImporterEditor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CsvImporterEditor));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Import Ingredients CSV"))
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
        var ingredients = records.Where(q => q.NomIngredient != "").ToList();

        if (Directory.Exists(PATH_TO_INGREDIENTS))
        {
            Directory.Delete(PATH_TO_INGREDIENTS, true);
        }
        Directory.CreateDirectory(PATH_TO_INGREDIENTS);

        foreach (var ingredient in ingredients)
        {
            var ingredientScriptableObject = CreateInstance<IngredientScriptableObject>();

            ingredientScriptableObject.SetName(ingredient.NomIngredient);
            ingredientScriptableObject.SetCategories(ingredient.Categories.Split(';')
                .Where(category => category != "")
                .Select(category =>
                {
                    switch (category.Trim())
                    {
                        case "Legume":
                            return Category.VEGETABLE;
                        case "Tomate":
                        case "Tomates":
                            return Category.TOMATOES;
                        case "Oeuf":
                            return Category.EGG;
                        case "Viande":
                            return Category.MEAT;
                        case "Viande crue":
                            return Category.RAW_MEAT;
                        case "Viande cuite":
                            return Category.COOKED_MEAT;
                        case "Boeuf":
                            return Category.BEEF;
                        case "Poulet":
                            return Category.CHICKEN;
                        case "Patate":
                            return Category.POTATOES;
                        case "Poisson cru":
                            return Category.RAW_FISH;
                        case "Poisson":
                            return Category.FISH;
                        case "Poisson cuit":
                            return Category.COOKED_FISH;
                        case "Riz":
                            return Category.RICE;
                        default:
                            Debug.LogError($"Unknown Categori : {category}");
                            return Category.VEGETABLE;
                    }
                }).ToList());

            switch (ingredient.Qualite)
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
                case "Legendaire":
                    ingredientScriptableObject.SetQuality(Rarity.LEGENDARY);
                    break;
                default:
                    Debug.LogWarning($"Unknown quality : {ingredient.Qualite}");
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

            ingredientScriptableObject.SetSprite(AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(PATH_TO_SPRITES, $"{ingredientScriptableObject.Name}.png")));
            if (ingredientScriptableObject.Sprite == null)
            {
                Debug.LogWarning($"No sprite found for {ingredientScriptableObject.Name}");
                ingredientScriptableObject.SetSprite(AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(PATH_TO_SPRITES, $"NO_TEXTURE.png")));
            }


            AssetDatabase.CreateAsset(ingredientScriptableObject, Path.Combine(PATH_TO_INGREDIENTS, $"{ingredientScriptableObject.Name}.asset"));
        }

    }

    public void ImportRecettesCsv()
    {
        
    }
}