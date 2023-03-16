using System;
using UnityEngine;
using Devlike.Characters;
using Devlike.Timing;

public class GameManager : ExecutableBehaviour
{
    public static GameManager instance;

    public GameState State { get; set; }
    public int CurrentTick { get; set; }


    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }        
    }

    protected override void OnStart()
    {
        //Do nothing
    }

    public event Action<Character> OnCharacterSelect;
    public void CharacterSelect(Character click)
    {
        OnCharacterSelect?.Invoke(click);
    }
}
