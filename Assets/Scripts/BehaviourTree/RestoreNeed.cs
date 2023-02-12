using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using DataTypes;

public class RestoreNeed : Action
{
    public SharedDoingTracker tracker;
    public SharedInt restoreTicks;
    private int curTicks = 0;

    public override TaskStatus OnUpdate()
    {
        if(curTicks < restoreTicks.Value)
        {
            curTicks++;
            return TaskStatus.Running;
        }
        else if (curTicks >= restoreTicks.Value)
        {
            tracker.Value.curValue = 1f;
            curTicks = 0;
            return TaskStatus.Failure;
        }
        return TaskStatus.Failure;
    }
}