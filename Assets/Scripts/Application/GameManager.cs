using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Devlike.Characters;
using Devlike.Timing;
using Devlike;

public class GameManager : ExecutableBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private bool ResetOnStart;
    [SerializeField]
    private GlobalGame gGame;
    [SerializeField]
    private GlobalTime gTime;
    [SerializeField]
    private GlobalStudio gStudio;
    [SerializeField]
    private GlobalProject gProject;
    [SerializeField]
    private GlobalCharacter gCharacter;
    [SerializeField]
    private GlobalDialogue gDialogue;
    [Header("TEXT DATA")]
    [SerializeField]
    private TextAsset hobbies;
    [SerializeField]
    private TextAsset firstNames;
    [SerializeField]
    private TextAsset lastNames;
    [SerializeField]
    private TextAsset nickNames;

    private GlobalObject[] GlobalObjects { get { return new GlobalObject[] { gGame, gTime, gStudio, gProject, gCharacter, gDialogue }; } }

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

        if (!Application.isEditor)
        {
            //Load other scenes
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }

        if (ResetOnStart)
        {
            ResetGlobal();
        }
    }

    protected override void AfterDelay()
    {
        EventManager.instance.DisplayUI(Devlike.Player.ActionType.TaskManagement);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            gGame.UpdateGameState(GameState.Paused);
            ResetGlobal();
        }
    }

    protected override void SetProperties()
    {
        //Load the contents of DataValues
        CodeValues.LoadLists(hobbies, firstNames, lastNames, nickNames);
        Debug.Log("Loaded Lists");
    }

    public void ResetGlobal()
    {
        foreach (GlobalObject obj in GlobalObjects)
        {
            obj.ResetValues();
        }

        Debug.LogWarning("Values reset");
    }
}
