using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    /// <summary>
    /// The data for a character's profession
    /// </summary>
    [CreateAssetMenu(fileName = "ProfessionType", menuName = "Devlike/Profession")]
    public class Profession : ScriptableObject
    {
        public Skill primarySkill;
        public Skill secondarySkill;
    }
}
