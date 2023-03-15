using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        public string levelName;
        public string goalWord;
        public bool isSinglePlayer;

        public int buildIndex;
    }
}