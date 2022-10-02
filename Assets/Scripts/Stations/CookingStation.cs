using UnityEngine;

[CreateAssetMenu(fileName = "CookingStation", menuName = "ScriptableObjects/Station/CookingStation", order = 1)]
public class CookingStation : Station
{
    [field: SerializeField]
    public float CookingDuration { get; private set; } = 1f;
    private bool CookingOngoing { get; set; }
    public override bool CanAddIngredient()
    {
        return Ingredients.Count < 3 && !CookingOngoing;
    }
}