using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Devlike.Characters
{
    public class CharacterMoodlet : MonoBehaviour
    {
        public bool Ticking { get; private set; } = false;
        private string id = "";
        private bool displaying = false;
        private int delayTicks = 0;
        private int displayTicks = 0;
        private int cooldownTicks = 0;
        private Sprite currentSprite;

        public bool TempDisplayReady { get => tempDisplayTicks == 0 && tempMoodletCooldown == 0 && !displaying; }
        private int tempDisplayTicks = 0;
        private int tempMoodletCooldown = 0;

        public void RegisterCharacter(string id)
        {
            this.id = id;
        }

        public void NewMoodlet(Sprite sprite, int delay, int display, int cooldown)
        {
            Debug.Log("New moodlet " + delay + " " + display + " " + cooldown);
            Ticking = true;
            currentSprite = sprite;
            delayTicks = delay;
            displayTicks = display;
            cooldownTicks = cooldown;
        }

        public void NewTempMoodlet(Sprite sprite, int display, int cooldown)
        {
            tempDisplayTicks = display;
            tempMoodletCooldown = cooldown;
            EventManager.instance.SetCharacterMoodlet(id, true, sprite);
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

        public event Action OnMoodletCompleted;
        public void Tick()
        {
            //TEMP MOODLETS
            if(tempDisplayTicks > 0)
            {
                tempDisplayTicks--;
                if(tempDisplayTicks == 0)
                {
                    EventManager.instance.SetCharacterMoodlet(id, false, currentSprite);
                }
            }
            else if(tempMoodletCooldown > 0)
            {
                tempMoodletCooldown--;
            }

            //NORMAL MOODLETS
            if (!displaying && delayTicks > 0)
            {
                delayTicks--;
                if(delayTicks == 0)
                {
                    tempDisplayTicks = 0;
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
                    Ticking = false;
                    OnMoodletCompleted?.Invoke();
                }
            }
        }

        public void ClearMoodlet()
        {
            delayTicks = 0;
            displayTicks = 0;
            cooldownTicks = 0;
            HideMoodlet();
        }
    }
}