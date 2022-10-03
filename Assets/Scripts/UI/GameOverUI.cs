using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private RectTransform gameOverRect;
    [SerializeField] private TextMeshProUGUI textScore;

    private void Start()
    {
        gameOverRect.DOAnchorPosY(Screen.height, 0f);
        ScoreBoard.Instance.OnGameOver += DisplayGameOver;
    }

    private void DisplayGameOver(int finalScore)
    {
        gameOverRect.DOAnchorPosY(0f, 0.3f).SetEase(Ease.InOutCirc);
        textScore.text = finalScore.ToString();
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenuScene");
    }
}
