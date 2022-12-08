using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public Animator transitionAnim;

    public void LoadScene(int index)
    {
        StartCoroutine(transition(index));
    }

    public void LoadNextScene()
    {
        StartCoroutine(transition(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ReloadScene()
    {
        StartCoroutine(transition(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator transition(int levelIndex)
    {
        transitionAnim.SetTrigger("Start");

        yield return new WaitForSeconds(0.8f);

        SceneManager.LoadScene(levelIndex);
    }
}
