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

    private void Start()
    {
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
}
