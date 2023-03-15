using System.Collections.Generic;
using Level;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class LevelSelection : MonoBehaviour
{
    public int levelCount;
    
    public GameObject levelButton;
    public SceneHandler sceneHandler;
    public bool isForSinglePlayer;
    

    private void Awake()
    {
        levelCount = SceneManager.sceneCountInBuildSettings;

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
        for (var i = 1; i <= levelCount; i++)
        {
            GameObject button = Instantiate(levelButton, this.transform);
            button.GetComponentInChildren<TMP_Text>().text = i.ToString();
            if (i <= availableLevelIndex)
            {
                int childIndex = button.transform.GetSiblingIndex() + 1;
                button.GetComponent<Button>().onClick.AddListener(() => AudioManager.Play("Button"));
                button.GetComponent<Button>().onClick.AddListener(() => GoToLevel(childIndex));
            } 
            else
            {
                button.GetComponent<Button>().interactable = false;
            }   
        }
    }

    private void GoToLevel(int levelIndex)
    {
        sceneHandler.LoadScene(levelIndex);
    }
}
