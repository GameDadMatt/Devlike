using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class RestoreNeed : Action
{
    public SharedDoingTracker tracker; //Tracks and updates the value of the need we're refilling
    public SharedDoingType doing; //Stores the type of thing we're currently doing
    public SharedInt restoreTicks;
    private int curTicks = 0;
    private int setTicks = 0;

    public override void OnStart()
    {
        //Set the value of the ticks to take, as RestoreTicks is set at random on the character
        setTicks = restoreTicks.Value;
    }

    public override TaskStatus OnUpdate()
    {
        curTicks++;
        if (curTicks < setTicks)
        {
            return TaskStatus.Running;
        }
        else if (curTicks >= setTicks)
        {
            tracker.Value.curValue = 1f;
            doing.Value = tracker.Value.type; //Update the doing tracker with what we're doing
            curTicks = 0;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}