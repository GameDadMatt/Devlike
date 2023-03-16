using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExecutableBehaviour : MonoBehaviour
{
    [SerializeField]
    protected LoadGroup LoadGroup;

    protected void OnEnable()
    {
        StartCoroutine(WaitForExecution());
    }

    protected void LaunchGroup(LoadGroup group)
    {
        if (LoadGroup == group)
        {
            OnStart();
        }
    }

    protected abstract void OnStart();

    private IEnumerator WaitForExecution()
    {
        while (ScriptExecutionGroup.instance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        ScriptExecutionGroup.instance.RegisterToGroup(LoadGroup);
        ScriptExecutionGroup.instance.OnGroupReady += LaunchGroup;
    }
}
