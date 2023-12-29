using System;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public event Action OnValueChanged;
    public int CurrentValue
    {
        get => currentIntValue_;
        set
        {
            if (currentIntValue_ != value)
            {
                currentIntValue_ = value;
                OnValueChanged?.Invoke();
            }
        }
    }

    private int currentIntValue_;
    private float currentValue_;
    private bool isPaused_;
    
    private void Update()
    {
        if (isPaused_)
        {
            return;
        }
        
        if (currentValue_ > 0f)
        {
            currentValue_ -= Time.deltaTime;
            CurrentValue = (int) currentValue_;
        }
    }

    public void SetTimer(float time)
    {
        currentValue_ = time;
        Pause(false);
    }

    public void Pause(bool value)
    {
        isPaused_ = value;
    }
    
    public void StopTimer()
    {
        Pause(true);
        currentValue_ = 0f;
    }
}
