using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileScript : MonoBehaviour
{
    public Text text;
    public int number;
    public float speed = 2f;
    public float buildSpeed = 6f;
    public RectTransform rt;

    public Vector2 target = Vector2.zero;
    public Vector2 origin = Vector2.zero;
    private float along = 0f;

    private void Start()
    {
        //SmoothTo(rt.anchoredPosition);
    }

    public void SmoothTo(Vector2 pos)
    {
        origin = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y);
        target = pos;
        //rt.anchoredPosition = pos;
        along = 0f;
    }

    private void Update()
    {
        if (along <= 1f)
        {
            if (GameManager.gm.active)
                along += speed * Time.deltaTime;
            else
                along += buildSpeed * Time.deltaTime;
            if (along >= 1f)
            {
                rt.anchoredPosition = target;
                along = 1f;
            }
            else
            {
                rt.anchoredPosition = Vector2.Lerp(origin, target, along);
            }
        }
    }

    public void SetNumber(int i, int sub)
    {
        text.text = GameManager.subs[sub][i];
    }

    
}
