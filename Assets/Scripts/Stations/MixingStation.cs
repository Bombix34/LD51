using UnityEngine;

[CreateAssetMenu(fileName = "MixingStation", menuName = "ScriptableObjects/Station/MixingStation", order = 1)]
public class MixingStation : Station
{
    public override bool CanAddIngredient() => Ingredients.Count < 3;
}
