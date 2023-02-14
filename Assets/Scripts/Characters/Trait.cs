using UnityEngine;

namespace Devlike.Characters
{
    [CreateAssetMenu(fileName = "TraitType", menuName = "Devlike/Trait")]
    public class Trait : ScriptableObject
    {
        [TextArea(5,20)]
        public string description;

        public ConfidenceLevel confidence;

        //Multipliers for needs
        public int restDropMultiplier = 1;
        public int foodDropMultiplier = 1;
        public int inspirationDropMultiplier = 1;
        public int socialDropMultiplier = 1;

        //How much empathy is needed to see this characters moodlets
        public int empathyBarrierMultiplier = 1;
        public int moodImpactMultiplier = 1;
        public int baseMood = 1;

        //How early or late does the character start and end the day
        public int dayStartMultiplier = 1;
        public int dayEndMultiplier = 1;

        //Multipliers for what tasks thiss character particularly excels at
        public int velocityMultiplier = 1;
        public int bugChanceMultiplier = 1;
        public int burnoutMultiplier = 1;
    }
}