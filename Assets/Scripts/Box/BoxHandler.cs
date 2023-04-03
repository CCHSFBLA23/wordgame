using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;

public class BoxHandler : MonoBehaviour
{
    //Mainly where I am planning to work on most of the game physics.
    //could possibly break into separate classes, but I think it would just be annoying to maintain
    
    //Box lists for each type that needs it.
    public Box[] boxes;
    public bool inBetweenMoves;
    private List<Box> _falling = new List<Box>();
    private List<Box> _linked = new List<Box>();

    private LevelHandler _levelHandler;
    public List<GridPosition> playerPositions;
    public Tilemap walls;
    
    //Input Manager
    private Vector2 _inputVector;
    private Vector2 _inputVectorPlayer2;
    


    private void Start()
    {
        GameObject[] boxObjects = GameObject.FindGameObjectsWithTag("box");
        
        boxes = new Box[boxObjects.Length];
        
        for (int i = 0; i < boxObjects.Length; i++)
        {
            boxes[i] = boxObjects[i].GetComponent<Box>();
        }

        _levelHandler = GetComponent<LevelHandler>();
        _levelHandler.boxes = boxes;
    }
    //Moves back to beginning. Stores them in dictionary in the start.
    
    
    //Checks if there is a tile in the Walls tilemap at the position that is about to be moved to.
    //Returns true if there is a tile there.
    private bool CheckWallAndPlayerCollisions(GridPosition current, Vector2 position, Vector2 moveVector)
    {
        Vector2 destination = position + moveVector;
        
        foreach (var playerPosition in playerPositions)
        {
            if (playerPosition.target == destination && playerPosition != current) return true;
        }
        
        return walls.GetTile(Vector3Int.FloorToInt(destination));
    }
    
    //Checks if there is a box in the scene that is at the position that is about to be moved to.
    //Returns either a reference to the object or null.
    public bool CheckBoxCollision(Vector2 position, Vector2 moveVector, out Box collidedBox)
    {
        
        foreach (var t in boxes)
        {
            if (t.target != position + moveVector) continue;
            collidedBox = t;
            return true;
        }
        
        collidedBox = null;
        return false;
    }
    //Overloads previous method to not require an output.
    public bool CheckBoxCollision(Vector2 position, Vector2 moveVector)
    {
        foreach (var t in boxes)
        {
            if (t.target != position + moveVector) continue;
            return true;
        }
        
        return false;
    }
    
    //When moving, this is used to deal with the player moving and pushing in general.
    //Basically, it checks if there is a box in the direction you are moving, then checks again using that box's center,
    //And so on until you hit a wall or run out of boxes.
    //Returns true if the player can move.
    //If there is not a wall, it will move every box in the row.
    private bool PushRowOfBoxes(GridPosition curPlayer, Vector2 position, Vector2 moveVector)
    {
        var toPush = new List<Box>();
        
        foreach (var playerPosition in playerPositions)
        {
            Debug.Log("Player Position: " + playerPosition.target);
            Debug.Log("Box Target: " + (position + moveVector));
            
            if (playerPosition.target != position + moveVector) continue;
            return false;
        }
        
        if (CheckBoxCollision(position, moveVector, out var cur))
        {
            if (!cur.pushable) return false;
            toPush.Add(cur);
        }
        else
        {
            return true;
        }

        
        while (CheckBoxCollision(cur.target, moveVector, out cur))
        {
            if (!cur.pushable) return false;
            if (cur.target == curPlayer.target) return false;
            toPush.Add(cur);
        }

        if (toPush.Any() && CheckWallAndPlayerCollisions(curPlayer, toPush.Last().target, moveVector))
        {
            return false;
        }

        bool moved = false;
        foreach (var gridPosition in toPush)
        {
            if (gridPosition.linked && !gridPosition.linkedMoved)
            {
                gridPosition.linkedMoved = true;
                foreach (var box in _linked)
                {
                    if (!toPush.Contains(box) && !moved && !box.linkedMoved)
                    {
                        box.linkedMoved = true;
                        PushRowOfBoxes(curPlayer, box.target - moveVector, moveVector);
                    }
                }
                moved = true;
            }
            gridPosition.target += moveVector;
        }

        return true;
    }

