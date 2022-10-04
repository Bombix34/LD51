using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowFeedback : MonoBehaviour
{
    [SerializeField] private GameObject arrowFeedbacks;

    private bool isDisplayed =false;

    private void Start()
    {
        arrowFeedbacks.transform.DOScale(0f,0f);
    }

    private void Update()
    {
        if(Timer.Instance.time <= 5f && !isDisplayed)
        {
            ShowArrows();
        }
    }

    private void OnEnable()
    {
        Timer.Instance.OnStartTurn += HideArrows;
    }

    private void OnDisable()
    {
        if(Timer.Instance != null)
            Timer.Instance.OnStartTurn -= HideArrows;
    }

    private void ShowArrows()
    {
        arrowFeedbacks.transform.DOScale(1f,0.3f);
        isDisplayed=true;
    }

    private void HideArrows()
    {
        arrowFeedbacks.transform.DOScale(0f,0.2f);
        isDisplayed=false;
    }
}
