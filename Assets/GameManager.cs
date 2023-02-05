using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
