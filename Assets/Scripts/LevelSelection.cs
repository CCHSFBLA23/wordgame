using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    public int levelCount;
    
    public GameObject levelButton;
    public SceneHandler sceneHandler;


    private void Awake()
    {
        
        levelCount = SceneManager.sceneCountInBuildSettings - 1;
        // Loop for as many times as there are levels, add the necessary functions and labels.
        for (int i = 1; i <= levelCount; i++)
        {
            GameObject button = Instantiate(levelButton, this.transform);
            button.GetComponentInChildren<TMP_Text>().text = i.ToString();
            int childIndex = button.transform.GetSiblingIndex() + 1;
            button.GetComponent<Button>().onClick.AddListener(() => GoToLevel(childIndex));
        }
    }

    private void GoToLevel(int levelIndex)
    {
        sceneHandler.LoadScene(levelIndex);
    }
}
