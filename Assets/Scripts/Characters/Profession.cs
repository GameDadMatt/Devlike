using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    [CreateAssetMenu(fileName = "ProfessionType", menuName = "Devlike/Profession")]
    public class Profession : ScriptableObject
    {
        public Tier experience = Tier.Lowest;
        public Tier art = Tier.Lowest;
        public Tier design = Tier.Lowest;
        public Tier engineering = Tier.Lowest;
    }
}
