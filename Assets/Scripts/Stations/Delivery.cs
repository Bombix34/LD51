using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Delivery", menuName = "ScriptableObjects/Station/Delivery", order = 1)]
public class Delivery : StationScriptableObject
{
    public override void HandleNewTurn()
    {
        ScoreBoard.Instance.AddScore(Ingredients.FirstOrDefault()?.IngredientSo.Score ?? -1);

        base.HandleNewTurn();
    }

    public override bool CanAddIngredient() => !Ingredients.Any();
}
