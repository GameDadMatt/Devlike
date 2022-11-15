using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;

namespace Characters
{
    public class AICharacter : Character
    {
        //AI Character makes decisions and passes them up to CharacterContainer

        //Private Values
        private PersonalityType personalityType;
        private Need rest;
        private Need eat;
        private Need think;
        
        //Public Getter/Setter Values
        public TaskType CurrentTask { get; private set; }
        public TaskType CurrentNeed { get; private set; }
        public EmotionType CurrentEmotion { get; private set; }
        public IncidentType RememberedIncident { get; private set; }

        public AICharacter()
        {
            personalityType = PersonalityType.Default;
            Personality personality = DataValues.GetPersonalityOfType(personalityType);

            rest = new Need(TaskType.Resting, personality.restThreshold);
            eat = new Need(TaskType.Eating, personality.foodThreshold);
            think = new Need(TaskType.Thinking, personality.thinkThreshold);
        }
    }
}
