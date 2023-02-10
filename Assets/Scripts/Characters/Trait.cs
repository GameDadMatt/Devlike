using UnityEngine;
using DataTypes;

namespace Characters
{
    [CreateAssetMenu(fileName = "TraitType", menuName = "Devlike/Trait")]
    public class Trait : ScriptableObject
    {
        public TraitType type;

        //Multipliers for needs
        public float restDropMultiplier = 1f;
        public float foodDropMultiplier = 1f;
        public float inspirationDropMultiplier = 1f;
        public float socialnDropMultiplier = 1f;

        //How much empathy is needed to see this characters moodlets
        public float empathyBarrierMultiplier = 1f;
        public float moodImpactMultiplier = 1f;
        public float baseMood = 0.5f;

        //How early or late does the character start and end the day
        public int dayStartMod = 0;
        public int dayEndMod = 0;

        //Multipliers for what tasks thiss character particularly excels at
        public float velocityMultiplier = 1f;
        public float bugChanceMultiplier = 1f;
        public float burnoutMultiplier = 1f;
        public float likesProgramming = 1f;
        public float likesArt = 1f;
        public float likesAudio = 1f;
        public float likesWriting = 1f;
        public float likesDesign = 1f;
    }
}