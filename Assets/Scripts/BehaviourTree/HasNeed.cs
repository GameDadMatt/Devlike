using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using DataTypes;

public class HasNeed : Conditional
{
    public SharedNeed need;

    public override TaskStatus OnUpdate()
    {
        if(need.Value.curValue <= need.Value.threshold)
        {
            Debug.Log("I need " + need.Value.type + " " + need.Value.curValue);
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
