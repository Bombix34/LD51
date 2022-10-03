using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private RectTransform notifRect;
    [SerializeField] private Image ingredientImage;

    private void Start()
    {
        notifRect.DOAnchorPosY(Screen.height, 0f);
        ScoreBoard.Instance.OnUnlockNewDish += (ingredient) => DisplayNotification(ingredient.Sprite);
    }

    public void DisplayNotification(Sprite ingredientLogo)
    {
        ingredientImage.sprite = ingredientLogo;
        StartCoroutine(DisplayNotificationCoroutine());
    }

    private IEnumerator DisplayNotificationCoroutine()
    {
        notifRect.DOAnchorPosY(0f, 0.75f);
        yield return new WaitForSeconds(4f);
        notifRect.DOAnchorPosY(Screen.height, 1f);
    }
}
