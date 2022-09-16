using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxType", menuName = "ScriptableObjects/BoxPreset")]
public class BoxPreset : ScriptableObject
{
    public Color color = Color.white;
    public bool pushable = true;
}
