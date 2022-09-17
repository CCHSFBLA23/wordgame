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
    public string letter; 
    private Color color = Color.white;
    private SpriteRenderer _spriteRenderer;
    private TMP_Text _text;

    [HideInInspector]public bool pushable = true;
    [HideInInspector]public bool pullable = false;
    [HideInInspector]public bool sticky = false;
    [HideInInspector]public bool weak = false; 
    [HideInInspector]public bool falling = false;
    [HideInInspector]public bool moving = false;
    [HideInInspector]public bool linked = false;


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
        _spriteRenderer.color = preset.boxColor;
        _text.text = letter;
        _text.color = preset.textColor;
        
        pushable = preset.pushable;
        pullable = preset.pullable;
        weak = preset.weak;
        sticky = preset.sticky;
        falling = preset.falling;
        moving = preset.moving;
        linked = preset.linked;

    }
}
