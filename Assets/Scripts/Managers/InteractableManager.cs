using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Devlike.Characters;

public class NPCInteractable
{
    public int id;
    public DoingType type;
    public GameObject thing;
    public bool inUse = false;

    public NPCInteractable(int id, DoingType type, GameObject thing)
    {
        this.id = id;
        this.type = type;
        this.thing = thing;
    }
}

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager instance;

    private int posIDs = 1;
    private List<NPCInteractable> interactables = new List<NPCInteractable>();
    public NPCInteractable Home { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }        
    }

    public void RegisterInteractable(RegisterInteractPosition ip)
    {
        NPCInteractable pos = new NPCInteractable(posIDs, ip.type, ip.gameObject);
        if (ip.type != DoingType.Home)
        {
            interactables.Add(pos);
            posIDs++;
        }
        else
        {
            pos.id = 0;
            Home = pos;
        }
    }

    public NPCInteractable ClaimWorkPosition()
    {
        NPCInteractable ip = RandomPosOfType(DoingType.Work);
        ip.inUse = true;
        return ip;
    }

    public NPCInteractable RandomPosOfType(DoingType type)
    {
        List<NPCInteractable> ps = new List<NPCInteractable>();
        foreach(NPCInteractable p in interactables)
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
        return new NPCInteractable(999, type, gameObject);
    }
}
