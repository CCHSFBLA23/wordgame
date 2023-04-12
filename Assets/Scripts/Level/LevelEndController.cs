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
    public GameObject newHighScoreIndicator;
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
        if (time.text == highScoreText.text.Substring(12))
        {
            StartCoroutine(SceneHandler.delay(0.2f, () => { AudioManager.Play("HighScore"); newHighScoreIndicator.SetActive(true); }));
        }
        if (!levelHandler.lastLevelInSeries)
        {
            GameObject.FindGameObjectWithTag("NextButton").GetComponent<Button>().interactable = true;
        }
    }

    public void Disable()
    {
        levelEndCanvasParent.SetActive(false);
        newHighScoreIndicator.SetActive(false);
    }

    private string GenerateCommendation()
    {
        return commendations[Random.Range(0, commendations.Length)];
    }
}
