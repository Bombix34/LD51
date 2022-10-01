using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
public class Ingredient: ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public List<Category> Categories { get; private set; }
    [field: SerializeField]
    public Rarity Rarity { get; private set; }
    [field: SerializeField]
    public bool IsEdible { get; private set; }
    [field: SerializeField]
    public int Score { get; private set; }
}
