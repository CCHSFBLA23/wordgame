using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Level
{
    public class LevelInstantiation : MonoBehaviour
    {
        [Header("Level Settings")] 
        public LevelData levelData;
    
        [Header("Drag and Drop")]
        public Transform playerStartingPosition;
        public Transform playerTwoStartingPosition;

        [Header("Don't Touch")]
        public GameObject levelPrefab;
        public GameObject playerPrefab;
        public Tilemap walls;
    
        //Hardcoded Colors Bad Someone Change Later
        private readonly Color _playerOneColor = Color.red; 
        private readonly Color _playerTwoColor = Color.blue;
    
        void Start()
        {
            if (levelData == null)
            {
                Debug.LogError("Missing LevelData!");
                return;
            }
        
            GameObject level = Instantiate(levelPrefab);
            LevelHandler levelHandler = level.GetComponentInChildren<LevelHandler>();
            BoxHandler boxHandler = level.GetComponentInChildren<BoxHandler>();

            levelHandler.levelTitle = levelData.levelName;
            levelHandler.goalWord = levelData.goalWord;
            levelHandler.isSinglePlayer = levelData.isSinglePlayer;

            levelHandler.lastLevelInSeries = levelData.lastLevelInSeries;
            
            var position = playerStartingPosition.position;
            var playerPosition = boxHandler.playerPositions.First();
            playerPosition.target = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
            playerPosition.positionHistory[0] = playerPosition.target;

            Destroy(playerStartingPosition.gameObject);
        
            if (!levelData.isSinglePlayer)
            {
            
                if (playerTwoStartingPosition == null)
                {
                    Debug.LogError("Missing Player Two Starting Position. (Make sure IsSinglePlayer is set correctly!)");
                    return;
                }
                boxHandler.playerPositions.Add(Instantiate(playerPrefab, level.transform).GetComponent<GridPosition>());
            
                var positionTwo = playerTwoStartingPosition.position;
                var playerPositionTwo = boxHandler.playerPositions[1];
                playerPositionTwo.target = new Vector2(Mathf.Round(positionTwo.x), Mathf.Round(positionTwo.y));
                playerPositionTwo.positionHistory[0] = playerPositionTwo.target;

                boxHandler.playerPositions.First().GetComponent<SpriteRenderer>().color = _playerOneColor;
                boxHandler.playerPositions.Last().GetComponent<SpriteRenderer>().color = _playerTwoColor;
            
                Destroy(playerTwoStartingPosition.gameObject);
            }
            boxHandler.walls = walls;
        }
    }
}
