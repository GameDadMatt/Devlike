using System.Collections.Generic;
using BehaviorDesigner.Runtime;

public enum GameState { Paused, Ticking, Interacting }
public enum SeedList { Hobbies, FirstNames, LastNames, Nicknames }
public enum TraitType { Average, EarlyBird, NightOwl, Caffienated, Insomniac, Wallflower, SocialButterfly, Imaginative, Forgetful, Hungry, Workaholic, Slacker, Hacker, Furry, Cool, FanfictionWriter, Cosplayer, Thorough, Hasty, AllRounder }
public enum EmotionType { Anxious, Concerned, Neutral, Optimistic, Enthusiastic }
public enum DoingType { Rest, Food, Inspiration, Social, Work, Home }
public enum TaskType { Art, Code, Audio, Design, Writing }
public enum TaskImportance { None, Bug, Required }
public enum RelationType { Partner, Child, Parent, Grandparent, Sibling }
public enum IncidentType { }
public enum CharacterState { Start, Active, End, Inactive }
public enum DialogueType { Casual, Work, Personal }
public enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }

//This custom class tracks a given need
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

public static class DataValues
{
    //DataValues returns a value from a given list stored in the game folders
    private static bool loaded = false;
    private static List<string> loadedHobbies;
    private static List<string> loadedFirstNames;
    private static List<string> loadedLastNames;
    private static List<string> loadedNicknames;

    /// <summary>
    /// Returns a string from a given list at the provided ID
    /// </summary>
    /// <param name="list"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetStringFromList(SeedList list, int id)
    {
        return "";
    }

    public static void LoadLists()
    {
        if (!loaded)
        {
            //Load from files into our lists
        }
    }
}