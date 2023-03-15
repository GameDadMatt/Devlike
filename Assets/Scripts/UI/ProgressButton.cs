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

        public void OnEnable()
        {
            TimeManager.instance.OnTick += UpdateProgress;
            GameplayUI.instance.RegisterButton(this);
        }

        public virtual void GenerateButton()
        {
            buttonText.SetText(actionContainer.name);
            action = new PlayerAction(this, actionContainer.name, actionContainer.type, null, actionContainer.randomCompleteTime, actionContainer.minHoursToComplete, actionContainer.maxHoursToComplete);
        }

        public virtual void PressButton()
        {
            EventManager.instance.PlayerAction(action);
        }

        public virtual void ResetButton()
        {
            progressBar.fillAmount = 0f;
            button.interactable = true;
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
                button.interactable = value;
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