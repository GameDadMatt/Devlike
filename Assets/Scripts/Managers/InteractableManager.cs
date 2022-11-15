using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager instance;
    private List<InteractivePosition> positions;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }        
    }

    public void RegisterInteractable(InteractivePosition ip)
    {
        positions.Add(ip);
    }

    public void UnregisterInteractable(InteractivePosition ip)
    {
        positions.Remove(ip);
    }

    public InteractivePosition GetPositionOfType(TaskType type)
    {
        foreach(InteractivePosition pos in positions)
        {
            if(pos.restores == type)
            {
                return pos;
            }
        }
        Debug.LogError("Failed to find position of type " + type);
        return null;
    }
}
