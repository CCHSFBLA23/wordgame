using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LevelList : ScriptableObject
    {
        public LevelData[] levels;

        public LevelData GetNext(LevelData data)
        {
            int index = Array.IndexOf(levels, data);
            if (index == -1 || index >= levels.Length)
            {
                return null;
            }
            return levels[index + 1];
        }

        public LevelData[] GetLevels()
        {
            return levels;
        }
    }
}