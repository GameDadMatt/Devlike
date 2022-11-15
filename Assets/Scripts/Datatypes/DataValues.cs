using System.Collections.Generic;
using UnityEngine;

namespace DataTypes
{
    public enum SeedList { Hobbies, FirstNames, LastNames, Nicknames }
    public enum PersonalityType { Default }
    public enum EmotionType { Anxious, Concerned, Neutral, Optimistic, Enthusiastic }
    public enum TaskType { Resting, Eating, Thinking, Meeting, Working }
    public enum RelationType { Partner, Child, Parent, Grandparent, Sibling }
    public enum IncidentType { }

    public static class DataValues
    {
        //DataValues returns a value from a given list stored in the game folders
        private static bool loaded = false;
        private static List<string> loadedHobbies;
        private static List<string> loadedFirstNames;
        private static List<string> loadedLastNames;
        private static List<string> loadedNicknames;
        private static List<Personality> personalities;

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

        public static Personality GetPersonalityOfType(PersonalityType type)
        {
            foreach(Personality personality in personalities)
            {
                if(personality.personalityType == type)
                {
                    return personality;
                }
            }
            Debug.LogError("Failed to find personality type of " + type);
            return null;
        }

        public static void LoadLists()
        {
            if (!loaded)
            {
                //Load from files into our lists
            }
        }
    }

    public class Need
    {
        public TaskType Type { get; private set; }
        public float CurrentValue { get; private set; }
        public float Threshold { get; private set; }

        public Need(TaskType type, float currentValue, float threshold)
        {
            Type = type;
            CurrentValue = currentValue;
            Threshold = threshold;
        }

        public Need(TaskType type, float threshold)
        {
            Type = type;
            CurrentValue = 1f;
            Threshold = threshold;
        }

        public float ReduceNeed(float rate)
        {
            CurrentValue -= rate;
            return CurrentValue;
        }

        public float RestoreNeed(float rate)
        {
            CurrentValue += rate;
            return CurrentValue;
        }

        public float FullRestoreNeed()
        {
            CurrentValue = 1f;
            return CurrentValue;
        }

        public bool RequiresMaintenance
        {
            get
            {
                return CurrentValue > Threshold;
            }
        }
    }
}