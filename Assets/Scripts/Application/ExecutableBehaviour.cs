using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExecutableBehaviour : MonoBehaviour
{
    [SerializeField]
    protected LoadGroup LoadGroup;
    [SerializeField]
    protected List<Object> RequiredScripts = new List<Object>();

    protected void OnEnable()
    {
        StartCoroutine(WaitForExecution());
    }

    protected void LaunchGroup(LoadGroup group)
    {
        if (LoadGroup == group)
        {
            if (RequiredScripts.Count > 0)
            {
                StartCoroutine(WaitForRequiredScripts());
            }
            else
            {
                OnStart();
            }
        }
    }

    protected IEnumerator WaitForRequiredScripts()
    {
        while(RequiredScripts.Count > 0)
        {
            List<Object> check = RequiredScripts;
            foreach(Object obj in RequiredScripts)
            {
                if (ScriptExecutionGroup.instance.TypeIsLaunched(obj.GetType()))
                {
                    check.Remove(obj);
                }
            }
            RequiredScripts = check;
            yield return new WaitForEndOfFrame();
        }

        OnStart();
    }

    protected abstract void OnStart();

    private IEnumerator WaitForExecution()
    {
        while (ScriptExecutionGroup.instance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        ScriptExecutionGroup.instance.Register(this);
        ScriptExecutionGroup.instance.OnGroupReady += LaunchGroup;
    }
}
