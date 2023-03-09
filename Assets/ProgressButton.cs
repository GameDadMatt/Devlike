using Devlike.Timing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Devlike.UI
{
    public class ProgressButton : MonoBehaviour
    {
        //Make a class to contain PLAYER TASK and TASK PROGRESS
        //Task progress 0 to 1
        //Player class to track active tasks

        public Image progressBar;

        public void Awake()
        {
            TimeManager.instance.OnTick += UpdateProgress;
        }

        private void UpdateProgress()
        {

        }
    }
}