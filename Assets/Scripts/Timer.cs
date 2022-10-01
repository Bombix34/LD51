using System;
using UnityEngine;

public class Timer : Singleton<Timer>
{
    private const int TURN_DURATION = 10;
    
    public float time = TURN_DURATION;
    public Action OnStartTurn { get; set; }
    
    void Start()
    {
        StartTurn();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            StartTurn();
        }
    }

    private void StartTurn()
    {
        time = TURN_DURATION;
        OnStartTurn?.Invoke();
    }
}
