using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreUI : MonoBehaviour
{
    private ScoreBoard scoreBoard;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Image sliderFill;
    private Color sliderColor;

    private bool isUpdatingScoreBar = false;

    private void Start()
    {
        scoreBoard = ScoreBoard.Instance;
        sliderColor = sliderFill.color;

        scoreBoard.OnAddScore += UpdateSlider;
        scoreBoard.OnReachingScoreStep += ResetSlider;
    }

    private void UpdateSlider(int scoreAdded)
    {
        isUpdatingScoreBar = true;
        scoreSlider.DOValue(scoreBoard.Score, 0.3f).SetEase(Ease.InOutElastic)
            .OnComplete(()=> isUpdatingScoreBar = false);
        sliderFill.color = Color.white;
        sliderFill.DOColor(sliderColor, 0.2f);
    }

    private void ResetSlider(ScoreLevel nextScoreLevel)
    {
        StartCoroutine(ResetSliderCoroutine(nextScoreLevel));
    }

    private IEnumerator ResetSliderCoroutine(ScoreLevel nextScoreLevel)
    {
        while(isUpdatingScoreBar)
            yield return new WaitForSeconds(0.1f);
        scoreSlider.transform.DOPunchScale(Vector3.one*0.1f, 0.5f).SetEase(Ease.OutCirc);
        scoreSlider.DOValue(0f, 0.3f)
            .OnComplete(()=>
            {
                scoreSlider.maxValue = nextScoreLevel.Score;
                scoreSlider.minValue = scoreBoard.Score;
            });
    }
}
