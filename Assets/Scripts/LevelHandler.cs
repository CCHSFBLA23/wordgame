using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private string goalWord;
    
    private BoxHandler _boxHandler;
    private Dictionary<GridPosition, Vector2> _startingLocations = new Dictionary<GridPosition, Vector2>();
    
    public bool solved = false;

    private void Start()
    {
        _boxHandler = GetComponent<BoxHandler>();
        _startingLocations[_boxHandler.playerPosition] = _boxHandler.playerPosition.target;
        foreach (var box in _boxHandler.boxes)
        {
            _startingLocations[box] = box.target;
        }
    }

    public void Reset()
    {
        foreach (var pair in _startingLocations)
        {
            pair.Key.target = pair.Value;
        }
        solved = false;
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
                    solved = true;
                }
            }
        }
    }
    
}
