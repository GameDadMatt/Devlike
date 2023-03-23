using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Devlike.Characters
{
    public class Moodlet : MonoBehaviour
    {
        [SerializeField]
        private GameObject moodletArea;
        [SerializeField]
        private Image moodletImage;

        public bool Ready { get; private set; } = true;
        private bool displaying = false;
        private int delayTicks = 0;
        private int displayTicks = 0;
        private int cooldownTicks = 0;

        public void OnEnable()
        {
            moodletArea.SetActive(false);
        }

        public void NewMoodlet(Sprite sprite, int delay, int display, int cooldown)
        {
            Ready = false;
            moodletImage.sprite = sprite;
            delayTicks = delay;
            displayTicks = display;
            cooldownTicks = cooldown;
        }

        private void DisplayMoodlet()
        {
            displaying = true;            
            moodletArea.SetActive(true);
        }

        private void HideMoodlet()
        {
            displaying = false;
            moodletArea.SetActive(false);
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