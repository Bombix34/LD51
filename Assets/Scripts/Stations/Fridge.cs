using UnityEngine;
using Tools.Managers;

[CreateAssetMenu(fileName = "Fridge", menuName = "ScriptableObjects/Station/Fridge", order = 1)]
public class Fridge : StationScriptableObject
{
    public override bool CanAddIngredient() => Ingredients.Count < 1;

    public override void HandleNewTurn() { }

    public override void AddIngredient(Ingredient ingredient)
    {
        SoundManager.Instance.PlaySound(AudioFieldEnum.SFX05_FRIDGE);
        base.AddIngredient(ingredient);
    }
}
