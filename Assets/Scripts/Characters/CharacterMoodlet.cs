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
        [Tooltip("Whether or not this Moodlet is a response to the given DoingType")]
        public bool doingTypeResponse;
        public DoingType doingType;
    }
}