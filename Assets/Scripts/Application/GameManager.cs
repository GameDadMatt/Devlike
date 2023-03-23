using System;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;
using Devlike.Timing;
using Devlike;

public class GameManager : ExecutableBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private bool ResetOnStart;
    [SerializeField]
    private List<GlobalObject> globalObjects = new List<GlobalObject>();

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple copies of " + name + " detected");
        }

        if (ResetOnStart)
        {
            ResetGlobal();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            EventManager.instance.ChangeGameState(GameState.Paused);
            ResetGlobal();
        }
    }

    protected override void SetProperties()
    {
        //Load the contents of DataValues
        CodeValues.LoadLists();
        Debug.Log("Loaded Lists");
    }

    public void ResetGlobal()
    {
        foreach (GlobalObject obj in globalObjects)
        {
            obj.ResetValues();
        }

        Debug.LogWarning("Values reset");
    }
}
