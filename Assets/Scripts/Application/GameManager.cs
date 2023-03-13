using System;
using UnityEngine;
using Devlike.Characters;
using Devlike.Timing;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState State { get { return TimeManager.instance.State; } }

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }        
    }

    public event Action<Character> OnCharacterSelect;
    public void CharacterSelect(Character click)
    {
        OnCharacterSelect?.Invoke(click);
    }
}
