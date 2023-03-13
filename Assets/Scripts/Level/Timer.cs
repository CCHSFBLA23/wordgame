using UnityEngine;
using System;
using TMPro;
using System.Diagnostics;
using UnityEngine.Playables;

public class Timer : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text timerText;
    public Stopwatch _timer = new Stopwatch();
    private string _timerString;
    public LevelHandler levelHandler;
    TimeSpan timeElapsed;

    private void UpdateTimer()
    {
        timeElapsed = _timer.Elapsed;
        _timerString = timeElapsed.ToString(@"mm\:ss");
        timerText.text = _timerString;
        if (timerText.text == "99:59")
        {
            _timer.Stop();
            return;
        }
    }

    public void Reset()
    {
        _timer.Restart();
    }

    public string GetTimerText()
    {
        return _timerString;
    }
    public void Pause()
    {
        _timer.Stop();
    }

    public void Unpause()
    {
        _timer.Start();
    }

    public double GetTimerSeconds()
    {
        return timeElapsed.TotalSeconds;
    }
    
    private void Update()
    {
        if (levelHandler.solved)
        {
            _timer.Stop();
            return;
        }

        UpdateTimer();
    }

}
