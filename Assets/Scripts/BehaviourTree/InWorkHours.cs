using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Devlike.Characters;

public class InWorkHours : Conditional
{
    public SharedInt currentTick;
    public SharedInt workStartTick;
    public SharedInt workEndTick;
    public SharedCharacterState characterState;

    public override void OnAwake()
    {
        base.OnAwake();
        characterState.Value = CharacterState.Inactive;
    }

    public override TaskStatus OnUpdate()
    {
        if(currentTick.Value > workStartTick.Value && currentTick.Value <= workEndTick.Value)
        {
            if (characterState.Value == CharacterState.Inactive)
            {
                characterState.Value = CharacterState.Start;
            }
            else if (characterState.Value == CharacterState.Start)
            {
                characterState.Value = CharacterState.Active;
            }
            return TaskStatus.Success;
        }
        
        //None of the above statements happened, so it must be the end of the day
        if (characterState.Value == CharacterState.Active)
        {
            characterState.Value = CharacterState.End;
        }
        else if (characterState.Value == CharacterState.End)
        {
            characterState.Value = CharacterState.Inactive;
        }
        return TaskStatus.Failure;
    }
}
