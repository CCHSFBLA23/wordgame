using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndController : MonoBehaviour
{
    public TMP_Text commendationText;
    public TMP_Text levelTitleText;
    public TMP_Text highScoreText;
    public TMP_Text time;
    
    public GameObject levelEndCanvasParent;
    public LevelHandler levelHandler;
    public Timer timer;
    public string[] commendations;

    public void Enable()
    {
        levelEndCanvasParent.SetActive(true);
        commendationText.text = GenerateCommendation();
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

    private string GenerateCommendation()
    {
        return commendations[Random.Range(0, commendations.Length)];
    }
}
