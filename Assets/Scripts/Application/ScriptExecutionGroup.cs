using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum LaunchStep { SetListeners, SetProperties, RegisterObjects, Launch, AfterDelay }

//Unity sucks, so we have this instead of using the Script Execution Order
public class ScriptExecutionGroup : MonoBehaviour
{
    public static ScriptExecutionGroup instance;

    private int registered = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(ExecuteSteps());
    }

    public event Action<LaunchStep> OnLaunchStep;
    public void Register()
    {
        registered++;
    }

    private IEnumerator ExecuteSteps()
    {
        yield return new WaitForSeconds(0.1f);

        while (registered < WaitingObjects)
        {
            yield return new WaitForSeconds(0.1f);
        }

        for(int i = 0; i < (int)LaunchStep.AfterDelay +1; i++)
        {
            Debug.Log("Step " + i);
            OnLaunchStep?.Invoke((LaunchStep)i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private int WaitingObjects
    {
        get
        {
            return OnLaunchStep.GetInvocationList().Length;
        }
    }
}
