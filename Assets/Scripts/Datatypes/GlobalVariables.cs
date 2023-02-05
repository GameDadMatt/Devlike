using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using DataTypes;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables value;

    //TIMING
    [Header("TIMING")]
    public bool paused = false;
    public float normalTickLength = 1f;
    public float slowTickLength = 3f;
    public float fastTickLength = 0.5f;
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
    public List<Trait> allTraits = new List<Trait>();

    public void Awake()
    {
        if(value == null)
        {
            value = this;
        }
    }

    public Trait GetTraitOfType(TraitType type)
    {
        foreach(Trait trait in allTraits)
        {
            if(trait.type == type)
            {
                return trait;
            }
        }
        Debug.LogError("Could not find trait of type " + type + ". Was it added to the All Traits list on GlobalVariables?");
        return null;
    }
}
