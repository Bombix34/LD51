using UnityEngine;
using UnityEngine.UI;

public class CookingTimerUI : MonoBehaviour
{
    [field: SerializeField]
    public CookingStation CookingStation { get; private set; }
    [field: SerializeField]
    public Slider Slider { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CookingStation.CookingOngoing)
        {
            Slider.maxValue = CookingStation.CookingDuration;
            Slider.value = CookingStation.CookingTimePassed;

            Slider.gameObject.SetActive(true);
        }
        else
        {
            Slider.gameObject.SetActive(false);
        }
    }
}
