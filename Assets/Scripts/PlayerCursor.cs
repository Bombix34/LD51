using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public IngredientSlot IngredientDraggedSlot { get; set; }
    [SerializeField] private LayerMask layerMask;

    private void Start()
    {
        Timer.Instance.OnStartTurn += ResetDraggedObject;
    }

    private void Update()
    {
        //drag n drop state
        if(IngredientDraggedSlot != null)
        {
            if(IngredientDraggedSlot.IngredientOnSlot == null)
                return;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Transform ingredientOnSlot = IngredientDraggedSlot.IngredientOnSlot;
            ingredientOnSlot.transform.position = Vector3.Lerp(ingredientOnSlot.position, mousePosition, 0.05f);
            if(Input.GetMouseButtonUp(0))
            {
                DropIngredient();
            }
        }
        //navigation state
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                TryClickOnIngredientSlot();
            }
        }
    }

    private void TryClickOnIngredientSlot()
    {
        GameObject slotClicked = TryFindInteractableElement();
        if(slotClicked == null)
        {
            return;
        }
        if(slotClicked.GetComponent<IngredientSlot>().IsEmpty)
            return;

        if(!slotClicked.transform.parent.GetComponent<PlayerStation>()?.StationSo.CanRemoveIngredient() ?? false)
        {
            return;
        }
        IngredientDraggedSlot = slotClicked.GetComponent<IngredientSlot>();
        IngredientDraggedSlot.SelectIngredient();
    }

    private void DropIngredient()
    {
        GameObject slotClicked = TryFindInteractableElement();
        if(slotClicked == null)
        {
            IngredientDraggedSlot.PutIngredientOnSlot();
            IngredientDraggedSlot = null;
        }
        else
        {
            IngredientSlot currentSlot = slotClicked.GetComponent<IngredientSlot>();
            PlayerStation currentSlotStation = currentSlot.transform.parent.GetComponent<PlayerStation>();

            if(currentSlotStation != null && (currentSlotStation.StationSo.Name == "CookingStation" ) && (!currentSlotStation?.StationSo.CanAddIngredient() ?? false))
            {
                IngredientDraggedSlot.PutIngredientOnSlot();
                IngredientDraggedSlot = null;
                return;
            }

            if(currentSlot.IngredientOnSlot != null)
            {
                var ingredientOnTarget = currentSlot.IngredientOnSlot;
                var currentIngredientDragged = IngredientDraggedSlot.IngredientOnSlot;
                currentSlot.IngredientOnSlot = null;
                IngredientDraggedSlot.IngredientOnSlot = null;
                currentSlot.IngredientOnSlot = currentIngredientDragged;
                IngredientDraggedSlot.IngredientOnSlot = ingredientOnTarget;
                IngredientDraggedSlot.PutIngredientOnSlot();
                currentSlot.PutIngredientOnSlot();
                IngredientDraggedSlot=null;
                return;
            }
            Transform ingredientOnSlot = IngredientDraggedSlot.IngredientOnSlot;
            IngredientDraggedSlot.IngredientOnSlot = null;
            IngredientDraggedSlot = null;
            currentSlot.IngredientOnSlot = ingredientOnSlot;
            currentSlot.PutIngredientOnSlot();
        }
    }

    public GameObject TryFindInteractableElement()
    {
        GameObject toReturn = null;
        RaycastHit2D hitData = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,Mathf.Infinity, layerMask);
        if(hitData.collider != null)
        {
            toReturn = hitData.collider.gameObject;
        }
        return toReturn;
    }

    private void ResetDraggedObject()
    {
        if(IngredientDraggedSlot != null && !IngredientDraggedSlot.IsEmpty)
        {
            IngredientDraggedSlot.PutIngredientOnSlot();
        }
        IngredientDraggedSlot = null;
    }
}
