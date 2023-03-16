using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum LoadGroup { Essential, Early, Late }

//Unity sucks, so we have this instead of using the Script Execution Order
public class ScriptExecutionGroup : MonoBehaviour
{
    public static ScriptExecutionGroup instance;

    private int loading = 0;
    private List<UnityEngine.Object> reportedScripts = new List<UnityEngine.Object>();
    private List<UnityEngine.Object> launchedScripts = new List<UnityEngine.Object>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(LaunchGroups());
    }

    public event Action<LoadGroup> OnGroupReady;
    public void Register(UnityEngine.Object script)
    {
        reportedScripts.Add(script);
    }

    public void Launched(UnityEngine.Object script)
    {
        launchedScripts.Add(script);
    }

    private IEnumerator LaunchGroups()
    {
        while (RegisteredObjects < WaitingObjects)
        {
            yield return new WaitForEndOfFrame();
        }

        //If everything is registered and waiting, we can go
        for (loading = 0; loading <= (int)LoadGroup.Late; loading++)
        {
            yield return new WaitForEndOfFrame();
            OnGroupReady?.Invoke((LoadGroup)loading);
        }
    }

    public bool TypeIsLaunched(Type type)
    {
        foreach(UnityEngine.Object obj in launchedScripts)
        {
            if(obj.GetType() == type)
            {
                return true;
            }
        }
        return false;
    }

    private int RegisteredObjects
    {
        get
        {
            return reportedScripts.Count;
        }
    }

    private int WaitingObjects
    {
        get
        {
            return OnGroupReady.GetInvocationList().Length;
        }
    }
}
