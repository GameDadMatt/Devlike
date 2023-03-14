using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Timing;

namespace Devlike.Player
{
    public enum ActionType { ScopeManagement, TaskManagement, TeamMeeting, CheckProgress, TalkTo }

    public class PlayerAction
    {
        public string ID { get; private set; }
        public object Object { get; private set; }
        public ActionType Type { get; private set; }
        public int CompletedTicks { get; set; }
        public int TotalTicks { get; private set; }
        public float Progress { get { return CompletedTicks / TotalTicks; } }
        public bool Completed { get { return CompletedTicks >= TotalTicks; } }
        public int voiceStat = 10;
        public int forteStat = 10;
        public int empathyStat = 10;

        public PlayerAction(string id, ActionType type, object obj, bool randTime, int minTime, int maxTime)
        {
            ID = id;
            Type = type;
            Object = obj;
            if (randTime)
            {
                TotalTicks = Random.Range(minTime, maxTime + 1);
            }
            else
            {
                TotalTicks = maxTime;
            }
        }
    }

    public class PlayerManager : MonoBehaviour
    {
        private List<PlayerAction> activeActions = new List<PlayerAction>();
        private PlayerAction currentAction;

        public void Start()
        {
            TimeManager.instance.OnTick += TickAction;
            EventManager.instance.OnPlayerAction += StartAction;
        }

        public void StartAction(PlayerAction action)
        {
            //Set the current action to the one that was just passed
            currentAction = ExistingOrNewAction(action);
        }

        private void TickAction()
        {
            if(currentAction != null)
            {
                currentAction.CompletedTicks++;
                if (currentAction.Completed)
                {
                    EventManager.instance.ParsePlayerAction(currentAction.Type, currentAction.Object);
                    activeActions.Remove(currentAction);

                    //If there are other actions, go to the next active action
                    if (activeActions.Count > 0)
                    {
                        currentAction = activeActions[0];
                    }
                    else
                    {
                        currentAction = null;
                    }
                }
            }            
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