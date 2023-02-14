using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    [CreateAssetMenu(fileName = "ProfessionType", menuName = "Devlike/Profession")]
    public class Profession : ScriptableObject
    {
        public ExperienceLevel experience;
        public float ProgrammingBase = 1f;
        public float ArtBase = 1f;
        public float AudioBase = 1f;
        public float WritingBase = 1f;
        public float DesignBase = 1f;
    }
}
