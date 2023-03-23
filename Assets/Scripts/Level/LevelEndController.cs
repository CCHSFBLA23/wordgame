using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndController : MonoBehaviour
{
    public TMP_Text levelTitleText;
    public TMP_Text highScoreText;
    public TMP_Text time;
    
    public GameObject levelEndCanvasParent;
    public LevelHandler levelHandler;
    public Timer timer;

    public void Enable()
    {
        levelEndCanvasParent.SetActive(true);
        levelTitleText.text = levelHandler.GetTitle();
        time.text = timer.GetTimerText();
        highScoreText.text = "HIGH SCORE: " + SaveSystem.LoadLevelScore(new LevelSaveData(levelHandler.buildIndex, levelHandler.timer.GetTimerSeconds()), levelHandler.isSinglePlayer).ToString(@"mm\:ss");
        if (levelHandler.lastLevelInSeries)
        {
            GameObject.FindGameObjectWithTag("NextButton").GetComponent<Button>().interactable = false;
        }
    }

    public void Disable()
    {
        levelEndCanvasParent.SetActive(false);
    }
}
