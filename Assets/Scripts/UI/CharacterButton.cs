using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;

namespace Devlike.UI
{
    public class CharacterButton : MonoBehaviour
    {
        private Character character;

        public void OnEnable()
        {
            character = GetComponent<Character>();
        }

        public void SetCharacter(Character character)
        {
            this.character = character;
        }

        public void OnMouseOver()
        {
            //Left click
            if (Input.GetMouseButtonDown(0))
            {
                EventManager.instance.CharacterInteract(character);
            }
            //Right click
            else if (Input.GetMouseButtonDown(1))
            {
                GameManager.instance.CharacterSelect(character);
            }
        }
    }
}