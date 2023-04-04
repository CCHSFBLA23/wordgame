using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Values")]
    private GameObject[] popUpObjects;
    public KeyCode[] moveKeys;
    private KeyCode[] undoKey = { KeyCode.Z };
    private KeyCode[] spaceKey = { KeyCode.Space };
    public GameObject popUpBase;
    public Vector2 position;
    public TutPopUp[] popUps;
    private int index;
    private Button resetButton;
    private Button pauseButton;

    private void Start()
    {
        index = 0;
        popUpObjects = new GameObject[popUps.Length];
        for (int i = 0; i < popUpObjects.Length; i++)
        {
            GameObject g = Instantiate(popUpBase, new Vector3(transform.position.x, 22, 0), Quaternion.identity, parent: transform);
            popUpObjects[i] = g;
            g.GetComponent<TextMeshProUGUI>().text = popUps[i].prompt;

            if (i > 0)
            {
                g.SetActive(false);
            }
        }

        pauseButton = GameObject.FindGameObjectWithTag("PauseButton").GetComponent<Button>();
        pauseButton.onClick.AddListener(() => { if (index == 2) { index += 1; ShowNextPrompt(); } });

        resetButton = GameObject.FindGameObjectWithTag("ResetButton").GetComponent<Button>();
        resetButton.onClick.AddListener(() => { if (index == 3) { index += 1; ShowNextPrompt(); } });
    }

    private void Update()
    {
        if (index == 0 && KeyPressed(moveKeys))
        {
            index += 1;
            ShowNextPrompt();
        }
        else if (index == 1 && KeyPressed(undoKey))
        {
            index += 1;
            ShowNextPrompt();
        }
        else if (index == 4 && KeyPressed(spaceKey))
        {
            index += 1;
            ShowNextPrompt();
        }
    }

    void ShowNextPrompt()
    {
        popUpObjects[index - 1].SetActive(false);
        popUpObjects[index].SetActive(true);
    }

    bool KeyPressed(KeyCode[] keys)
    {
        bool result = false;
        for (int i = 0; i < keys.Length; i++) {
            if (Input.GetKeyDown(keys[i]))
            {
                result = true;
                break;
            }
        }
        return result;
    }
}
