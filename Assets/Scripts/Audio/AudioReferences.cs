using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Audio References", menuName = "ScriptableObjects/AudioReferences")]
public class AudioReferences : ScriptableObject
{ 
    public List<Sound> sounds;
}
