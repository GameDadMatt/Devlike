using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class InWorkHours : Conditional
{
    public SharedInt currentTick;
    public SharedInt dayStartTick;
    public SharedInt dayEndTick;

    public override TaskStatus OnUpdate()
    {
        Debug.Log("The tick is " + currentTick.Value);
        if(currentTick.Value > dayStartTick.Value && currentTick.Value <= dayEndTick.Value)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
