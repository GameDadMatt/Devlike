using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;

namespace Devlike
{
    [CreateAssetMenu(fileName = "Studio", menuName = "Devlike/Properties/Studio")]
    public class GlobalStudio : GlobalObject
    {
        [Header("STUDIO")]
        [SerializeField]
        private int studioSize; // = 0;
        [SerializeField]
        private ChanceWeights weights;
        [SerializeField]
        private float studioExperienceTarget; // = 2.5f;

        public int StudioSize { get => studioSize; }
        public ChanceWeights Weights { get => weights; }
        public float StudioExperienceTarget { get => studioExperienceTarget; }

        //CHARACTERS
        [Header("CHARACTERS")]
        private List<Character> characters = new List<Character>();

        public List<Character> Characters { get => characters; }

        public void SetCharacters(List<Character> characters)
        {
            this.characters = characters;
        }

        public bool CharactersActive
        {
            get
            {
                foreach (Character c in Characters)
                {
                    if (c.CurrentState != CharacterState.Inactive)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public float Alignment
        {
            get
            {
                float alignment = 0;
                foreach (Character c in Characters)
                {
                    alignment += c.Worker.Alignment;
                }
                alignment /= studioSize;
                return alignment;
            }
        }

        public override void ResetValues()
        {
            characters = new List<Character>();
        }
    }
}
