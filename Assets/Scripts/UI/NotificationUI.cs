using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private GameObject notificationPrefab;

    private int currentDisplayedNotif = 0;

    private void Start()
    {
        ScoreBoard.Instance.OnUnlockNewDish += (ingredient) => DisplayNotification(ingredient.Sprite);
    }

    public void DisplayNotification(Sprite ingredientLogo)
    {
        GameObject newNotif = Instantiate(notificationPrefab, transform);
        newNotif.transform.parent = this.transform;
        newNotif.GetComponent<RectTransform>().DOAnchorPosY(Screen.height, 0f);
        newNotif.transform.GetChild(1).GetComponent<Image>().sprite = ingredientLogo;
        StartCoroutine(DisplayNotificationCoroutine(newNotif.GetComponent<RectTransform>()));
    }

    private IEnumerator DisplayNotificationCoroutine(RectTransform notificationRect)
    {
        currentDisplayedNotif++;
        notificationRect.DOAnchorPosY(0f-(notificationRect.rect.height*(currentDisplayedNotif-1)), 0.75f);
        yield return new WaitForSeconds(4f);
        notificationRect.DOAnchorPosY(Screen.height, 1f);
        currentDisplayedNotif = Mathf.Max(0, currentDisplayedNotif-1);
    }
}
