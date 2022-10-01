using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    private Text textUi;

    // Start is called before the first frame update
    void Start()
    {
        textUi = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textUi.text = Timer.Instance.time.ToString("F2");
    }
}
