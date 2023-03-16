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
    private bool _paused;
    
    public void OpenPauseMenu()
    {
        _paused = true;
        _PauseCanvasParent.SetActive(true);
        Populate(levelHandler.GetTitle(), timer.GetTimerText());
        LevelHandler.inputEnabled = false;
        timer.Pause();
    }

    public void ClosePauseMenu()
    { 
        _PauseCanvasParent.SetActive(false);
        timer.Unpause();
        LevelHandler.inputEnabled = true;
        _paused = false;
    }

    private void Populate(string levelName, string curTime)
    {
        title.text = levelName;
        time.text = curTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_paused)
            {
                OpenPauseMenu();
            }
            else
            {
                ClosePauseMenu();
            }
        }
    }
    
}
