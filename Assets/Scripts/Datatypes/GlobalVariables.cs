using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Characters;
using DataTypes;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables value;

    //TIMING
    [Header("TIMING")]
    [SerializeField]
    private float tickLength = 0.5f;
    [SerializeField]
    private int workStartTick = 6;
    [SerializeField]
    private int workEndTick = 58;
    [SerializeField]
    private int dayEndTick = 66;
    [SerializeField]
    private int timeUnitTicks = 6;

    public float TickLength { get { return tickLength; } private set { tickLength = value; } }
    public int WorkStartTick { get { return workStartTick; } private set { workStartTick = value; } }
    public int WorkEndTick { get { return workEndTick; } private set { workEndTick = value; } }
    public int DayEndTick { get { return dayEndTick; } private set { dayEndTick = value; } }
    private int WorkTicks { get { return workEndTick - workStartTick; } }

    //CHARACTER
    [Header("CHARACTERS")]
    [SerializeField]
    private float foodBreaksPerDay = 1f;
    [SerializeField]
    private float restBreaksPerDay = 4f;
    [SerializeField]
    private float inspirationBreaksPerDay = 3f;
    [SerializeField]
    private float socialBreaksPerDay = 3f;
    [SerializeField]
    private float moodImpactDays = 2.5f;
    public List<Trait> allTraits = new List<Trait>();

    public float BaseFoodBurn { get { return foodBreaksPerDay / WorkTicks;  } }
    public float BaseRestBurn { get { return restBreaksPerDay / WorkTicks; } }
    public float BaseInspBurn { get { return inspirationBreaksPerDay / WorkTicks; } }
    public float BaseSoclBurn { get { return socialBreaksPerDay / WorkTicks; } }
    public float MoodImpactBurn { get { return moodImpactDays / WorkTicks; } }

    //TASKS
    [SerializeField]
    private float tasksPerDay = 3f;
    [SerializeField]
    private float bugChance = 0.1f;
    public float BaseVelocity { get { return tasksPerDay / WorkTicks; } }
    public float BaseBugChance { get { return bugChance; } }

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
