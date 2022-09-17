using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class GridPosition : MonoBehaviour
{
    //Used to move around the grid with lerp. Boxes inherit this class to be able to handle their properties
    //without calling a bunch of GetComponent<> calls

    public Vector2 current;
    public Vector2 target;
    [SerializeField] private float moveSpeed = 20f;


    private void Awake()
    {
        var position = transform.position;
        target = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        current = target;
    }

    protected virtual void Start()
    {
        
    }
    
    protected virtual void Update()
    {
        if (!Application.isPlaying) return;
            current = transform.position;
        if (Vector2.Distance(current, target) > 0.001f)
        {
            transform.position = Vector2.Lerp(current, target, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = target;
        }
    }
}
