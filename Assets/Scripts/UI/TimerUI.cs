using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TimerUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Image sliderChrono;

    private void Start()
    {
        Timer.Instance.OnStartTurn += OnStartTurn;
    }

    private void Update()
    {
        textUI.text = (Timer.Instance.time+0.5f).ToString("F0");
        sliderChrono.fillAmount = ((10f-Timer.Instance.time))/10f;
    }

    private void OnStartTurn()
    {
        sliderChrono.transform.parent.DOPunchScale(Vector3.one * 0.25f, 0.3f).SetEase(Ease.OutCirc);
    }
}
