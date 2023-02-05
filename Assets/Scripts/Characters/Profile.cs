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
        public float restDropMultiplier { get; private set; } = 1f;
        public float foodDropMultiplier { get; private set; } = 1f;
        public float inspirationDropMultiplier { get; private set; } = 1f;
        public float socialDropMultiplier { get; private set; } = 1f;

        //How much empathy is needed to see this characters moodlets
        public float empathyBarrierMultiplier { get; private set; } = 1f;
        public float moodImpactMultiplier { get; private set; } = 1f;

        //How early or late does the character start and end the day
        public int dayStartMod { get; private set; } = 0;
        public int dayEndMod { get; private set; } = 0;

        //Multipliers for what tasks thiss character particularly excels at
        public float velocityMultiplier { get; private set; } = 1f;
        public float buglocityMultiplier { get; private set; } = 1f;
        public float burnoutMultiplier { get; private set; } = 1f;
        public float likesProgramming { get; private set; } = 1f;
        public float likesArt { get; private set; } = 1f;
        public float likesAudio { get; private set; } = 1f;
        public float likesWriting { get; private set; } = 1f;
        public float likesDesign { get; private set; } = 1f;

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
                restDropMultiplier *= t.restDropMultiplier;
                foodDropMultiplier *= t.foodDropMultiplier;
                inspirationDropMultiplier *= t.inspirationDropMultiplier;
                socialDropMultiplier *= t.socialnDropMultiplier;

                empathyBarrierMultiplier *= t.empathyBarrierMultiplier;
                moodImpactMultiplier *= t.moodImpactMultiplier;

                dayStartMod += t.dayStartMod;
                dayEndMod += t.dayEndMod;

                velocityMultiplier *= t.velocityMultiplier;
                buglocityMultiplier *= t.buglocityMultiplier;
                burnoutMultiplier *= t.burnoutMultiplier;
                likesProgramming *= t.likesProgramming;
                likesArt *= t.likesArt;
                likesAudio *= t.likesAudio;
                likesWriting *= t.likesWriting;
                likesDesign *= t.likesDesign;
            }
        }
    }
}