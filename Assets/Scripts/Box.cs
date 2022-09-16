using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[ExecuteInEditMode]
public class Box : GridPosition
{
    //Mainly going to store box properties such as the type, color, letter, etc.


    [SerializeField] private BoxPreset preset;
    [SerializeField] private string letter; 
    private Color color = Color.white;
    private SpriteRenderer _spriteRenderer;
    private TMP_Text _text;

    [HideInInspector]public bool pushable = true;

    protected override void Start()
    {
        base.Start();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _text = GetComponentInChildren<TMP_Text>();
        UpdateVisualsAndProperties();
    }
    
    //2 do make the changes for the letters and color happen in the editor.
    protected override void Update()
    {
        base.Update();
        UpdateVisualsAndProperties();
    }

    private void UpdateVisualsAndProperties()
    {
        //Appearance
        _spriteRenderer.color = preset.color;
        _text.text = letter;
        
        pushable = preset.pushable;
    }
}
