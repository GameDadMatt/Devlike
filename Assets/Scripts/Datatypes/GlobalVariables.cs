using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables value;

    //TIMING
    [Header("TIMING")]
    public float tickLength = 1f;
    public int dayTickLength = 60;
    public int dayStartTick = 5;
    public int dayEndTick = 55;
    public int DayTickLength { get { return dayTickLength; } private set { dayTickLength = value; } }
    public int DayStartTick { get { return dayStartTick; } private set { dayStartTick = value; } }
    public int DayEndTick { get { return dayEndTick; } private set { dayEndTick = value; } }

    //CHARACTER
    [Header("CHARACTERS")]
    public float baseFoodBurnTime = 0.0f;
    public float baseRestBurnTime = 0.0f;
    public float baseInspirationBurnTime = 0.0f;
    public float baseSocialBurnTime = 0.0f;

    public void Awake()
    {
        if(value == null)
        {
            value = this;
        }
    }
}
