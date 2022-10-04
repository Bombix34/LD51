using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] private RectTransform collectionRect;

    private void Start()
    {
        collectionRect.DOAnchorPosY(Screen.height, 0f);
        if(PlayerPrefs.HasKey("HIGH_SCORE"))
        {
            highScoreText.text = "HIGH SCORE : " + PlayerPrefs.GetInt("HIGH_SCORE").ToString();
        }
        else
        {
            highScoreText.gameObject.SetActive(false);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Collection()
    {
        SceneManager.LoadScene("CollectionScene");
    }

    public void DisplayCollection(bool isDisplayed)
    {
        if(isDisplayed)
            collectionRect.DOAnchorPosY(0f, 0.3f);
        else
            collectionRect.DOAnchorPosY(Screen.height, 0.3f);
            

    }
}
