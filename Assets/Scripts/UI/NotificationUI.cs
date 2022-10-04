using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private GameObject notificationPrefab;

    public Sprite testSprite;

    private void Start()
    {
        ScoreBoard.Instance.OnUnlockNewDish += (ingredient) => DisplayNotification(ingredient.Sprite);
    }

    public void DisplayNotification(Sprite ingredientLogo)
    {
        GameObject newNotif = Instantiate(notificationPrefab, transform);
        newNotif.transform.parent = this.transform;
        newNotif.GetComponent<RectTransform>().DOAnchorPosX(-Screen.width, 0f);
        newNotif.transform.GetChild(2).GetComponent<Image>().sprite = ingredientLogo;
        StartCoroutine(DisplayNotificationCoroutine(newNotif.GetComponent<RectTransform>()));
    }

    private IEnumerator DisplayNotificationCoroutine(RectTransform notificationRect)
    {
        //notificationRect.anchoredPosition = 
        notificationRect.DOAnchorPosX(0f, 0.75f);
        yield return new WaitForSeconds(4f);
        notificationRect.DOAnchorPosX(-Screen.width, 1f);
    }
}
