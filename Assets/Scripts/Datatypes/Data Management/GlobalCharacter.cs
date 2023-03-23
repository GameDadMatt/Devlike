using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;

namespace Devlike
{
    [CreateAssetMenu(fileName = "CharacterProperties", menuName = "Devlike/Properties/Character")]
    public class GlobalCharacter : GlobalObject
    {
        [Header("BASE VALUES")]
        [SerializeField]
        private int totalTraits;
        [SerializeField]
        private int moodletDisplayTicks;
        [SerializeField]
        private float lowToHighBaseValue, highToLowBaseValue;
        [SerializeField]
        private int dayHoursModifier;

        [Header("NEEDS")]
        [SerializeField]
        private float needThreshold;
        [SerializeField]
        private float foodBreaksPerDay, restBreaksPerDay, inspirationBreaksPerDay, socialBreaksPerDay, alignmentBase, alignmentDriftPerDay;
        [SerializeField]
        private float moodImpactDays, moodImpactMax, moodImpactMin;

        [Header("OTHER")]
        [SerializeField]
        private Gradient characterColours;
        [SerializeField]
        private List<Trait> traits = new List<Trait>();
        [SerializeField]
        private List<Profession> professions = new List<Profession>();
        [SerializeField]
        private List<CharacterMoodlet> moodlets = new List<CharacterMoodlet>();

        //Returns
        public int TotalTraits { get => totalTraits; }
        public float MoodletDisplayTicks { get => moodletDisplayTicks; }
        public float LowToHighBaseValue { get => lowToHighBaseValue; }
        public float HighToLowBaseValue { get => highToLowBaseValue; }
        public int DayHoursModifier { get => dayHoursModifier; }

        public float NeedThreshold { get => needThreshold; }
        public float FoodBreaksPerDay { get => foodBreaksPerDay; }
        public float RestBreaksPerDay { get => restBreaksPerDay; }
        public float InspBreaksPerDay { get => inspirationBreaksPerDay; }
        public float SoclBreaksPerDay { get => socialBreaksPerDay; }
        public float AlignmentBase { get => alignmentBase; }
        public float AlignmentDriftPerDay { get => alignmentDriftPerDay; }
        public float MoodImpactDuration { get => moodImpactDays; }
        public float MoodImpactMin { get => moodImpactMin; }
        public float MoodImpactMax { get => moodImpactMax; }

        public Gradient CharacterColours { get => characterColours; }
        public List<Trait> Traits { get => traits; }
        public List<Profession> Professions { get => professions; }
        public List<CharacterMoodlet> Moodlets { get => moodlets; }

        public Trait GetTrait(string name)
        {
            foreach (Trait trait in traits)
            {
                if (trait.name == name)
                {
                    return trait;
                }
            }
            Debug.LogError("Could not find a trait by the name of " + name + ". Was it added to the All Traits list on GlobalVariables?");
            return null;
        }

        public Profession GetProfession(string name)
        {
            foreach (Profession prof in professions)
            {
                if (prof.name == name)
                {
                    return prof;
                }
            }
            Debug.LogError("Could not find a profession by the name of " + name + ". Was it added to the All Professions list on GlobalVariables?");
            return null;
        }

        public CharacterMoodlet GetMoodlet(MoodletType type)
        {
            foreach (CharacterMoodlet moodlet in moodlets)
            {
                if (type == moodlet.type)
                {
                    return moodlet;
                }
            }

            Debug.LogError("Unable to find moodlet of type " + type);
            return null;
        }
    }
}