    //Pushes in negative move vector basically.
        //See PushRowOfBoxes for a similar process.
        void PullRowOfBoxes(Vector2 position, Vector2 moveVector)
        {
            var toPull = new List<GridPosition>();

            if (!CheckBoxCollision(position, -moveVector, out var cur)) return;
            CheckBoxCollision(position, -moveVector, out cur);
            if (!cur.pullable) return;
            toPull.Add(cur);
        
            while (CheckBoxCollision(cur.target, -moveVector, out cur))
            {
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
        private void CalculateMovementPlayer(GridPosition curPlayer)
        {
            if (Vector2.Distance(curPlayer.current, curPlayer.target) > .05f)
            {
                inBetweenMoves = true;
            }
            
            var inputVector = curPlayer == playerPositions.First() ? _inputVector : _inputVectorPlayer2;

            if (!(Vector2.Distance(curPlayer.current, curPlayer.target) <= .05f) || inputVector == Vector2.zero) return;
                var moveVector = CalcMoveVector(inputVector);
            if (moveVector == Vector2.zero) return;
            
            var canMovePlayer = true;
            if (CheckBoxCollision(curPlayer.target, moveVector))
            {
                //Checks if the original box pushed is a linked box.
                canMovePlayer = PushRowOfBoxes(curPlayer, curPlayer.target, moveVector);
            }

            if (CheckWallAndPlayerCollisions(curPlayer, curPlayer.target, moveVector) || !canMovePlayer) return;
            PullRowOfBoxes(curPlayer.target, moveVector);
            curPlayer.target += moveVector;
            AudioManager.Play("PlayerMove", true);
            
            UpdateMoveHistory();
        }

        private void UpdateMoveHistory()
        {
            foreach (var curPlayer in playerPositions)
            {
                curPlayer.positionHistory.Add(curPlayer.target);
            }
            foreach (var box in boxes)
            {
                box.positionHistory.Add(box.target);
            }
        }

        private void CalculateFallingMovement()
        {
            foreach (var box in _falling)
            {
            
                if(Vector2.Distance(box.target, box.current) > 0.1f) return;
                Vector2 cur = box.target;
                if (cur + Vector2.down == playerPositions.First().target) return;

                while (true)
                {
                
                    if (CheckWallAndPlayerCollisions(null, cur, Vector2.down)) break;
                    if (CheckBoxCollision(cur, Vector2.down)) break;
                    cur += Vector2.down;
                }
                box.target = cur;
            }
        }
        private Vector2 CalcMoveVector(Vector2 input)
        {
            Vector2 moveVector = new Vector2();
            float inputDirection = Vector2.SignedAngle(input, Vector2.right);
            if (Math.Abs(inputDirection) < 10f)
            {
                moveVector = Vector2.right;
            }
            else if (Math.Abs(inputDirection - (-90f)) < 10f)
            {
                moveVector = Vector2.up;
            }
            else if (Math.Abs(inputDirection - 90f) < 10f)
            {
                moveVector = Vector2.down;
            }
            else if (Math.Abs(inputDirection - 180f) < 10f)
            {
                moveVector = Vector2.left;
            }
            
            return moveVector;
        }
    
        private void Update()
        {
            inBetweenMoves = false;
            if (LevelHandler.inputEnabled)
            {
                _inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if (!_levelHandler.isSinglePlayer)
                {
                    _inputVectorPlayer2 = new Vector2(Input.GetAxisRaw("Horizontal 2"), Input.GetAxisRaw("Vertical 2"));
                }
            }
            else
            {
                _inputVector = Vector2.zero;
                _inputVectorPlayer2 = Vector2.zero;
            }
            
            _falling = new List<Box>();
            _linked = new List<Box>();
            
            foreach (var box in boxes)
            {
                if (box.falling)
                {
                    _falling.Add(box);
                }

                if (box.linked)
                {
                    box.linkedMoved = false;
                    _linked.Add(box);
                }
            }

            foreach (var playerPosition in playerPositions)
            {
                
                CalculateMovementPlayer(playerPosition);
            }
            CalculateFallingMovement();

        }

}