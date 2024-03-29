using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Devlike.Player;
using Devlike.Timing;

namespace Devlike.UI
{
    public class ProgressButton : ExecutableBehaviour
    {
        [SerializeField]
        protected GlobalTime gTime;

        [SerializeField]
        protected Image progressBar;
        [SerializeField]
        protected TextMeshProUGUI buttonText;
        [SerializeField]
        protected ActionContainer actionContainer;
        [SerializeField]
        protected Button button;
        [SerializeField]
        protected bool overrideDisable = false;
        protected PlayerAction action;

        protected override void SetListeners()
        {
            EventManager.instance.OnTick += UpdateProgress;
            progressBar.fillAmount = 0f;
        }

        protected override void RegisterObjects()
        {
            EventManager.instance.RegisterButton(this);
        }

        public virtual void GenerateButton()
        {
            buttonText.SetText(actionContainer.name);
            action = new PlayerAction(gTime, this, actionContainer.name, actionContainer.type, null, actionContainer.randomCompleteTime, actionContainer.minHoursToComplete, actionContainer.maxHoursToComplete);
            if (overrideDisable)
            {
                button.interactable = false;
            }
        }

        public virtual void PressButton()
        {
            EventManager.instance.PlayerAction(action);
        }

        public virtual void ResetButton()
        {
            progressBar.fillAmount = 0f;
            if (!overrideDisable)
            {
                button.interactable = true;
            }            
        }

        protected void UpdateProgress()
        {
            if(action != null)
            {
                if (action.Active)
                {
                    button.interactable = false;
                    progressBar.fillAmount = action.Progress;
                }
                else if(!action.Active && action.Progress > 0)
                {
                    button.interactable = true;
                }
            }
            else
            {
                Debug.LogError("Action has not been set on " + actionContainer.name + " button");
            }
        }

        public bool Interactable
        {
            get
            {
                return button.interactable;
            }
            set
            {
                if (!overrideDisable)
                {
                    button.interactable = value;
                }
            }
        }

        public bool InProgress
        {
            get
            {
                return action.Progress > 0 && !action.Completed;
            }
        }
    }
}