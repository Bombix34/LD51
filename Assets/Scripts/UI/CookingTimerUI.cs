using UnityEngine;
using UnityEngine.UI;

public class CookingTimerUI : MonoBehaviour
{
    [field: SerializeField]
    public CookingStation CookingStation { get; private set; }
    [field: SerializeField]
    public Slider Slider { get; private set; }

    [SerializeField] private Button useButton;

    private void Update()
    {
        if (CookingStation.CookingOngoing)
        {
            Slider.maxValue = CookingStation.CookingDuration;
            Slider.value = CookingStation.CookingTimePassed;

            Slider.gameObject.SetActive(true);
            useButton.gameObject.SetActive(false);
        }
        else
        {
            Slider.gameObject.SetActive(false);
            useButton.gameObject.SetActive(true);
        }
    }
}
