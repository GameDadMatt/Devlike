using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class ExecutableBehaviour : MonoBehaviour
{
    protected void OnEnable()
    {
        StartCoroutine(WaitForExecution());
    }

    private IEnumerator WaitForExecution()
    {
        while (ScriptExecutionGroup.instance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        ScriptExecutionGroup.instance.Register();
        ScriptExecutionGroup.instance.OnLaunchStep += OnLaunch;
    }

    protected void OnLaunch(LaunchStep step)
    {
        switch (step)
        {
            case LaunchStep.SetListeners:
                SetListeners();
                break;
            case LaunchStep.SetProperties:
                SetProperties();
                break;
            case LaunchStep.RegisterObjects:
                RegisterObjects();
                break;
            case LaunchStep.Launch:
                Launch();
                break;
            case LaunchStep.AfterDelay:
                AfterDelay();
                break;
        }
    }

    protected virtual void SetListeners()
    {
        //Do nothing
    }

    protected virtual void SetProperties()
    {
        //Do nothing
    }

    protected virtual void RegisterObjects()
    {
        //Do nothing
    }

    protected virtual void Launch()
    {
        //Do nothing
    }

    protected virtual void AfterDelay()
    {
        //Do nothing
    }
}
