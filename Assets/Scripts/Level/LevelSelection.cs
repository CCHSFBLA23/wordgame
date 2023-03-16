using Level;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    private int levelCount;
    public GameObject levelButton;
    public SceneHandler sceneHandler;
    public bool isForSinglePlayer;
    public LevelData[] levels;

    private void Awake()
    {
        levelCount = levels.Length;

        int availableLevelIndex = 1;
        for(int i = 2; i <= levelCount; i++)
        {
            LevelSaveData cur = SaveSystem.LoadLevelDataThroughBuildIndex(i - 1, isForSinglePlayer);
            if (cur != null)
            {
                availableLevelIndex += 1;
            }
        }

        // Loop for as many times as there are levels, add the necessary functions and labels.
        int index = 0;
        foreach (var level in levels)
        {
            GameObject button = Instantiate(levelButton, this.transform);
            button.GetComponentInChildren<TMP_Text>().text = (index + 1).ToString();

            if (index < availableLevelIndex)
            {
                button.GetComponent<Button>().onClick.AddListener(() => AudioManager.Play("Button"));
                button.GetComponent<Button>().onClick.AddListener(() => GoToLevel(level.buildIndex));
            }
            else
            {
                button.GetComponent<Button>().interactable = false;
            }

            index++;
        }
    }

    private void GoToLevel(int levelIndex)
    {
        sceneHandler.LoadScene(levelIndex);
    }
}
