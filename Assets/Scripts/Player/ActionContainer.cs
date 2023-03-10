using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Player
{
    [CreateAssetMenu(fileName = "Action Container", menuName = "Devlike/Action Container")]
    public class ActionContainer : ScriptableObject
    {
        public ActionType type;
        public bool randomCompleteTime;
        public int minHoursToComplete = 0;
        public int maxHoursToComplete = 0;
    }
}