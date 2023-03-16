using System.Diagnostics;

[System.Serializable]
public class LevelSaveData
{
    public int levelIndex;
    public double seconds;

    public LevelSaveData (LevelHandler levelHandler)    
    {
        levelIndex = levelHandler.buildIndex;
        seconds = levelHandler.timer.GetTimerSeconds();
    }

    public LevelSaveData (int buildIndex, double time)
    {
        levelIndex = buildIndex;
        seconds = time;
    }
}
