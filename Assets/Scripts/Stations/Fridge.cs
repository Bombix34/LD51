using UnityEngine;

[CreateAssetMenu(fileName = "Fridge", menuName = "ScriptableObjects/Station/Fridge", order = 1)]
public class Fridge : Station
{
    public override bool CanAddIngredient() => Ingredients.Count < 1;

    public override void HandleNewTurn() { }
}
