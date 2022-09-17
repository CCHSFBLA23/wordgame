using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class BoxHandler : MonoBehaviour
{
    //Mainly where I am planning to work on most of the game physics.
    //could possibly break into separate classes, but I think it would just be annoying to maintain
    
    //Game Objects
    public GameObject player;
    //Box lists for each type that needs it.
    [HideInInspector]public Box[] boxes;
    private List<Box> _falling = new List<Box>();
    private List<Box> _linked = new List<Box>();

    private LevelHandler _levelHandler;
    private GridPosition _playerPosition;
    public Tilemap walls;
    
    //Input Manager
    private Vector2 _inputVector;
    
    //Reset Function
    private Dictionary<GridPosition, Vector2> startingLocations = new Dictionary<GridPosition, Vector2>();

    private void Start()
    {
        _playerPosition = player.GetComponent<GridPosition>();
        startingLocations[_playerPosition] = _playerPosition.target;
        
        GameObject[] boxObjects = GameObject.FindGameObjectsWithTag("box");
        boxes = new Box[boxObjects.Length];
        
        for (int i = 0; i < boxObjects.Length; i++)
        {
            boxes[i] = boxObjects[i].GetComponent<Box>();
            //Sets starting positions for each of the boxes.
            startingLocations[boxes[i]] = boxes[i].target;
            //Adds boxes of type in to list.
            if (boxes[i].falling)
            {
                _falling.Add(boxes[i]);
            }
            if (boxes[i].linked)
            {
                _linked.Add(boxes[i]);
            }
        }

        _levelHandler = GetComponent<LevelHandler>();
    }
    //Moves back to beginning. Stores them in dictionary in the start.
    public void Reset()
    {
        foreach (var pair in startingLocations)
        {
            pair.Key.target = pair.Value;
        }
    }
    private void OnMove(InputValue value)
    {
        _inputVector = value.Get<Vector2>();
    }
    
    //Checks if there is a tile in the Walls tilemap at the position that is about to be moved to.
    //Returns true if there is a tile there.
    private bool CheckWallCollisions(Vector2 position, Vector2 moveVector)
    {
        Vector2 destination = position + moveVector;
        
        return walls.GetTile(Vector3Int.FloorToInt(destination));
    }
    
    //Checks if there is a box in the scene that is at the position that is about to be moved to.
    //Returns either a reference to the object or null.
    public Box CheckBoxCollision(Vector2 position, Vector2 moveVector)
    {
        foreach (var t in boxes)
        {
            if (t.target == position + moveVector)
            {
                return t;
            }
        }

        return null;
    }
    
    //When moving, this is used to deal with the player moving and pushing in general.
    //Basically, it checks if there is a box in the direction you are moving, then checks again using that box's center,
    //And so on until you hit a wall or run out of boxes.
    //Returns true if the player can move.
    //If there is not a wall, it will move every box in the row.
    private bool PushRowOfBoxes(Vector2 position, Vector2 moveVector)
    {
        Box cur;
        var toPush = new List<GridPosition>();

        if (CheckBoxCollision(position, moveVector))
        {
            cur = CheckBoxCollision(position, moveVector);
            if (!cur.pushable) return false;
            toPush.Add(cur);
        }
        else
        {
            return true;
        }

        while (CheckBoxCollision(cur.target, moveVector))
        {
            cur = CheckBoxCollision(cur.target, moveVector);
            if (!cur.pushable) return false;
            toPush.Add(cur);
        }
        
        if (CheckWallCollisions(cur.target, moveVector))
        {
            return false;
        }
        else
        {
            foreach (var gridPosition in toPush)
            {
                gridPosition.target += moveVector;
            }
            return true;
        }
    }
    //Pushes in negative move vector basically.
    //See PushRowOfBoxes for a similar process.
    void PullRowOfBoxes(Vector2 position, Vector2 moveVector)
    {
        Box cur;
        var toPull = new List<GridPosition>();

        if (!CheckBoxCollision(position, -moveVector)) return;
        cur = CheckBoxCollision(position, -moveVector);
        if (!cur.pullable) return;
        toPull.Add(cur);
        
        while (CheckBoxCollision(cur.target, -moveVector))
        {
            cur = CheckBoxCollision(cur.target, -moveVector);
            if (!cur.pullable) break;
            toPull.Add(cur);
        }

        foreach (var gridPosition in toPull)
        {
            gridPosition.target += moveVector;
        }
    }

    //Deals with sending input to the player target (aka preventing it from jumping around).
    //Checks walls and the box pushing stuff above.
    private void CalculateMovementPlayer()
    {
        if (!(Vector2.Distance(_playerPosition.current, _playerPosition.target) <= .05f) || _inputVector == Vector2.zero) return;
        
        var moveVector = CalcMoveVector(_inputVector);
        
        var canMoveBoxes = true;
        if (CheckBoxCollision(_playerPosition.target, moveVector))
        {
            //Checks if the original box pushed is a linked box.
            var originalBox = CheckBoxCollision(_playerPosition.target, moveVector);
            canMoveBoxes = PushRowOfBoxes(_playerPosition.target, moveVector);
            if (originalBox.linked && canMoveBoxes)
            {
                foreach (var box in _linked)
                {
                    if (box == originalBox) continue;
                    if (!CheckBoxCollision(box.target, moveVector))
                    {
                        PushRowOfBoxes(box.target - moveVector, moveVector);
                    }
                    else if(!CheckBoxCollision(box.target, moveVector).linked)
                    {
                        PushRowOfBoxes(box.target - moveVector, moveVector);
                    }
                }
            }
        }
        

        if (!CheckWallCollisions(_playerPosition.target, moveVector) && canMoveBoxes)
        {
            PullRowOfBoxes(_playerPosition.target, moveVector);
            _playerPosition.target += moveVector;
        }
    }
    private void CalculateFallingMovement()
    {
        
        foreach (var box in _falling)
        {
            
            if(Vector2.Distance(box.target, box.current) > 0.1f) return;
            Vector2 cur = box.target;
            if (cur + Vector2.down == _playerPosition.target) return;
            
            while (true)
            {
                
                if (CheckWallCollisions(cur, Vector2.down)) break;
                if (CheckBoxCollision(cur, Vector2.down)) break;
                
                cur += Vector2.down;
            }
            box.target = cur;
        }
    }
    private Vector2 CalcMoveVector(Vector2 input)
    {
        Vector2 moveVector = new Vector2();
        if (input.x != 0 ^ input.y != 0)
        {
            moveVector = input;
        }
        return moveVector;
    }
    
    private void Update()
    {
        CalculateMovementPlayer();
        CalculateFallingMovement();
    }

}