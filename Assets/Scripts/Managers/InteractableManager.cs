using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DataTypes;

public class InteractPosition
{
    public int id;
    public DoingType type;
    public Transform transform;
    public bool inUse = false;

    public InteractPosition(int id, DoingType type, Transform transform)
    {
        this.id = id;
        this.type = type;
        this.transform = transform;
    }
}

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager instance;

    private int posIDs = 0;
    private List<InteractPosition> positions = new List<InteractPosition>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }        
    }

    public PlayerInput GetPlayerInput
    {
        get
        {
            return GetComponent<PlayerInput>();
        }
    }

    public void RegisterInteractable(RegisterInteractPosition ip)
    {
        InteractPosition pos = new InteractPosition(posIDs, ip.type, ip.transform);
        positions.Add(pos);
        posIDs++;
    }

    public InteractPosition RandomPosOfType(DoingType type)
    {
        List<InteractPosition> ps = new List<InteractPosition>();
        foreach(InteractPosition p in positions)
        {
            if(p.type == type && !p.inUse)
            {
                ps.Add(p);
            }
        }

        if(ps.Count > 1)
        {
            int rand = Random.Range(0, ps.Count);
            return ps[rand];
        }
        else if(ps.Count == 1)
        {
            return ps[0];
        }
        Debug.LogError("Failed to find position of type " + type);
        return new InteractPosition(999, type, transform);
    }
}
