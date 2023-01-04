using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
        timer.Pause();
        levelHandler.GetComponent<PlayerInput>().SwitchCurrentActionMap("PauseMenu");
    }

    public void ClosePauseMenu()
    { 
        _PauseCanvasParent.SetActive(false);
        timer.Unpause();
        _paused = false;
        levelHandler.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }

    private void Populate(string levelName, string curTime)
    {
        title.text = levelName;
        time.text = curTime;
    }

    private void OnPause(InputValue value)
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
