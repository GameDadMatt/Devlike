using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using DataTypes;

public class RestoreNeed : Action
{
    public SharedNeed need;

    public override TaskStatus OnUpdate()
    {
        if(need.Value.curValue < 1f)
        {
            need.Value.curValue = 1f;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}