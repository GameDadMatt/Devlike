using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Devlike.UI;

namespace Devlike.UI
{
    public class MoodletDisplay : MonoBehaviour
    {
        [SerializeField]
        private GameObject moodletBG;
        [SerializeField]
        private Image moodletImage;
        private string id;

        public void RegisterMoodlet(string id)
        {
            EventManager.instance.OnSetCharacterMoodlet += SetSprite;
            this.id = id;
            moodletBG.SetActive(false);
        }

        public void SetSprite(string id, bool active, Sprite sprite)
        {
            if(id == this.id)
            {
                moodletBG.SetActive(active);
                moodletImage.sprite = sprite;
            }
        }

        private void OnDisable()
        {
            EventManager.instance.OnSetCharacterMoodlet -= SetSprite;
        }
    }
}
