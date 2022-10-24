using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class CharacterContainer : MonoBehaviour
    {
        //Character Container controls character movement and position with input from AICharacter and PlayerCharacter
        public int ID { get; private set; }
        public bool IsPlayer { get; private set; }

        public void MoveCharacter()
        {
            //Takes input from either AI or Player to move around
        }

        public bool CanInteract
        {
            //Returns whether this container is in contact with an interactable object
            get
            {
                return false;
            }
        }
    }

}