using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum LoadGroup { Essential, Early, Late, NUMTYPES }

//Unity sucks, so we have this instead of using the Script Execution Order
public class ScriptExecutionGroup : MonoBehaviour
{
    public static ScriptExecutionGroup instance;

    private int loading = 0;
    private int[] groupNums = new int[3];

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
    public void RegisterToGroup(LoadGroup group)
    {
        switch (group)
        {
            case LoadGroup.Essential:
                groupNums[0]++;
                break;
            case LoadGroup.Early:
                groupNums[1]++;
                break;
            case LoadGroup.Late:
                groupNums[2]++;
                break;
        }
    }

    private IEnumerator LaunchGroups()
    {
        while (RegisteredObjects < WaitingObjects)
        {
            yield return new WaitForEndOfFrame();
        }

        //If everything is registered and waiting, we can go
        for (loading = 0; loading < (int)LoadGroup.NUMTYPES; loading++)
        {
            yield return new WaitForEndOfFrame();
            OnGroupReady?.Invoke((LoadGroup)loading);
        }
    }

    private int RegisteredObjects
    {
        get
        {
            int i = 0;
            foreach(int group in groupNums)
            {
                i += group;
            }
            return i;
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
