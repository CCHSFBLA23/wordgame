using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxType", menuName = "ScriptableObjects/BoxPreset")]
public class BoxPreset : ScriptableObject
{
    public Color boxColor = Color.white;
    public Color textColor = Color.black;
    public bool pushable = true;
}
