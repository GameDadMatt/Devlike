using System.Collections.Generic;
using DataTypes;
using UnityEngine;

namespace Characters
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
        public List<TraitType> Traits = new List<TraitType>(); //Temporarily made available publically
        //public List<TraitType> Traits { get; private set; }

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

        //Multipliers for what tasks thiss character particularly excels at
        public float VelocityMultiplier { get; private set; } = 1f;
        public float BugChanceMultiplier { get; private set; } = 1f;
        public float BurnoutMultiplier { get; private set; } = 1f;
        public float LikesProgramming { get; private set; } = 1f;
        public float LikesArt { get; private set; } = 1f;
        public float LikesAudio { get; private set; } = 1f;
        public float LikesWriting { get; private set; } = 1f;
        public float LikesDesign { get; private set; } = 1f;

        public void SetupProfile()
        {
            foreach(TraitType type in Traits)
            {
                ApplyTrait(type);
            }
            Debug.Log("My traits are " + Traits[0] + ", " + Traits[1] + ", " + Traits[2]);
        }

        public void ApplyTrait(TraitType type)
        {
            Trait t = GlobalVariables.value.GetTraitOfType(type);
            if (t != null)
            {
                RestDropMultiplier *= t.restDropMultiplier;
                FoodDropMultiplier *= t.foodDropMultiplier;
                InspDropMultiplier *= t.inspirationDropMultiplier;
                SoclDropMultiplier *= t.socialnDropMultiplier;

                EmpathyBarrierMultiplier *= t.empathyBarrierMultiplier;
                MoodImpactMultiplier *= t.moodImpactMultiplier;
                BaseMood = t.baseMood;

                WorkStartMod += t.dayStartMod;
                WorkEndMod += t.dayEndMod;

                VelocityMultiplier *= t.velocityMultiplier;
                BugChanceMultiplier *= t.bugChanceMultiplier;
                BurnoutMultiplier *= t.burnoutMultiplier;
                LikesProgramming *= t.likesProgramming;
                LikesArt *= t.likesArt;
                LikesAudio *= t.likesAudio;
                LikesWriting *= t.likesWriting;
                LikesDesign *= t.likesDesign;
            }
        }
    }
}