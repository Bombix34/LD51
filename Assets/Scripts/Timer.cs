using System;
using UnityEngine;
using Tools.Managers;

public class Timer : Singleton<Timer>
{
    private const int TURN_DURATION = 10;

    public int Level { get; private set; } = 0;
    
    public float time = TURN_DURATION;
    public Action OnStartTurn { get; set; }
    
    void Start()
    {
        StartTurn(false);
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            StartTurn();
        }
    }

    private void StartTurn(bool playSound=true)
    {
        time = TURN_DURATION;
        ++Level;
        if(playSound)
            SoundManager.Instance.PlaySound(AudioFieldEnum.SFX08_END_LOOP);
        SoundManager.Instance.Launch();
        OnStartTurn?.Invoke();
    }
}
