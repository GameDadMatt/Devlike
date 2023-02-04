using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using DataTypes;

public class RestoreNeed : Action
{
    public SharedNeed need;
    public SharedInt restoreTicks;
    private int curTicks = 0;

    public override TaskStatus OnUpdate()
    {
        if(curTicks < restoreTicks.Value)
        {
            curTicks++;

            if (curTicks >= restoreTicks.Value)
            {
                need.Value.curValue = 1f;
                curTicks = 0;
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}