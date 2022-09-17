using UnityEngine;
using System;
using TMPro;
using System.Diagnostics;

public class Timer : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text timerText;
    private Stopwatch _timer = new Stopwatch();
    public LevelHandler levelHandler;

    private void Start()
    {
        _timer.Start();
    }

    private void UpdateTimer()
    {
        TimeSpan timeElapsed = _timer.Elapsed;
        timerText.text = timeElapsed.ToString(@"mm\:ss");

        if (timerText.text == "99:59")
        {
            _timer.Stop();
            return;
        }
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
