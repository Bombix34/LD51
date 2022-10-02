using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CuttingBoard", menuName = "ScriptableObjects/Station/CuttingBoard", order = 1)]
public class CuttingBoard : StationScriptableObject
{
    public override bool CanAddIngredient() => !Ingredients.Any();

}
