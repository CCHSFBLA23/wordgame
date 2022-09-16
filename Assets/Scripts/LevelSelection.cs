using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    public int levelCount;
    
    public GameObject levelButton;

    void Awake()
    {
        // Loop for as many times as there are levels, add the necessary functions and labels.
        for (int i = 1; i <= levelCount; i++)
        {
            GameObject button = Instantiate(levelButton, this.transform);
            button.GetComponentInChildren<TMP_Text>().text = i.ToString();
            int childIndex = button.transform.GetSiblingIndex() + 1;
            button.GetComponent<Button>().onClick.AddListener(() => GoToLevel(childIndex));
        }
    }

    void GoToLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}