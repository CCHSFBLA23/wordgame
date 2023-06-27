using System.Diagnostics;
using Level;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Debug = UnityEngine.Debug;

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
        for(int i = 2; i <= levels.Length; i++)
        {
            LevelSaveData cur = SaveSystem.LoadLevelDataThroughBuildIndex(SceneIndexFromName(levels[i-2].name), levels[i-2].isSinglePlayer);
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

    private static string NameFromIndex(int buildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }
    private static int SceneIndexFromName(string sceneName) {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            string testedScreen = NameFromIndex(i);
            if (testedScreen == sceneName)
                return i;
        }
        return -1;
    }

}
