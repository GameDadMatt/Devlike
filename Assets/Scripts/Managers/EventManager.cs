using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalManagers
{
    public class EventManager : MonoBehaviour
    {
        public EventManager Instance { get; private set; }

        public void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }


    }
}
