using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public Animator transitionAnim;
    public LevelList list;
    public LevelData cur;
    public void LoadScene(int index)
    {
        if (index <= SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(transition(index));
        }
        else
        {
            Debug.LogWarning("There is no scene with the index: " + index.ToString() + "!");
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(transition(sceneName));
    }

    public void LoadNextScene()
    {
        LevelData next = list.GetNext(cur);
        if (next != null)
        {
            StartCoroutine(transition(next.name));
        }
        else
        {
            Debug.LogWarning("There is not a scene with an index higher than the current index of: " + SceneManager.GetActiveScene().buildIndex.ToString() + "!");
        }
            
        
           
    }

    public void ReloadScene()
    {
        StartCoroutine(transition(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator transition(int levelIndex)
    {
        transitionAnim.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.2f);
        
        SceneManager.LoadScene(levelIndex);
    }
    
    IEnumerator transition(string name)
    {
        transitionAnim.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.2f);
        
        SceneManager.LoadScene(name);
    }


    public IEnumerator delay(float length, Action functionToDelay)
    {
        yield return new WaitForSeconds(length);
        functionToDelay();
    }
}
