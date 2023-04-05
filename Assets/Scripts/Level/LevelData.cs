﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        public string levelName;
        public string goalWord;
        public bool isSinglePlayer;

        public bool lastLevelInSeries;
        public int buildIndex;
    }
}