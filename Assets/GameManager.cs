using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float tickRate = 4f;
    private float curTick = 0f;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        curTick += Time.deltaTime;
        if(curTick >= tickRate)
        {
            curTick -= tickRate;
            Tick();
        }
    }

    public event Action OnTick;
    public void Tick()
    {
        OnTick?.Invoke();
    }
}
