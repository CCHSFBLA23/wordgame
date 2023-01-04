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
        timer.Pause();
        togglePlayerInput(true);
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        AudioManager.Play($"Level {buildIndex}");
        // Doing this because I am too lazy to add a custom inspector button
        if (debugResetOnStart)
        {
            SaveSystem.DeleteLevelSaveFile(this);
        }
        
        _boxHandler = GetComponent<BoxHandler>();
        _levelEndController = GetComponent<LevelEndController>();
        _sceneHandler = GetComponent<SceneHandler>();
    }

    public void Reset()
    {
        timer.Reset();
        _boxHandler.playerPosition.target = _boxHandler.playerPosition.positionHistory[0];
        _boxHandler.playerPosition.positionHistory.Clear();
        _boxHandler.playerPosition.positionHistory.Add(_boxHandler.playerPosition.target);
        foreach (var box in _boxHandler.boxes)
        {
            box.target = box.positionHistory[0];
            box.positionHistory.Clear();
            box.positionHistory.Add(box.target);
        }
    }


    
    private void OnUndo(InputValue value)
    {
        if (value.isPressed)
        {
            Undo();
        }
    }
    public void Undo()
    {
        if (_boxHandler.playerPosition.positionHistory.Count == 1)
        {
            return;
        }
        
        _boxHandler.playerPosition.target = _boxHandler.playerPosition.positionHistory[^2];
        _boxHandler.playerPosition.positionHistory.RemoveAt(_boxHandler.playerPosition.positionHistory.Count-1);
        foreach (var box in _boxHandler.boxes)
        {
            box.target = box.positionHistory[^2];
            box.positionHistory.RemoveAt(box.positionHistory.Count-1);
        }
    }

    public string GetTitle()
    {
        return levelTitle;
    }

    private void OnAnyKey(InputValue value)
    {
        if (value.isPressed)
        {
            GameObject.FindGameObjectWithTag("FadeTransition").GetComponent<Animator>().SetTrigger("FadeIn");
            GameObject.FindGameObjectWithTag("FadeTransition").transform.GetChild(0).GetComponent<Animator>().SetTrigger("FadeOut");
            StartCoroutine(_sceneHandler.delay(0.4f, () => timer.Unpause()));
        }
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
