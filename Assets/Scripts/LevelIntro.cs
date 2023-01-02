using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class LevelIntro : MonoBehaviour
{
    public GameObject fade;
    private GameObject levelManager;
    
    private void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("levelManager");
        LevelHandler levelHandler = levelManager.GetComponent<LevelHandler>();
        fade.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = levelHandler.GetTitle();
    }

    public void OnAnyKey(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Here");
            fade.transform.GetChild(0).GetComponent<Animator>().SetTrigger("FadeIn");
        }
    }
}