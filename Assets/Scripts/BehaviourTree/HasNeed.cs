using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using DataTypes;

public class HasNeed : Conditional
{
    public SharedNeed need;

    public override TaskStatus OnUpdate()
    {
        Debug.Log(need.Value.curValue);
        if(need.Value.curValue <= need.Value.threshold)
        {
            Debug.Log("NEED TRIGGER" + need.Value.curValue);
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
