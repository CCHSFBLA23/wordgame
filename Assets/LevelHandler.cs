using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private string goalWord;
    private BoxHandler _boxHandler;
    public bool solved = false;

    private void Start()
    {
        _boxHandler = GetComponent<BoxHandler>();
    }

    private void Update()
    {
        if (solved) return;
        
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
