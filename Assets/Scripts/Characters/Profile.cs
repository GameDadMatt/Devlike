using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;

namespace Devlike.Characters
{
    /// <summary>
    /// The profile for an individual character
    /// </summary>
    public class Profile
    {
        //Private Values
        private List<Relationship> relationships;

        //RANDOMLY GENERATED
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string FullNameAndAlias { get { return FirstName + " \"" + Nickname + "\" " + LastName; } }
        public string Nickname { get; private set; }
        public string Hobby { get; private set; }
        public Color Color { get; private set; } = Color.black;

        //TRAITS & PROFESSION
        public List<string> TraitNames { get; private set; } = new List<string>();
        public Profession Profession { get; private set; }
        public Tier Confidence { get; private set; } = Tier.Average;
        public Tier Experience { get; private set; } = Tier.Average;

        //Multipliers for needs
        public float RestDropMultiplier { get; private set; } = 1f;
        public float FoodDropMultiplier { get; private set; } = 1f;
        public float InspDropMultiplier { get; private set; } = 1f;
        public float SoclDropMultiplier { get; private set; } = 1f;
        public float AlignDriftMultiplier { get; private set; } = 1f;

        //How much empathy is needed to see this characters moodlets
        public float EmpathyBarrierMultiplier { get; private set; } = 1f;
        //How much is this character's mood impacted by decisions & their own actions
        public float MoodImpactMultiplier { get; private set; } = 1f;
        //What is the base value for this characters mood
        public float BaseMood { get; private set; } = 0.5f; //Percentage
        //What is the alignment of this character normally
        public float NaturalAlignment { get; private set; } = 0f; //Percentage

        //What is this character's likeliness to crunch
        public float CrunchThreshold { get; private set; } = 0f;

        //How early or late does the character start and end the day
        public int WorkStartMod { get; private set; } = 0;
        public int WorkEndMod { get; private set; } = 0;

        //Multipliers for what tasks this character particularly excels at
        public float VelocityMultiplier { get; private set; } = 1f;
        public float BugChanceMultiplier { get; private set; } = 1f;
        public float BurnoutMultiplier { get; private set; } = 1f;
        public float Art { get; private set; } = 0.2f;
        public float Engineering { get; private set; } = 0.2f;
        public float Design { get; private set; } = 0.2f;

        public Profile(GlobalCharacter character, int ticksPerHour, string fname, string lname, string nname, string hobby, Tier experience, Profession profession, List<Trait> traits, Color color)
        {
            FirstName = fname;
            LastName = lname;
            Nickname = nname;
            Hobby = hobby;
            Experience = experience;
            Color = color;
            ApplyProfession(character, profession);

            foreach(Trait trait in traits)
            {
                ApplyTrait(character, ticksPerHour, trait);
            }

            //WorkStart and WorkEnd are averaged across their modifiers
            WorkStartMod /= character.TotalTraits;
            WorkEndMod /= character.TotalTraits;
        }

        private void ApplyProfession(GlobalCharacter character, Profession p)
        {
            Profession = p;
            ApplySkill(character, p.primarySkill);
            ApplySkill(character, p.secondarySkill);
        }

        private void ApplySkill(GlobalCharacter character, Skill skill)
        {
            switch (skill.type)
            {
                case TaskType.Art:
                    Art *= TierLowToHigh(character, skill.tier);
                    break;
                case TaskType.Engineering:
                    Engineering *= TierLowToHigh(character, skill.tier);
                    break;
                case TaskType.Design:
                    Design *= TierLowToHigh(character, skill.tier);
                    break;
            }
        }

        private void ApplyTrait(GlobalCharacter character, int ticksPerHour, Trait trait)
        {
            TraitNames.Add(trait.name);
            Confidence = ConfidenceAverage(trait.confidence);

            //Low Tier = High Return
            RestDropMultiplier = TierHighToLow(character, trait.restDrop);
            FoodDropMultiplier = TierHighToLow(character, trait.foodDrop);
            InspDropMultiplier = TierHighToLow(character, trait.inspirationDrop);
            SoclDropMultiplier = TierHighToLow(character, trait.socialDrop);
            AlignDriftMultiplier = TierHighToLow(character, trait.alignment);

            CrunchThreshold = TierHighToLow(character, trait.crunchThreshold);
            EmpathyBarrierMultiplier = TierHighToLow(character, trait.empathyBarrier);

            BugChanceMultiplier = TierHighToLow(character, trait.bugChance);
            BurnoutMultiplier = TierHighToLow(character, trait.burnout);
            MoodImpactMultiplier = TierHighToLow(character, trait.moodImpact);

            //Low Tier = Low Return
            VelocityMultiplier = TierLowToHigh(character, trait.velocity);         

            WorkStartMod += TierDayMod(character, ticksPerHour, trait.dayStart);
            WorkEndMod += TierDayMod(character, ticksPerHour, trait.dayEnd);

            //Percentages
            BaseMood = TierPercentage(trait.baseMood);
            NaturalAlignment = AlignmentCalc(trait.alignment, Experience);
            CrunchThreshold = TierPercentage(trait.crunchThreshold);
        }

        /// <summary>
        /// Low to High assumes that a lower tier returns a lower float
        /// </summary>
        /// <param name="tier"></param>
        /// <returns></returns>
        private float TierLowToHigh(GlobalCharacter character, Tier tier)
        {
            return character.LowToHighBaseValue * ((int)tier + 1);
        }

        /// <summary>
        /// High to Low assumes that a lower tier returns a higher float
        /// </summary>
        /// <param name="tier"></param>
        /// <returns></returns>
        private float TierHighToLow(GlobalCharacter character, Tier tier)
        {
            return character.HighToLowBaseValue / ((int)tier + 1);
        }

        private int TierDayMod(GlobalCharacter character, int ticksPerHour, Tier tier)
        {
            return (character.DayHoursModifier + (int)tier) * ticksPerHour;
        }

        private float TierPercentage(Tier tier)
        {
            return (float)tier / (float)Tier.Highest;
        }

        private Tier ConfidenceAverage(Tier tier)
        {
            int a = Mathf.CeilToInt((int)Confidence * (int)tier / 2);
            return (Tier)a;
        }

        //Factors experience into the calculation
        private float AlignmentCalc(Tier alignment, Tier experience)
        {
            return (TierPercentage(alignment) + TierPercentage(experience)) / 2f;
        }
    }
}