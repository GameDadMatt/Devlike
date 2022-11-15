using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DataTypes;
using Characters;

public class HasNeed : Conditional
{
	public TaskType needType;
	public AICharacter character;

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}