using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Devlike.Timing;
using Devlike.UI;

namespace Devlike.Player
{
    public enum ActionType { ScopeManagement, TaskManagement, TeamMeeting, CheckProgress, TalkTo }

    public class PlayerAction
    {
        public string ID { get; private set; }
        public object Object { get; private set; }
        public ActionType Type { get; private set; }
        public bool Active { get; set; }
        public int CompletedTicks { get; set; }
        public int TotalTicks { get; private set; }
        public float Progress { get { return (float)CompletedTicks / (float)TotalTicks; } }
        public bool Completed { get { return CompletedTicks >= TotalTicks; } }

        public PlayerAction(GlobalTime time, ProgressButton button, string id, ActionType type, object obj, bool randTime, float minHours, float maxHours)
        {
            ID = id;
            Type = type;
            Object = obj;
            if (randTime)
            {
                TotalTicks = Mathf.RoundToInt(Random.Range(minHours, maxHours) * time.TicksPerHour);
            }
            else
            {
                TotalTicks = Mathf.RoundToInt(maxHours * time.TicksPerHour);
            }
        }

        public void Reset()
        {
            CompletedTicks = 0;
            Active = false;
        }
    }

    public class PlayerManager : ExecutableBehaviour
    {
        private List<PlayerAction> activeActions = new List<PlayerAction>();
        private PlayerAction progressingAction = null;
        private PlayerAction currentAction = null;

        public int voiceStat = 10;
        public int forteStat = 10;
        public int empathyStat = 10;

        protected override void SetListeners()
        {
            EventManager.instance.OnTick += TickAction;
            EventManager.instance.OnPlayerAction += StartAction;
            EventManager.instance.OnCompletePlayerAction += CompleteAction;
        }

        public void StartAction(PlayerAction action)
        {
            //Set the current action to the one that was just passed
            progressingAction = ExistingOrNewAction(action);
            SetActionsInactive();
            progressingAction.Active = true;
        }

        private void SetActionsInactive()
        {
            foreach(PlayerAction action in activeActions)
            {
                action.Active = false;
            }
        }

        private void TickAction()
        {
            if(progressingAction != null && currentAction == null)
            {
                progressingAction.CompletedTicks++;
                if (progressingAction.Completed)
                {
                    Debug.Log("DISPLAY " + progressingAction.Type);
                    EventManager.instance.ParsePlayerAction(progressingAction.Type, progressingAction.Object);
                    currentAction = progressingAction; //Set the current action, which will stop this function acting on tick until it's done
                    activeActions.Remove(progressingAction);

                    //If there are other actions, go to the next active action
                    if (activeActions.Count > 0)
                    {
                        progressingAction = activeActions[0];
                        progressingAction.Active = true;
                    }
                    else
                    {
                        progressingAction = null;
                    }
                }
            }      
        }

        private void CompleteAction()
        {
            if (currentAction != null)
            {
                currentAction.Reset();
                currentAction = null;
            }
            Debug.LogWarning("CompleteAction called when there is no current action");
        }

        /// <summary>
        /// Check the list of actions to see if the passed action exists. If it doesn't, add it to the list and return it.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private PlayerAction ExistingOrNewAction(PlayerAction action)
        {
            if(activeActions.Count > 0)
            {
                foreach (PlayerAction listAction in activeActions)
                {
                    if (action.ID == listAction.ID)
                    {
                        //It's an existing action
                        return listAction;
                    }
                }
            }            

            //It's a new action
            activeActions.Add(action);
            return action;
        }
    }
}