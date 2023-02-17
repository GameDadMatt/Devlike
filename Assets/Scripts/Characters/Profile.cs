using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    public class Profile
    {
        //Private Values
        private List<Relationship> relationships;

        //Public Values
        public int Seed { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Nickname { get; private set; }
        public string Hobby { get; private set; }
        public string Role { get; private set; }
        public Tier Experience { get; private set; } = Tier.Average;
        public Tier Confidence { get; private set; } = Tier.Average;

        public List<string> traitNames = new List<string>(); //Temporarily made available publically
        public List<Trait> traits = new List<Trait>();
        public string profName; //Temporarily made available publically
        public Profession profession;

        //Multipliers for needs
        public float RestDropMultiplier { get; private set; } = 1f;
        public float FoodDropMultiplier { get; private set; } = 1f;
        public float InspDropMultiplier { get; private set; } = 1f;
        public float SoclDropMultiplier { get; private set; } = 1f;

        //How much empathy is needed to see this characters moodlets
        public float EmpathyBarrierMultiplier { get; private set; } = 1f;
        //How much is this character's mood impacted by decisions & their own actions
        public float MoodImpactMultiplier { get; private set; } = 1f;
        //What is the base value for this characters mood
        public float BaseMood { get; private set; } = 0.5f;

        //How early or late does the character start and end the day
        public int WorkStartMod { get; private set; } = 0;
        public int WorkEndMod { get; private set; } = 0;

        //Multipliers for what tasks this character particularly excels at
        public float VelocityMultiplier { get; private set; } = 1f;
        public float BugChanceMultiplier { get; private set; } = 1f;
        public float BurnoutMultiplier { get; private set; } = 1f;
        public float Programming { get; private set; } = 1f;
        public float Art { get; private set; } = 1f;
        public float Audio { get; private set; } = 1f;
        public float Writing { get; private set; } = 1f;
        public float Design { get; private set; } = 1f;

        public void SetupProfile()
        {
            ApplyProfession(profName);

            foreach(string trait in traitNames)
            {
                ApplyTrait(trait);
            }
            Debug.Log("My traits are " + traitNames[0] + ", " + traitNames[1] + ", " + traitNames[2]);
        }

        private void ApplyProfession(string name)
        {
            Profession p = GlobalVariables.value.GetProfession(name);
            if (p != null)
            {
                profession = p;
                Experience = p.experience;
                Programming = p.ProgrammingBase;
                Art = p.ArtBase;
                Audio = p.AudioBase;
                Writing = p.WritingBase;
                Design = p.DesignBase;
            }
        }

        private void ApplyTrait(string name)
        {
            Trait t = GlobalVariables.value.GetTrait(name);
            if (t != null)
            {
                traits.Add(t);
                Confidence = ConfidenceAverage(t.confidence);

                RestDropMultiplier *= TraitValue(t.restDrop);
                FoodDropMultiplier *= TraitValue(t.foodDrop);
                InspDropMultiplier *= TraitValue(t.inspirationDrop);
                SoclDropMultiplier *= TraitValue(t.socialDrop);

                EmpathyBarrierMultiplier *= TraitValue(t.empathyBarrier);
                MoodImpactMultiplier *= TraitValue(t.moodImpact);
                BaseMood = TraitValue(t.baseMood);

                WorkStartMod += DayMod(t.dayStart);
                WorkEndMod += DayMod(t.dayEnd);

                VelocityMultiplier *= TraitValue(t.velocity);
                BugChanceMultiplier *= TraitValue(t.bugChance);
                BurnoutMultiplier *= TraitValue(t.burnout);
            }
        }

        private float TraitValue(Tier tier)
        {
            return GlobalVariables.value.TraitEffectMultiplier * (int)tier;
        }

        private int DayMod(Tier tier)
        {
            return GlobalVariables.value.DayModifierBase + (int)tier;
        }

        private int TierNum(Tier tier)
        {
            return (int)tier;
        }

        private Tier ConfidenceAverage(Tier tier)
        {
            int a = Mathf.CeilToInt((int)Confidence * (int)tier / 2);
            return (Tier)a;
        }
    }
}