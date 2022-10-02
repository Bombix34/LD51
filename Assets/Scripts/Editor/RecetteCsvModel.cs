using CsvHelper.Configuration;
using System;
using System.Collections.Generic;

public class RecetteCsvModel
{
    public string Station { get; set; }
    public string IngredientsString { get; set; }
    public string IngredientProduit { get; set; }
    public List<IngredientMandatory> IngredientMandatories => GetIngredientMandatories();

    private List<IngredientMandatory> GetIngredientMandatories()
    {
        var ingredientMandatories = new List<IngredientMandatory>();
        var ingredients = IngredientsString.Split(';');
        for (int i = 0; i < ingredients.Length; i += 2)
        {
            ingredientMandatories.Add(new IngredientMandatory(ingredients[i], ingredients[i + 1] == "TRUE"));
        }
        return ingredientMandatories;
    }
}


public struct IngredientMandatory
{
    public string Name { get; private set; }
    public bool Mendatory { get; private set; }
    public IngredientMandatory(string name, bool mendatory)
    {
        Name = name;
        Mendatory = mendatory;
    }

}

public class RecetteCsvModelClassMap : ClassMap<RecetteCsvModel>
{
    public RecetteCsvModelClassMap()
    {
        Map(m => m.Station).Name("Station");
        Map(m => m.IngredientsString).Name("Ingredients");
        Map(m => m.IngredientProduit).Name("Résultat");;
    }
}
