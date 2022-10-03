using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Tools.Managers;

public class IngredientSlot : MonoBehaviour
{
    [SerializeField] private Transform ingredientOnSlot;

    public Action<Ingredient> OnAddIngredient { get; internal set; }
    public Action<Ingredient> OnRemoveIngredient { get; internal set; }

    public void PutIngredientOnSlot(bool playSound=true)
    {
        if(ingredientOnSlot==null)
            return;
        if(playSound)
            SoundManager.Instance.PlaySound(AudioFieldEnum.SFX07_DROP_INGREDIENT);
        ingredientOnSlot.DOScale(1f, 0.2f);
        ingredientOnSlot?.DOMove(transform.position, 0.3f).SetEase(Ease.InOutCirc)
            .OnComplete(()=> 
            {
                ingredientOnSlot.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            );
    }

    public void SelectIngredient()
    {
        if(ingredientOnSlot==null)
            return;
        SoundManager.Instance.PlaySound(AudioFieldEnum.SFX06_SELECT_INGREDIENT);
        ingredientOnSlot.GetComponent<SpriteRenderer>().sortingOrder = 5;
        ingredientOnSlot.DOScale(1.3f, 0.2f);
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
