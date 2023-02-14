using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class HasNeed : Conditional
{
    public SharedDoingTracker need;

    public override TaskStatus OnUpdate()
    {
        if(need.Value.curValue <= need.Value.threshold)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
