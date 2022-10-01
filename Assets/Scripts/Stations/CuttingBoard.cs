using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CuttingBoard", menuName = "ScriptableObjects/Station/CuttingBoard", order = 1)]
public class CuttingBoard : Station
{
    public override void OnAddIngredient()
    {
        Craft();
    }
}
