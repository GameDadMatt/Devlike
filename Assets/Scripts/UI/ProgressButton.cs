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
        protected PlayerAction thisAction;

        public void Awake()
        {
            TimeManager.instance.OnTick += UpdateProgress;
            GameplayUI.instance.RegisterButton(this);
        }

        public virtual void GenerateButton()
        {
            buttonText.SetText(actionContainer.name);
            thisAction = new PlayerAction(actionContainer.name, actionContainer.type, actionContainer.randomCompleteTime, actionContainer.minHoursToComplete, actionContainer.maxHoursToComplete);
        }

        private void UpdateProgress()
        {
            if(thisAction != null)
            {
                progressBar.fillAmount = thisAction.Progress;
            }
            else
            {
                Debug.LogError("Action has not been set on " + actionContainer.name + " button");
            }
        }
    }
}