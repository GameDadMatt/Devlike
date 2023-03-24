using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Devlike.Characters
{
    public class CharacterMoodlet : MonoBehaviour
    {
        public bool Ready { get; private set; } = true;
        private string id = "";
        private bool displaying = false;
        private int delayTicks = 0;
        private int displayTicks = 0;
        private int cooldownTicks = 0;
        private Sprite currentSprite;

        public void RegisterCharacter(string id)
        {
            this.id = id;
        }

        public void NewMoodlet(Sprite sprite, int delay, int display, int cooldown)
        {
            Ready = false;
            currentSprite = sprite;
            delayTicks = delay;
            displayTicks = display;
            cooldownTicks = cooldown;
        }

        private void DisplayMoodlet()
        {
            displaying = true;
            EventManager.instance.SetCharacterMoodlet(id, true, currentSprite);
        }

        private void HideMoodlet()
        {
            displaying = false;
            EventManager.instance.SetCharacterMoodlet(id, false, currentSprite);
        }

        public void Tick()
        {
            if (!displaying && delayTicks > 0)
            {
                delayTicks--;
                if(delayTicks == 0)
                {
                    DisplayMoodlet();
                }
            }
            else if (displaying && displayTicks > 0)
            {
                displayTicks--;
                if(displayTicks == 0)
                {
                    HideMoodlet();
                }
            }
            else if (!displaying && cooldownTicks > 0)
            {
                cooldownTicks--;
                if(cooldownTicks == 0)
                {
                    Ready = true;
                }
            }
        }
    }
}