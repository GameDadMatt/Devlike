using UnityEngine;

namespace Devlike.Characters
{
    /// <summary>
    /// Values for an individual character trait
    /// </summary>
    [CreateAssetMenu(fileName = "TraitType", menuName = "Devlike/Trait")]
    public class Trait : ScriptableObject
    {
        [TextArea(5,20)]
        public string description;

        //Multipliers for needs
        [Header("Lower Tier = Higher Return")]
        public Tier restDrop = Tier.Average;
        public Tier foodDrop = Tier.Average;
        public Tier inspirationDrop = Tier.Average;
        public Tier socialDrop = Tier.Average;

        public Tier empathyBarrier = Tier.Average;
        public Tier burnout = Tier.Average;
        public Tier bugChance = Tier.Average;

        [Header("Lower Tier = Lower Return")]
        public Tier confidence = Tier.Average;
        public Tier velocity = Tier.Average;
        public Tier moodImpact = Tier.Average;
        public Tier baseMood = Tier.Average;

        public Tier dayStart = Tier.Average;
        public Tier dayEnd = Tier.Average;
    }
}