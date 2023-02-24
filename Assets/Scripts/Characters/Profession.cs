using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    [CreateAssetMenu(fileName = "ProfessionType", menuName = "Devlike/Profession")]
    public class Profession : ScriptableObject
    {
        public Tier experience = Tier.Lowest;
        public Skill primarySkill;
        public Skill secondarySkill;
    }
}
