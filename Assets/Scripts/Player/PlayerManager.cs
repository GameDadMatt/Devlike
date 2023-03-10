using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Timing;

namespace Devlike.Player
{
    public enum ActionType { ScopeManagement, TaskManagement, TeamMeeting, CheckProgress, TalkTo }

    public class PlayerAction
    {
        public string ID { get; private set; } = "";
        public ActionType Type { get; private set; }
        public float CompletedTime { get; set; }
        public int TotalTime { get; private set; }
        public float Progress { get { return CompletedTime / TotalTime; } }
        public bool Completed { get { return CompletedTime >= TotalTime; } }

        public PlayerAction(string id, ActionType type, bool randTime, int minTime, int maxTime)
        {
            ID = id;
            Type = type;
            if (randTime)
            {
                TotalTime = Random.Range(minTime, maxTime + 1);
            }
            else
            {
                TotalTime = maxTime;
            }
        }
    }

    public class PlayerManager : MonoBehaviour
    {
        public List<PlayerAction> activeActions;
        private PlayerAction currentAction;

        public void Awake()
        {
            TimeManager.instance.OnTick += DoAction;
        }

        public void StoreAction(PlayerAction action)
        {
            currentAction = ExistingOrNewAction(action);
        }

        private void DoAction()
        {
            if(currentAction != null)
            {
                currentAction.CompletedTime += GlobalVariables.value.TickLength;
                if (currentAction.Completed)
                {
                    EventManager.instance.PlayerAction(currentAction.Type, currentAction.ID);
                    activeActions.Remove(currentAction);

                    //If there are other actions, go to the next active action
                    if (activeActions.Count > 0)
                    {
                        currentAction = activeActions[0];
                    }
                }
            }            
        }

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