using System;
using UnityEngine;
using Characters;
using DataTypes;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState State { get; private set; } = GameState.Ticking;

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
