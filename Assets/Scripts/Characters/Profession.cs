using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    [CreateAssetMenu(fileName = "ProfessionType", menuName = "Devlike/Profession")]
    public class Profession : ScriptableObject
    {
        public Tier experience = Tier.Lowest;
        public Tier programming = Tier.Lowest;
        public Tier art = Tier.Lowest;
        public Tier audio = Tier.Lowest;
        public Tier writing = Tier.Lowest;
        public Tier design = Tier.Lowest;
    }
}
