using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataTypes
{
    public interface ITraitMod
    {
        float traitMod { get; }
        void ApplyMod(float traitMod);
    }

    [CreateAssetMenu(fileName = "TraitType", menuName = "Devlike/Trait")]
    public class Trait : ScriptableObject
    {
        //More than 1 drops faster, less than 1 drops slower
        public float restDropMultiplier = 1f;
        public float foodDropMultiplier = 1f;
        public float inspirationDropMultiplier = 1f;
        public float velocityMultiplier = 1f;
        public float buglocityMultiplier = 1f;

        //These modify other stats
        public float dayStartMod = 0f;
        public float dayEndMod = 0f;
    }
}