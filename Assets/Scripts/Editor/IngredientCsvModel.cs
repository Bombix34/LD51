using CsvHelper.Configuration;

public class IngredientCsvModel
{
    public bool Done { get; set; }
    public string Nomdelingrédient { get; set; }
    public string Catégories { get; set; }
    public string Filler1 { get; set; }
    public string Filler2 { get; set; }
    public string Qualité { get; set; }
    public string Score { get; set; }
    public string Descriptionvisuelle { get; set; }
    public bool Surcestdedans { get; set; }
    public int? Nombredoccurencesdanslesrecettes { get; set; }
    public int? Nombredoccurencesdestagsdanslesrecettes { get; set; }
    public int? Filler3 { get; set; }
    public int? Filler4 { get; set; }
    public int? Totaloccurences { get; set; }
    public string Nombredingrédientsparqualité { get; set; }
    public string Filler5 { get; set; }
    public string Filler6 { get; set; }
    public string Filler7 { get; set; }
    public string Filler8 { get; set; }
}

public class IngredientCsvModelClassMap : ClassMap<IngredientCsvModel>
{
    public IngredientCsvModelClassMap()
    {
        Map(m => m.Done).Name("Done");
        Map(m => m.Nomdelingrédient).Name("Nom de l'ingrédient");
        Map(m => m.Catégories).Name("Catégorie(s)");
        Map(m => m.Filler1).Name("Filler1");
        Map(m => m.Filler2).Name("Filler2");
        Map(m => m.Qualité).Name("Qualité");
        Map(m => m.Score).Name("Score");
        Map(m => m.Descriptionvisuelle).Name("Description visuelle");
        Map(m => m.Surcestdedans).Name("Sur c'est dedans !");
        Map(m => m.Nombredoccurencesdanslesrecettes).Name("Nombre d'occurences dans les recettes");
        Map(m => m.Nombredoccurencesdestagsdanslesrecettes).Name("Nombre d'occurences des tags dans les recettes");
        Map(m => m.Filler3).Name("Filler3");
        Map(m => m.Filler4).Name("Filler4");
        Map(m => m.Totaloccurences).Name("Total occurences");
        Map(m => m.Nombredingrédientsparqualité).Name("Nombre d'ingrédients par qualité ");
        Map(m => m.Filler5).Name("Filler5");
        Map(m => m.Filler6).Name("Filler6");
        Map(m => m.Filler7).Name("Filler7");
        Map(m => m.Filler8).Name("Filler8");
    }
}
