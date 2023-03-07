using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Devlike.Characters;
using Devlike.Tasks;

/// <summary>
/// Variables to be tracked globally in the game
/// </summary>
public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables value;

    //TIMING
    [Header("TIMING")]
    [SerializeField]
    private float tickLength = 0.5f;
    [SerializeField]
    private float idleTickLength = 0.05f;
    [SerializeField]
    private int ticksPerHour = 4;
    [SerializeField]
    private int workStartHour = 9;
    [SerializeField]
    private int workEndHour = 17;
    [SerializeField]
    private int dayEndHour = 24;
    [SerializeField]
    private int timeUnitTicks = 6;

    public float TickLength { get { return tickLength; } private set { tickLength = value; } }
    public float IdleTickLength { get { return idleTickLength; } private set { idleTickLength = value; } }
    public int TicksPerHour { get { return ticksPerHour; } }
    public int WorkStartTick { get { return workStartHour * ticksPerHour; } }
    public int WorkEndTick { get { return workEndHour * ticksPerHour; } }
    public int DayEndTick { get { return dayEndHour * ticksPerHour; } }
    private int WorkTicks { get { return WorkEndTick - WorkStartTick; } }

    //CHARACTER
    [Header("CHARACTERS")]
    public int totalTraits = 3;
    [SerializeField]
    private float lowToHighBaseValue = 0.35f;
    [SerializeField]
    private float highToLowBaseValue = 3.0f;
    [SerializeField]
    private int dayModifierBase = -3;
    [SerializeField]
    private int traitTicksMultiplier = 2;
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
    public float moodImpactMax = 1.5f;
    public float moodImpactMin = 0.1f;
    public List<Trait> allTraits = new List<Trait>();
    public List<Profession> allProfessions = new List<Profession>();

    public float LowToHighBaseValue { get { return lowToHighBaseValue; } }
    public float HighToLowBaseValue { get { return highToLowBaseValue; } }
    public int DayModifierBase { get { return dayModifierBase; } }
    public float TraitDaysMultiplier { get { return traitTicksMultiplier; } }
    public float BaseFoodBurn { get { return foodBreaksPerDay / WorkTicks;  } }
    public float BaseRestBurn { get { return restBreaksPerDay / WorkTicks; } }
    public float BaseInspBurn { get { return inspirationBreaksPerDay / WorkTicks; } }
    public float BaseSoclBurn { get { return socialBreaksPerDay / WorkTicks; } }
    public float MoodImpactBurn { get { return moodImpactDays / WorkTicks; } }

    //TASKS
    [Header("TASKS")]
    [SerializeField]
    private int maxTaskPoints = 5;
    [SerializeField]
    private float pointsPerDay = 7f;
    [SerializeField]
    private float bugChance = 0.1f;
    public Color artTaskColor;
    public Color designTaskColor;
    public Color engineeringTaskColor;
    public Color taskBorderColor;
    public Color bugBorderColor;
    public int MaxTaskPoints { get { return maxTaskPoints; } }
    public float PointsPerDay { get { return pointsPerDay; } }
    /// <summary>
    /// The base value for how much of a task should be completed per tick
    /// </summary>
    public float BaseVelocity { get { return pointsPerDay / WorkTicks; } }
    public float BaseBugChance { get { return bugChance; } }

    //STUDIO
    [Header("STUDIO")]
    [SerializeField]
    private int studioSize = 0;
    public int StudioSize { get { return studioSize; } }
    [SerializeField]
    private ChanceWeights weights;
    public ChanceWeights Weights { get { return weights; } }
    [SerializeField]
    private float studioExperienceTarget = 2.5f;
    public float StudioExperienceTarget { get { return studioExperienceTarget; } }

    public void Awake()
    {
        if(value == null)
        {
            value = this;
        }

        //Load the contents of DataValues
        DataValues.LoadLists();
        Debug.Log("Loaded Lists");
    }

    public Trait GetTrait(string name)
    {
        foreach(Trait trait in allTraits)
        {
            if(trait.name == name)
            {
                return trait;
            }
        }
        Debug.LogError("Could not find a trait by the name of " + name + ". Was it added to the All Traits list on GlobalVariables?");
        return null;
    }

    public Profession GetProfession(string name)
    {
        foreach (Profession prof in allProfessions)
        {
            if(prof.name == name)
            {
                return prof;
            }
        }
        Debug.LogError("Could not find a profession by the name of " + name + ". Was it added to the All Professions list on GlobalVariables?");
        return null;
    }
}
