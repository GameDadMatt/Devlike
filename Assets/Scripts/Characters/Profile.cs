﻿using System.Collections.Generic;
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

        public Profile(GlobalCharacter gCharacter, GlobalTime gTime, string fname, string lname, string nname, string hobby, Tier experience, Profession profession, List<Trait> traits, Color color)
        {
            FirstName = fname;
            LastName = lname;
            Nickname = nname;
            Hobby = hobby;
            Experience = experience;
            Color = color;
            ApplyProfession(profession);

            foreach(Trait trait in traits)
            {
                ApplyTrait(gCharacter, gTime, trait);
            }

            //Average out the traits
            AverageTraits(gCharacter.TotalTraits);
        }

        private void ApplyProfession(Profession p)
        {
            Profession = p;
            ApplySkill(p.primarySkill);
            ApplySkill(p.secondarySkill);

            //Average out the skills
            Art /= 2;
            Engineering /= 2;
            Design /= 2;
        }

        private void ApplySkill(Skill skill)
        {
            switch (skill.type)
            {
                case TaskType.Art:
                    Art += TierSkillMultiplier(skill.tier);
                    break;
                case TaskType.Engineering:
                    Engineering += TierSkillMultiplier(skill.tier);
                    break;
                case TaskType.Design:
                    Design += TierSkillMultiplier(skill.tier);
                    break;
            }
        }

        private void ApplyTrait(GlobalCharacter gCharacter, GlobalTime gTime, Trait trait)
        {
            TraitNames.Add(trait.name);
            Confidence = ConfidenceAverage(trait.confidence);

            //Low Tier = High Return
            RestDropMultiplier += TierHighToLow(trait.restDrop);
            FoodDropMultiplier += TierHighToLow(trait.foodDrop);
            InspDropMultiplier += TierHighToLow(trait.inspirationDrop);
            SoclDropMultiplier += TierHighToLow(trait.socialDrop);
            AlignDriftMultiplier += TierHighToLow(trait.alignment);

            EmpathyBarrierMultiplier += TierHighToLow(trait.empathyBarrier);
            BugChanceMultiplier += TierHighToLow(trait.bugChance);

            BurnoutMultiplier += TierHighToLow(trait.burnout);
            MoodImpactMultiplier += TierHighToLow(trait.moodImpact);

            //Low Tier = Low Return
            VelocityMultiplier += TierLowToHigh(trait.velocity);
            BaseMood += TierLowToHigh(trait.baseMood);
            CrunchThreshold += TierLowToHigh(trait.crunchThreshold);

            WorkStartMod += TierDayMod(gCharacter, gTime.TicksPerHour, trait.dayStart);
            WorkEndMod += TierDayMod(gCharacter, gTime.TicksPerHour, trait.dayEnd);

            NaturalAlignment += AlignmentCalc(trait.alignment, Experience);
        }

        private void AverageTraits(int count)
        {
            //Low Tier = High Return
            RestDropMultiplier /= (float)count;
            FoodDropMultiplier /= (float)count;
            InspDropMultiplier /= (float)count;
            SoclDropMultiplier /= (float)count;
            AlignDriftMultiplier /= (float)count;

            EmpathyBarrierMultiplier /= (float)count;
            BugChanceMultiplier /= (float)count;

            BurnoutMultiplier /= (float)count;
            MoodImpactMultiplier /= (float)count;

            WorkStartMod /= count;
            WorkEndMod /= count;

            //Low Tier = Low Return
            VelocityMultiplier /= (float)count;
            BaseMood /= (float)count;
            CrunchThreshold /= (float)count;
            NaturalAlignment /= (float)count;
        }

        /// <summary>
        /// Low to High assumes that a lower tier returns a lower float
        /// </summary>
        /// <param name="tier"></param>
        /// <returns></returns>
        private float TierLowToHigh(Tier tier)
        {
            switch (tier)
            {
                case Tier.Lowest:
                    return 0.5f;
                case Tier.Low:
                    return 0.8f;
                case Tier.Average:
                    return 1f;
                case Tier.High:
                    return 1.2f;
                case Tier.Highest:
                    return 1.5f;
            }
            Debug.LogError("Failed to find tier " + tier);
            return 1f;
        }

        /// <summary>
        /// High to Low assumes that a lower tier returns a higher float
        /// </summary>
        /// <param name="tier"></param>
        /// <returns></returns>
        private float TierHighToLow(Tier tier)
        {
            switch (tier)
            {
                case Tier.Lowest:
                    return 1.5f;
                case Tier.Low:
                    return 1.2f;
                case Tier.Average:
                    return 1f;
                case Tier.High:
                    return 0.8f;
                case Tier.Highest:
                    return 0.5f;
            }
            Debug.LogError("Failed to find tier " + tier);
            return 1f;
        }

        private float TierSkillMultiplier(Tier tier)
        {
            switch (tier)
            {
                case Tier.Lowest:
                    return 0.05f;
                case Tier.Low:
                    return 0.1f;
                case Tier.Average:
                    return 0.5f;
                case Tier.High:
                    return 0.8f;
                case Tier.Highest:
                    return 1f;
            }
            Debug.LogError("Failed to find tier " + tier);
            return 1f;
        }

        private int TierDayMod(GlobalCharacter character, int ticksPerHour, Tier tier)
        {
            return (character.DayHoursModifier + (int)tier) * ticksPerHour;
        }

        private Tier ConfidenceAverage(Tier tier)
        {
            int a = Mathf.CeilToInt((int)Confidence * (int)tier / 2);
            return (Tier)a;
        }

        //Factors experience into the calculation
        private float AlignmentCalc(Tier alignment, Tier experience)
        {
            return (TierLowToHigh(alignment) + TierLowToHigh(experience)) / 2f;
        }
    }
}