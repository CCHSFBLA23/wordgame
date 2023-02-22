using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class LevelInstantiation : MonoBehaviour
{
    [Header("Level Settings")]
    public string levelName;
    public string goalWord;
    [Header("Drag and Drop")]
    // Start is called before the first frame update
    public GameObject levelPrefab;
    public Transform playerStartingPosition;
    public Tilemap walls;
    
    
    void Start()
    {
        GameObject level = Instantiate(levelPrefab);
        LevelHandler levelHandler = level.GetComponentInChildren<LevelHandler>();
        BoxHandler boxHandler = level.GetComponentInChildren<BoxHandler>();
        levelHandler.levelTitle = levelName;
        levelHandler.goalWord = goalWord;
        var position = playerStartingPosition.position;
        var playerPosition = boxHandler.player.GetComponent<GridPosition>();
        playerPosition.target = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        playerPosition.positionHistory[0] = playerPosition.target;
        boxHandler.walls = walls;
        Destroy(playerStartingPosition.gameObject);
    }
}
