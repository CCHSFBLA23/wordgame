using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class LevelIntro : MonoBehaviour
{
    public GameObject fadePanel;
    public string pressAnyKeyPrompt;
    
    private void Start()
    {
        GameObject levelManager = GameObject.FindGameObjectWithTag("levelManager");
        LevelHandler levelHandler = levelManager.GetComponent<LevelHandler>();
        fadePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = levelHandler.GetTitle() + Environment.NewLine + Environment.NewLine + pressAnyKeyPrompt;
    }
}