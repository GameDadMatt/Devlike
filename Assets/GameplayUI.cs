using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Devlike.UI
{
    public class GameplayUI : MonoBehaviour
    {
        public static GameplayUI instance;

        [SerializeField]
        private TextMeshProUGUI week;
        [SerializeField]
        private TextMeshProUGUI dayHour;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        public void SetWeek(int num)
        {
            week.SetText("Week " + num);
        }

        public void SetTime(string day, string hour)
        {
            dayHour.SetText(day + " " + hour);
        }
    }
}