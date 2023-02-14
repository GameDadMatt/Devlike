using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using Devlike.Characters;

public class MoveToLocation : Action
{
    public DoingType positionType;
    private Character character;
    public SharedDoingType charDoing;

    public override void OnAwake()
    {
        character = transform.GetComponent<Character>();
    }

    public override TaskStatus OnUpdate()
    {
        if (charDoing.Value != positionType)
        {
            if(positionType == DoingType.Work)
            {
                //Go to your defined desk
                character.NewPosition(character.Desk);
                return TaskStatus.Success;
            }
            else if(positionType == DoingType.Home)
            {
                //Go home
                character.NewPosition(character.Home);
                return TaskStatus.Success;
            }
            else
            {
                NPCInteractable interactable = InteractableManager.instance.RandomPosOfType(positionType);
                if (interactable != null)
                {
                    character.NewPosition(interactable);
                    return TaskStatus.Success;
                }
            }            
            return TaskStatus.Failure;
        }
        return TaskStatus.Success;
    }
}
