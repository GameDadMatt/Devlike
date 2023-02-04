using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class MoveToLocation : Action
{
    public SharedTransform location;

    public override TaskStatus OnUpdate()
    {
        if (transform != location.Value)
        {
            transform.position = location.Value.position;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
