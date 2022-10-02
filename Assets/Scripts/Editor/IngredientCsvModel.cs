using CsvHelper.Configuration;

public class IngredientCsvModel
{
    public string NomIngredient { get; set; }
    public string Categories { get; set; }
    public string Qualite { get; set; }
    public string Score { get; set; }
    public string Station { get; set; }
    public string PositionDessin { get; set; }
}

public class IngredientCsvModelClassMap : ClassMap<IngredientCsvModel>
{
    public IngredientCsvModelClassMap()
    {
        Map(m => m.NomIngredient).Name("Nom d'ingredient");
        Map(m => m.Categories).Name("Categorie(s)");
        Map(m => m.Qualite).Name("Qualite");
        Map(m => m.Score).Name("Score");
        Map(m => m.Station).Name("Station");
        Map(m => m.PositionDessin).Name("PositionDessin");
    }
}
