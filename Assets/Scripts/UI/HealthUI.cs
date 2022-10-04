using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    private int healthLose=0;

    [SerializeField] private List<GameObject> heartContainers;

    private void OnEnable()
    {
        ScoreBoard.Instance.OnPlayerLoseLife += OnPlayerTakeDamage;
    }

    private void OnDisable()
    {
        if(ScoreBoard.Instance == null)
            return;
        ScoreBoard.Instance.OnPlayerLoseLife -= OnPlayerTakeDamage;
    }

    private void OnPlayerTakeDamage()
    {
        if(heartContainers.Count-1-healthLose < 0)
            return;
        heartContainers[heartContainers.Count-1-healthLose].transform.DOScale(0f,0.3f).SetEase(Ease.InBounce);
        ++healthLose;
        if(healthLose == 2)
            heartContainers[0].GetComponent<Animator>().SetTrigger("Anim");
    }
}
