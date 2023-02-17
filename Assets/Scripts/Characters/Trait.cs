using UnityEngine;

namespace Devlike.Characters
{
    [CreateAssetMenu(fileName = "TraitType", menuName = "Devlike/Trait")]
    public class Trait : ScriptableObject
    {
        [TextArea(5,20)]
        public string description;

        public Tier confidence;

        //Multipliers for needs
        public Tier restDrop = Tier.Average;
        public Tier foodDrop = Tier.Average;
        public Tier inspirationDrop = Tier.Average;
        public Tier socialDrop = Tier.Average;

        //How much empathy is needed to see this characters moodlets
        public Tier empathyBarrier = Tier.Average;
        public Tier moodImpact = Tier.Average;
        public Tier baseMood = Tier.Average;

        //How early or late does the character start and end the day
        public Tier dayStart = Tier.Average;
        public Tier dayEnd = Tier.Average;

        //Multipliers for what tasks thiss character particularly excels at
        public Tier velocity = Tier.Average;
        public Tier bugChance = Tier.Average;
        public Tier burnout = Tier.Average;
    }
}