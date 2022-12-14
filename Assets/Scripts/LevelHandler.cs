using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelHandler : MonoBehaviour
{
    [Header("Level Properties")]
    [SerializeField] private string goalWord;
    [SerializeField] public string levelTitle;
    public bool solved = false;
    
    private BoxHandler _boxHandler;
    private bool _firstSolved = false;

    [Header("Score Stuff")]
    public Timer timer;
    public int buildIndex;
    public bool debugResetOnStart = false;

    private LevelEndController _levelEndController;
    private SceneHandler _sceneHandler;

    //Ran at start of level.
    private void Start()
    {
        timer.Pause();
        togglePlayerInput(true);
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        AudioManager.Play($"Level {buildIndex}");
        //When enabled in editor will delete save file.
        if (debugResetOnStart)
        {
            SaveSystem.DeleteLevelSaveFile(this);
        }
        
        _boxHandler = GetComponent<BoxHandler>();
        _levelEndController = GetComponent<LevelEndController>();
        _sceneHandler = GetComponent<SceneHandler>();
    }

    //Every frame checks if the level is solved.
     private void Update()
    {    
        //Loops through every box.
        foreach (var startBox in _boxHandler.boxes)
        {
            //Checks words from top to bottom. Finds the first box in a chain by checking if it has one below and not above.
            if (!_boxHandler.CheckBoxCollision(startBox.target, Vector2.up) && _boxHandler.CheckBoxCollision(startBox.target, Vector2.down))
            {
                var currentString = "";
                var cur = startBox;
                currentString += cur.letter;
                //Follows the chain of boxes and adds their letters to the word.
                while (_boxHandler.CheckBoxCollision(cur.target, Vector2.down))
                {
                    cur = _boxHandler.CheckBoxCollision(cur.target, Vector2.down);
                    currentString += cur.letter;
                }
                //Checks if the target word is the same as the current word.
                if (string.Equals(currentString, goalWord, StringComparison.CurrentCultureIgnoreCase))
                {
                    //Makes sure the end screen and sound effect are only played once.
                    if (!solved)
                    {
                        OnSolve();
                    }
                    solved = true;
                }
            }
            //SEE ABOVE
            //Same as above but goes from left to right. Checks if there are none to the left.
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
     // For Save System
     private void OnSolve()
     {
         UpdateBestScore();
         timer.Pause();
         togglePlayerInput(false);
         AudioManager.Play("LevelComplete");
         StartCoroutine(_sceneHandler.delay(0.4f, () => _levelEndController.Enable()));
         Debug.Log("level beat");
     }
    
    //Called on reset button press. Resets timer and positions. Reset undo list.
    public void Reset()
    {
        timer.Reset();
        //Finds initial position in undo list.
        _boxHandler.playerPosition.target = _boxHandler.playerPosition.positionHistory[0];
        _boxHandler.playerPosition.positionHistory.Clear();
        _boxHandler.playerPosition.positionHistory.Add(_boxHandler.playerPosition.target);
        
        //Gets boxes in current level.
        foreach (var box in _boxHandler.boxes)
        {
            box.target = box.positionHistory[0];
            box.positionHistory.Clear();
            box.positionHistory.Add(box.target);
        }
    }
    
    public void Undo()
    {
        //Checks if in initial position
        if (_boxHandler.playerPosition.positionHistory.Count == 1)
        {
            return;
        }
        
        //Finds the previous position of every box and changes their target position back to them.
        //Removes the current position from the list
        _boxHandler.playerPosition.target = _boxHandler.playerPosition.positionHistory[^2];
        _boxHandler.playerPosition.positionHistory.RemoveAt(_boxHandler.playerPosition.positionHistory.Count-1);
        foreach (var box in _boxHandler.boxes)
        {
            box.target = box.positionHistory[^2];
            box.positionHistory.RemoveAt(box.positionHistory.Count-1);
        }
    }
    
    //Bound in Player Input
    private void OnUndo(InputValue value)
    {
        if (value.isPressed)
        {
            Undo();
        }
    }
    
    public void UpdateBestScore()
    {
        double currentAttempt = timer.GetTimerSeconds();
        LevelData savedScore = SaveSystem.LoadLevelData(this);

        //Checks if there is a high score.
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
            //Creates a new save file for the level.
            SaveSystem.SaveLevelData(this);
            Debug.Log("New time to beat is: " + SaveSystem.LoadLevelScore(this).ToString(@"mm\:ss"));
        }
    }
    
    

    public string GetTitle()
    {
        return levelTitle;
    }
    
    //Causes the level to fade in when anything is pressed. Set up in Player Input
    private void OnAnyKey(InputValue value)
    {
        if (value.isPressed)
        {
            //Starts the animation for the fade in and out.
            GameObject.FindGameObjectWithTag("FadeTransition").GetComponent<Animator>().SetTrigger("FadeIn");
            GameObject.FindGameObjectWithTag("FadeTransition").transform.GetChild(0).GetComponent<Animator>().SetTrigger("FadeOut");
            StartCoroutine(_sceneHandler.delay(0.4f, () => timer.Unpause()));
        }
    }

    
    
    public void togglePlayerInput(bool setActive)
    {
        if (setActive)
            GetComponent<PlayerInput>().ActivateInput();
        else
            GetComponent<PlayerInput>().DeactivateInput();
    }

}
