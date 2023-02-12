using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using DataTypes;

public class MoveToLocation : Action
{
    public DoingType positionType;
    public SharedDoingType charDoing;

    public override TaskStatus OnUpdate()
    {
        if (charDoing.Value != positionType)
        {
            InteractPosition newPos = InteractableManager.instance.RandomPosOfType(positionType);
            if (newPos != null)
            {
                transform.position = newPos.transform.position;
                charDoing.Value = positionType;
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
        return TaskStatus.Success;
    }
}
