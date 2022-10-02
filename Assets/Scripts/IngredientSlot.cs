using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class IngredientSlot : MonoBehaviour
{
    [SerializeField] private Transform ingredientOnSlot;

    public Action<Ingredient> OnAddIngredient { get; internal set; }
    public Action<Ingredient> OnRemoveIngredient { get; internal set; }

    private void Start()
    {
        PutIngredientOnSlot();
    }

    public void PutIngredientOnSlot()
    {
        if(ingredientOnSlot==null)
            return;
        ingredientOnSlot.DOScale(1f, 0.2f);
        ingredientOnSlot?.DOMove(transform.position, 0.3f).SetEase(Ease.InOutCirc)
            .OnComplete(()=> ingredientOnSlot.GetComponent<SpriteRenderer>().sortingOrder = 0);
    }

    public void SelectIngredient()
    {
        if(ingredientOnSlot==null)
            return;
        ingredientOnSlot.GetComponent<SpriteRenderer>().sortingOrder = 5;
        ingredientOnSlot.DOScale(1.2f, 0.2f);
    }

    public Transform IngredientOnSlot 
    {
        set
        {
            if(value == null)
            {
                OnRemoveIngredient?.Invoke(ingredientOnSlot?.GetComponent<Ingredient>());
            }
            else
            {
                OnAddIngredient?.Invoke(value.GetComponent<Ingredient>());
            }
            ingredientOnSlot = value; 
        }
        get => ingredientOnSlot;
    }

    public bool IsEmpty => ingredientOnSlot == null;

    public void CleanIngredient()
    {
        ingredientOnSlot = null;
    }
}
