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
        List<int> availableLevels = new List<int> { 1 };
        for(int index = 2; index <= levelCount; index++)
        {
            LevelData cur = SaveSystem.LoadLevelDataThroughBuildIndex(index - 1);
            if (cur != null)
            {
                availableLevels.Add(index);
            }
        }
        
        // Loop for as many times as there are levels, add the necessary functions and labels.
        foreach (var i in availableLevels)
        {
            GameObject button = Instantiate(levelButton, this.transform);
            button.GetComponentInChildren<TMP_Text>().text = i.ToString();
            int childIndex = button.transform.GetSiblingIndex() + 1;
            button.GetComponent<Button>().onClick.AddListener(() => AudioManager.Play("Button"));
            button.GetComponent<Button>().onClick.AddListener(() => GoToLevel(childIndex));
            
        }
        
    }

    private void GoToLevel(int levelIndex)
    {
        sceneHandler.LoadScene(levelIndex);
    }
}
