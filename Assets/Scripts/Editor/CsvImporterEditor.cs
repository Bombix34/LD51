using CsvHelper;
using CsvHelper.Configuration;
using DG.Tweening.Plugins.Core.PathCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using UnityEditor;
using UnityEngine;
using Path = System.IO.Path;

public class CsvImporterEditor : EditorWindow
{
    private const string INGREDIENTS_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vSJz0yhsCZ_koHUICcono0vbQhEIOzLH3u44UatbJCtUqZFxnK8Vgqyw_f_jTIY2JcJNLwW7hnAMV89/pub?gid=599060883&single=true&output=csv";
    private const string RECIPES_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vSJz0yhsCZ_koHUICcono0vbQhEIOzLH3u44UatbJCtUqZFxnK8Vgqyw_f_jTIY2JcJNLwW7hnAMV89/pub?gid=517237446&single=true&output=csv";

    private const string PATH_TO_INGREDIENTS = "Assets/ScriptableObjects/Ingredients/";
    private const string PATH_TO_RECIPES = "Assets/ScriptableObjects/Recipes/";
    private const string PATH_TO_STATIONS = "Assets/ScriptableObjects/Stations/";
    private const string PATH_TO_SPRITES = "Assets/Sprites";
    

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
        if (GUILayout.Button("Import Recettes CSV"))
        {
            ImportRecipesCsv();
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

        var sprites = AssetDatabase.LoadAllAssetsAtPath(Path.Combine(PATH_TO_SPRITES, $"Ingredients.png")).OfType<Sprite>().OrderBy(q => q.name).ToList();

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
                //ingredientScriptableObject.SetScore(score);
                ingredientScriptableObject.SetScore(50);
            }
            else
            {
                ingredientScriptableObject.SetScore(-1);
            }

            try
            {
                ingredientScriptableObject.SetSprite(sprites.Where(q => q.name == $"Ingredients_{ingredient.PositionDessin}").Single());
            }
            catch (System.Exception)
            {

            }
            finally
            {
                if (ingredientScriptableObject.Sprite == null)
                {
                    Debug.LogWarning($"No sprite found for {ingredientScriptableObject.Name} at position {ingredient.PositionDessin}");
                    ingredientScriptableObject.SetSprite(AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(PATH_TO_SPRITES, "NO_TEXTURE.png")));
                }
            }



            AssetDatabase.CreateAsset(ingredientScriptableObject, Path.Combine(PATH_TO_INGREDIENTS, $"{ingredientScriptableObject.Name}.asset"));
        }

    }

    public void ImportRecipesCsv()
    {
        var httpClient = new HttpClient();
        var responseStream = httpClient.GetStreamAsync(RECIPES_URL).Result;

        using var reader = new StreamReader(responseStream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        };
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<RecetteCsvModelClassMap>();
        var records = csv.GetRecords<RecetteCsvModel>();
        var recipes = records.Where(q => q.Station != "").ToList();

        if (Directory.Exists(PATH_TO_RECIPES))
        {
            Directory.Delete(PATH_TO_RECIPES, true);
        }
        Directory.CreateDirectory(PATH_TO_RECIPES);

        string[] ingredientsFiles = Directory.GetFiles(PATH_TO_INGREDIENTS, "*.asset", SearchOption.AllDirectories);

        var incredientScriptableObjects = new List<IngredientScriptableObject>();
        foreach (var file in ingredientsFiles)
        {
            incredientScriptableObjects.Add(AssetDatabase.LoadAssetAtPath<IngredientScriptableObject>(file));
        }

        string[] stationsFiles = Directory.GetFiles(PATH_TO_STATIONS, "*.asset", SearchOption.AllDirectories);
        var StationScriptableObjects = new List<StationScriptableObject>();
        foreach (var file in stationsFiles)
        {
            var station = AssetDatabase.LoadAssetAtPath<StationScriptableObject>(file);
            station.CleanRecipes();
            StationScriptableObjects.Add(station);
        }

        foreach (var recipe in recipes)
        {
            var recipeScriptableObject = CreateInstance<Recipe>();
            recipeScriptableObject.IngredientProduced = incredientScriptableObjects.Single(q => q.Name == recipe.IngredientProduit.Trim());
            var ingredientsMandatories = recipe.IngredientMandatories;
            foreach (var ingredientInfo in ingredientsMandatories)
            {
                var ingredient = incredientScriptableObjects.SingleOrDefault(q => q.Name == ingredientInfo.Name.Trim());
                if (ingredient != null)
                {
                    recipeScriptableObject.StrictIngredients.Add(ingredient);
                    continue;
                }

                switch (ingredientInfo.Name)
                {
                    case ("Legume"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.VEGETABLE);
                        break;
                    case ("Tomate"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.TOMATOES);
                        break;
                    case ("Oeuf"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.EGG);
                        break;
                    case ("Viande"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.MEAT);
                        break;
                    case ("Viande crue"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.RAW_MEAT);
                        break;
                    case ("Viande cuite"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.COOKED_MEAT);
                        break;
                    case ("Boeuf"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.BEEF);
                        break;
                    case ("Poulet"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.CHICKEN);
                        break;
                    case ("Patate"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.POTATOES);
                        break;
                    case ("Poisson cru"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.RAW_FISH);
                        break;
                    case ("Poisson"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.FISH);
                        break;
                    case ("Poisson cuit"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.COOKED_FISH);
                        break;
                    case ("Riz"):
                        recipeScriptableObject.InterchangeableIngredientsCategory.Add(Category.RICE);
                        break;
                    default:
                        Debug.Log("Unsuported category : " + ingredientInfo.Name);
                        break;
                }
            }

            AssetDatabase.CreateAsset(recipeScriptableObject, Path.Combine(PATH_TO_RECIPES, $"{recipeScriptableObject.IngredientProduced.Name}.asset"));
            switch (recipe.Station)
            {
                case ("Decoupage"):
                    StationScriptableObjects.Single(q => q.Name == "CuttingBoard").AddRecipe(recipeScriptableObject);
                    break;
                case ("Cuisson"):
                    StationScriptableObjects.Single(q => q.Name == "CookingStation").AddRecipe(recipeScriptableObject);
                    break;
                case ("Melange"):
                    StationScriptableObjects.Single(q => q.Name == "MixingStation").AddRecipe(recipeScriptableObject);
                    break;
                default:
                    Debug.Log("Unsuported station : " + recipe.Station);
                    break;
            }
        }
    }
}