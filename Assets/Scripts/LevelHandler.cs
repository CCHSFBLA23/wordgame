using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private string goalWord;
    [SerializeField] public string levelTitle;
    
    private BoxHandler _boxHandler;
    private Dictionary<GridPosition, Vector2> _startingLocations = new Dictionary<GridPosition, Vector2>();
    
    public bool solved = false;
    private bool _firstSolved = false;

    [Header("Score Stuff")]
    public Timer timer;
    public int buildIndex;
    public bool debugResetOnStart = false;

    private LevelEndController _levelEndController;
    private SceneHandler _sceneHandler;

    private void Start()
    {
        togglePlayerInput(true);
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        AudioManager.Play($"Level {buildIndex}");
        // Doing this because I am too lazy to add a custom inspector button
        if (debugResetOnStart)
        {
            SaveSystem.DeleteLevelSaveFile(this);
        }
        
        _boxHandler = GetComponent<BoxHandler>();
        _startingLocations[_boxHandler.playerPosition] = _boxHandler.playerPosition.target;
        foreach (var box in _boxHandler.boxes)
        {
            _startingLocations[box] = box.target;
        }

        _levelEndController = GetComponent<LevelEndController>();
        _sceneHandler = GetComponent<SceneHandler>();
    }

    public void Reset()
    {
        foreach (var pair in _startingLocations)
        {
            pair.Key.target = pair.Value;
        }
        solved = false;
    }

    public string GetTitle()
    {
        return levelTitle;
    }
    
    private void Update()
    {    
        foreach (var startBox in _boxHandler.boxes)
        {
            if (!_boxHandler.CheckBoxCollision(startBox.target, Vector2.up) && _boxHandler.CheckBoxCollision(startBox.target, Vector2.down))
            {
                var currentString = "";
                var cur = startBox;
                currentString += cur.letter;
                while (_boxHandler.CheckBoxCollision(cur.target, Vector2.down))
                {
                    cur = _boxHandler.CheckBoxCollision(cur.target, Vector2.down);
                    currentString += cur.letter;
                }
                if (string.Equals(currentString, goalWord, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!solved)
                    {
                        OnSolve();
                    }
                    solved = true;
                }
            }
            
            if (!_boxHandler.CheckBoxCollision(startBox.target, Vector2.left) && _boxHandler.CheckBoxCollision(startBox.target, Vector2.right))
            {
                var currentString = "";
                var cur = startBox;
                currentString += cur.letter;
                while (_boxHandler.CheckBoxCollision(cur.target, Vector2.right))
                {
                    cur = _boxHandler.CheckBoxCollision(cur.target, Vector2.right);
                    currentString += cur.letter;
                }
                if (string.Equals(currentString, goalWord, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!solved)
                    {
                        OnSolve();
                    }
                    solved = true;
                }
            }
        }
    }


    private void OnSolve()
    {
        UpdateBestScore();
        timer.Pause();
        togglePlayerInput(false);
        AudioManager.Play("LevelComplete");
        StartCoroutine(_sceneHandler.delay(0.4f, () => _levelEndController.Enable()));
        Debug.Log("level beat");
    }

    public void togglePlayerInput(bool setActive)
    {
        if (setActive)
            GetComponent<PlayerInput>().ActivateInput();
        else
            GetComponent<PlayerInput>().DeactivateInput();
    }

    // For Save System
    public void UpdateBestScore()
    {
        double currentAttempt = timer.GetTimerSeconds();
        LevelData savedScore = SaveSystem.LoadLevelData(this);

        if (savedScore != null)
        {
            if (savedScore.seconds > currentAttempt)
            {
                SaveSystem.SaveLevelData(this);
                Debug.Log("New time to beat is: " + SaveSystem.LoadLevelScore(this).ToString(@"mm\:ss"));
            }
        }
        else
        {
            SaveSystem.SaveLevelData(this);
            Debug.Log("New time to beat is: " + SaveSystem.LoadLevelScore(this).ToString(@"mm\:ss"));
        }
    }
}
