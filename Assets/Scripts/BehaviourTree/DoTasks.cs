using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class DoTasks : Action
{
    public SharedInt taskNum;
    public SharedDoingType type;

    public override TaskStatus OnUpdate()
    {
        if (taskNum.Value > 0)
        {
            type.Value = Devlike.Characters.DoingType.Work; //Update the doing type to reflect that we are doing work
        }
        else
        {
            type.Value = Devlike.Characters.DoingType.Idle; //Update the doing type to reflect that we have no work to do
        }        
        return TaskStatus.Success;
    }
}