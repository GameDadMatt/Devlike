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
        private int moodVal;
        private int motivationVal;
        private int hungerVal;
        private Personality personality;
        
        //Public Getter/Setter Values
        public Task CurrentTask { get; private set; }
        public Task CurrentNeed { get; private set; }
        public Emotion CurrentEmotion { get; private set; }
        public Incident RememberedIncident { get; private set; }
    }
}
