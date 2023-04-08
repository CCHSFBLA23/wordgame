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
    public LevelList list;

    private void Start()
    {
        LevelData[] levels = list.levels;
        int availableLevelIndex = 1;
        for(int i = 2; i <= levelCount; i++)
        {
            LevelSaveData cur = SaveSystem.LoadLevelDataThroughBuildIndex(levels[i-2].buildIndex, levels[i-2].isSinglePlayer);
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
            button.gameObject.SetActive(false);
            button.GetComponentInChildren<TMP_Text>().text = (index + 1).ToString();

            if (index < availableLevelIndex)
            {
                button.GetComponent<Button>().interactable = true;
                button.GetComponent<Button>().onClick.AddListener(() => AudioManager.Play("Button"));
                button.GetComponent<Button>().onClick.AddListener(() => GoToLevel(level.name));
            }
            button.gameObject.SetActive(true);
            index++;
        }
    }

    private void GoToLevel(string sceneName)
    {
        Debug.Log(sceneName);
        sceneHandler.LoadScene(sceneName);
    }

    private void GoToLevel(int levelIndex)
    {
        sceneHandler.LoadScene(levelIndex);
    }
}
