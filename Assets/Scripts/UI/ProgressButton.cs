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
        protected Image progressBar;
        [SerializeField]
        protected TextMeshProUGUI buttonText;
        [SerializeField]
        protected ActionContainer actionContainer;
        [SerializeField]
        protected Button button;
        protected PlayerAction action;

        private void Awake()
        {
            progressBar.fillAmount = 0f;
        }

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

        public virtual void PressButton()
        {
            EventManager.instance.PlayerAction(action);
        }

        public bool Interactable
        {
            get
            {
                return button.interactable;
            }
            set
            {
                button.interactable = value;
            }
        }

        private void UpdateProgress()
        {
            if(action != null)
            {
                if (action.Active)
                {
                    progressBar.fillAmount = action.Progress;
                }
            }
            else
            {
                Debug.LogError("Action has not been set on " + actionContainer.name + " button");
            }
        }
    }
}