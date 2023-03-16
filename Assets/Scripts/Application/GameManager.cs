using System;
using UnityEngine;
using Devlike.Characters;
using Devlike.Timing;

public class GameManager : ExecutableBehaviour
{
    public static GameManager instance;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }        
    }
}
