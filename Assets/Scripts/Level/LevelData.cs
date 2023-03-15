using System.Diagnostics;

[System.Serializable]
public class LevelData
{
    public int levelIndex;
    public double seconds;

    public LevelData (LevelHandler levelHandler)    
    {
        levelIndex = levelHandler.buildIndex;
        seconds = levelHandler.timer.GetTimerSeconds();
    }

    public LevelData (int buildIndex, double time)
    {
        levelIndex = buildIndex;
        seconds = time;
    }
}
