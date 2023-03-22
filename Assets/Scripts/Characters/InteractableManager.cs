using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Devlike.Characters;
using Devlike;

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

public class InteractableManager : ExecutableBehaviour
{
    //InteractableManager still needs to be a singleton for easy access from the BehaviourTree
    public static InteractableManager instance;

    private GlobalStudio studio;

    private int posIDs = 1;
    [SerializeField]
    public GameObject homePosition;
    public List<NPCInteractable> Interactables { get; private set; } = new List<NPCInteractable>();
    public NPCInteractable Home { get { return new NPCInteractable(0, DoingType.Home, homePosition); } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple copies of " + name + " detected");
        }
    }

    protected override void SetProperties()
    {
        studio = GameManager.instance.GetGlobal("Studio") as GlobalStudio;
    }

    protected override void SetListeners()
    {
        //Get the interactables after the characters have been set
        EventManager.instance.OnSetCharacters += GetInteractables;
    }

    protected override void AfterDelay()
    {
        Debug.Log("Doing after delay " + studio.Characters.Count);
        //Set character positions
        foreach(Character character in studio.Characters)
        {
            character.SetPositions();
        }
    }

    public event Action OnGetInteractables;
    private void GetInteractables()
    {
        Debug.Log("Get interactables");
        OnGetInteractables?.Invoke();
    }

    public void RegisterInteractable(InteractPosition ip)
    {
        NPCInteractable pos = new NPCInteractable(posIDs, ip.type, ip.gameObject);
        if (ip.type != DoingType.Home)
        {
            Interactables.Add(pos);
            posIDs++;
        }
        else
        {
            Debug.LogError("Home is already assigned");
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
        foreach(NPCInteractable p in Interactables)
        {
            if(p.type == type && !p.inUse)
            {
                ps.Add(p);
            }
        }

        if(ps.Count > 1)
        {
            int rand = UnityEngine.Random.Range(0, ps.Count);
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
