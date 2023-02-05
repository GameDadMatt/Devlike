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
    public SharedBool atWork;

    public override void OnAwake()
    {
        base.OnAwake();
        atWork.Value = false;
    }

    public override TaskStatus OnUpdate()
    {
        if(currentTick.Value > dayStartTick.Value && currentTick.Value <= dayEndTick.Value)
        {
            if (!atWork.Value)
            {
                atWork.Value = true;
            }
            return TaskStatus.Success;
        }
        
        if (atWork.Value)
        {
            atWork.Value = false;
        }
        return TaskStatus.Failure;
    }
}
