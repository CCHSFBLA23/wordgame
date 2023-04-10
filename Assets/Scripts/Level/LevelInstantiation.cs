using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        private Color32 _playerOneColor = new Color32(255, 189, 89, 255);
        private Color _playerTwoColor = new Color32(126, 217, 87, 255);

        void Awake()
        {
            if (levelData == null)
            {
                Debug.LogError("Missing LevelData!");
                return;
            }
        
            GameObject level = Instantiate(levelPrefab);
            LevelHandler levelHandler = level.GetComponentInChildren<LevelHandler>();
            BoxHandler boxHandler = level.GetComponentInChildren<BoxHandler>();
            SceneHandler sceneHandler = level.GetComponentInChildren<SceneHandler>();
            LevelListTypes levelListTypes = level.GetComponentInChildren<LevelListTypes>();
            
            
            levelHandler.levelTitle = levelData.levelName;
            levelHandler.goalWord = levelData.goalWord;
            levelHandler.isSinglePlayer = levelData.isSinglePlayer;
            levelHandler.levelName = levelData.name;
            
            var position = playerStartingPosition.position;
            var playerPosition = boxHandler.playerPositions.First();
            playerPosition.target = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
            playerPosition.positionHistory[0] = playerPosition.target;
            
            Destroy(playerStartingPosition.gameObject);

            sceneHandler.cur = levelData;
            sceneHandler.list = levelData.isSinglePlayer ? levelListTypes.singlePlayer : levelListTypes.multiPlayer;
            levelHandler.lastLevelInSeries = sceneHandler.list.levels.Last() == levelData || !sceneHandler.list.levels.Contains(levelData);

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
