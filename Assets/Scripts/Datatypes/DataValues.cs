using System.Collections.Generic;

namespace DataTypes
{
    public enum SeedList { Hobbies, FirstNames, LastNames, Nicknames }
    public enum PersonalityType { }
    public enum Emotion { Anxious, Concerned, Neutral, Optimistic, Enthusiastic }
    public enum Task { Resting, Eating, Thinking, Meeting, Working }
    public enum Relation { Partner, Child, Parent, Grandparent, Sibling }
    public enum Incident { }

    public class DataValues
    {
        //DataValues returns a value from a given list stored in the game folders
        private bool loaded = false;
        private List<string> loadedHobbies;
        private List<string> loadedFirstNames;
        private List<string> loadedLastNames;
        private List<string> loadedNicknames;

        /// <summary>
        /// Returns a string from a given list at the provided ID
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetStringFromList(SeedList list, int id)
        {
            return "";
        }

        public void LoadLists()
        {
            if (!loaded)
            {
                //Load from files into our lists
            }
        }
    }
}