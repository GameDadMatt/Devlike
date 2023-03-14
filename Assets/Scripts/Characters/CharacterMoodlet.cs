using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    [CreateAssetMenu(fileName = "Moodlet", menuName = "Devlike/Moodlet")]
    public class CharacterMoodlet : ScriptableObject
    {
        public Sprite sprite;
        public MoodletType type;
    }
}