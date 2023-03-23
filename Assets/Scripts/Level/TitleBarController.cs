using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleBarController : MonoBehaviour
{
    public LevelHandler levelHandler;

    private void Start()
    {
        GetComponent<TMP_Text>().text = levelHandler.GetTitle();
    }
}
