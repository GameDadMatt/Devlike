using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Devlike.Player;
using Devlike.Timing;

namespace Devlike.UI
{
    public class ProgressButton : MonoBehaviour
    {
        [SerializeField]
        public Image progressBar;
        [SerializeField]
        public TextMeshProUGUI buttonText;
        [SerializeField]
        public ActionContainer actionContainer;
        protected PlayerAction action;

        public void Start()
        {
            //We do this on Start to give TimeManager time to start up
            TimeManager.instance.OnTick += UpdateProgress;
            GameplayUI.instance.RegisterButton(this);
        }

        public virtual void GenerateButton()
        {
            buttonText.SetText(actionContainer.name);
            action = new PlayerAction(actionContainer.name, actionContainer.type, null, actionContainer.randomCompleteTime, actionContainer.minHoursToComplete, actionContainer.maxHoursToComplete);
        }

        public void PressButton()
        {
            EventManager.instance.PlayerAction(action);
        }

        private void UpdateProgress()
        {
            if(action != null)
            {
                progressBar.fillAmount = action.Progress;
            }
            else
            {
                Debug.LogError("Action has not been set on " + actionContainer.name + " button");
            }
        }
    }
}