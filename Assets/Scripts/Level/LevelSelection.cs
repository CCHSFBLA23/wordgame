using System.Collections.Generic;
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

    private void Awake()
    {
        levelCount = SceneManager.sceneCountInBuildSettings - 1;

        int availableLevelIndex = 1;
        for(int i = 2; i <= levelCount; i++)
        {
            LevelData cur = SaveSystem.LoadLevelDataThroughBuildIndex(i - 1);
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
