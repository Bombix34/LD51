using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
public class IngredientScriptableObject: ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public List<Category> Categories { get; private set; }
    [field: SerializeField]
    public Rarity Rarity { get; private set; }
    [field: SerializeField]
    public int Score { get; private set; }
    [field: SerializeField]
    public Sprite Sprite { get; private set; }

#if UNITY_EDITOR

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetCategories(List<Category> categories)
    {
        Categories = categories;
    }

    public void SetQuality(Rarity rarity)
    {
        Rarity = rarity;
    }

    public void SetScore(int score)
    {
        Score = score;
    }

    public void SetSprite(Sprite sprite)
    {
        Sprite = sprite;
    }

#endif
}
