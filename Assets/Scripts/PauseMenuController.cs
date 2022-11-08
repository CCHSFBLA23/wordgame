using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    //PauseMenu
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text time;
    [SerializeField] private GameObject _PauseCanvasParent;
    [SerializeField] private Timer timer;
    [SerializeField] private LevelHandler levelHandler;
    
    public void OpenPauseMenu()
    {
        _PauseCanvasParent.SetActive(true);
        Populate(levelHandler.GetTitle(), timer.GetTimerText());
        timer.Pause();
        Time.timeScale = 0.0f;
    }

    public void ClosePauseMenu()
    { 
        _PauseCanvasParent.SetActive(false);
        timer.Unpause();
        Time.timeScale = 1.0f;
    }

    private void Populate(string levelName, string curTime)
    {
        title.text = levelName;
        time.text = curTime;
    }
}
