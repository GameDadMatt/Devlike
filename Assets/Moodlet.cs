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

        public void OnEnable()
        {
            moodletArea.SetActive(false);
        }

        public void DisplayMoodlet(Sprite sprite)
        {
            moodletImage.sprite = sprite;
            moodletArea.SetActive(true);
        }

        public void HideMoodlet()
        {
            moodletArea.SetActive(false);
        }
    }
}