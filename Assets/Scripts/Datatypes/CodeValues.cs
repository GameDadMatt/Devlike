using System.Collections.Generic;
using System.IO;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Devlike.Characters;
using Devlike.Tasks;

namespace Devlike.Characters
{
    public enum SeedList { Hobbies, FirstNames, LastNames, Nicknames }
    public enum MoodletType { None, HasDrama, HasDialogue, BadMood, LowVelocity, HighVelocity, Overwhelmed, GoodMood, NoTask, CompletedTask, GeneratedBug }
    public enum DialogueType { None, Alignment, Crunching, Miscalculation, Complication, Clarification, Communication, Personal, Interpersonal, BadMood, GoodMood, LowVelocity, HighVelocity, Overwhelmed, NoTask, Casual }
    public enum RelationType { Partner, Child, Parent, Grandparent, Sibling }
    public enum CharacterState { Start, Active, End, Inactive }
    public enum DoingType { Rest, Food, Inspiration, Social, Work, Home, Idle }
    public enum TriggerEvent { None, Crunch, Alignment, Conflict  }

    /// <summary>
    /// Contains the tier and type to inform a character skill
    /// </summary>
    [System.Serializable]
    public class Skill
    {
        public TaskType type;
        public Tier tier;

        public Skill(TaskType type, Tier tier)
        {
            this.type = type;
            this.tier = tier;
        }
    }
}

namespace Devlike.Tasks
{
    public enum TaskType { Art, Engineering, Design }
    public enum TaskImportance { None, Bug, Required }
}

public enum GameState { Paused, Ticking, Interacting }
public enum GameSpeed { Normal, Fast }
public enum Tier { Lowest, Low, Average, High, Highest }
public enum Day { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }

/// <summary>
/// Tracks what a character is doing and when they need to refill the stat
/// </summary>
[System.Serializable]
public class DoingTracker
{
    public DoingType type;
    public float curValue;
    public float threshold;

    public DoingTracker(DoingType type, float curValue, float threshold)
    {
        this.type = type;
        this.curValue = curValue;
        this.threshold = threshold;
    }

    public bool NearThreshold
    {
        get
        {
            return curValue < (threshold + 0.1f);
        }
    }
}

/// <summary>
/// This helps us to share and keep track of chance weights much more easily
/// </summary>
[System.Serializable]
public class ChanceWeights
{
    public float art;
    public float des;
    public float eng;
    public float Total { get { return art + des + eng; } }

    public ChanceWeights(float art, float des, float eng)
    {
        this.art = art;
        this.des = des;
        this.eng = eng;
    }

    public TaskType RandomFromWeights
    {
        get
        {
            float r = Random.Range(0, Total);
            return WithinWeight(r);
        }
    }

    public TaskType WithinWeight(float val)
    {
        if(val < art)
        {
            return TaskType.Art;
        }
        else if (val < art + des)
        {
            return TaskType.Design;
        }
        else
        {
            return TaskType.Engineering;
        }
    }

    public float ArtWeightPercent
    {
        get
        {
            return art / Total;
        }
    }

    public float DesWeightPercent
    {
        get
        {
            return des / Total;
        }
    }

    public float EngWeightPercent
    {
        get
        {
            return eng / Total;
        }
    }
}

//Used to keep track of values that have a threshold point
[System.Serializable]
public class Threshold
{
    public float Point { get; private set; } = 0f;
    public float Value { get; set; } = 0f;

    public Threshold(float point)
    {
        Point = point;
    }

    public bool OverThreshold
    {
        get
        {
            return Value > Point;
        }
    }

    public bool UnderThreshold
    {
        get
        {
            return Value < Point;
        }
    }

    public override string ToString()
    {
        return Value + " / " + Point;
    }
}

//This allows our needs to be used in the BehaviorDesigner
[System.Serializable]
public class SharedDoingTracker : SharedVariable<DoingTracker>
{
    public static implicit operator SharedDoingTracker(DoingTracker value) { return new SharedDoingTracker { Value = value }; }
}

//This allows our doing state to be used in the BehaviourDesigner
[System.Serializable]
public class SharedDoingType : SharedVariable<DoingType>
{
    public static implicit operator SharedDoingType(DoingType value) { return new SharedDoingType { Value = value }; }
}

//This allows our character state to be used in the BehaviourDesigner
[System.Serializable]
public class SharedCharacterState : SharedVariable<CharacterState>
{
    public static implicit operator SharedCharacterState(CharacterState value) { return new SharedCharacterState { Value = value }; }
}

/// <summary>
/// Loaded values that can be stored and accessed
/// </summary>
public static class CodeValues
{
    //DataValues returns a value from a given list stored in the game folders
    private static string path = "Assets/Data/";
    /*public static List<string> Hobbies;
    public static List<string> FirstNames;
    public static List<string> LastNames;
    public static List<string> Nicknames;*/
    public static string[] Hobbies;
    public static string[] FirstNames;
    public static string[] LastNames;
    public static string[] Nicknames;

    /// <summary>
    /// Returns a string from a given list
    /// </summary>
    /// <param name="list"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static List<string> GetStringsFromFile(string filename)
    {
        List<string> strings = new List<string>();
        StreamReader stream = new StreamReader(path + filename);
        while (!stream.EndOfStream)
        {
            strings.Add(stream.ReadLine());
        }
        return strings;
    }

    private static string[] GetStringsFromTextAsset(TextAsset asset)
    {
        string[] strings = asset.text.Split('\n');
        if (strings.Length <= 1)
        {
            strings = asset.text.Split(System.Environment.NewLine);

            if (strings.Length <= 1)
            {
                Debug.LogError("Failed to split loaded strings from TextAsset " + asset.name);
            }
        }
        return strings;
    }

    public static void LoadLists(TextAsset hb, TextAsset fn, TextAsset ln, TextAsset nn)
    {
        /*Hobbies = GetStringsFromFile("Hobbies.txt");
        FirstNames = GetStringsFromFile("FirstNames.txt");
        LastNames = GetStringsFromFile("LastNames.txt");
        Nicknames = GetStringsFromFile("Nicknames.txt");*/

        Hobbies = GetStringsFromTextAsset(hb);
        FirstNames = GetStringsFromTextAsset(fn);
        LastNames = GetStringsFromTextAsset(ln);
        Nicknames = GetStringsFromTextAsset(nn);
    }
}