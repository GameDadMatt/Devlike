using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;

public class StudioManager : MonoBehaviour
{
    public static StudioManager instance;
    public int studioSize = 0;
    [SerializeField]
    private GameObject characterPrefab;
    private List<Character> characters = new List<Character>();
    public TaskContainer Backlog { get; private set; } = new TaskContainer();

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        SpawnCharacters();
    }

    public void SpawnCharacters()
    {
        for (int i = 0; i < studioSize; i++)
        {
            GameObject newchar = Instantiate(characterPrefab);
            newchar.transform.position = InteractableManager.instance.Home.thing.transform.position;
            characters.Add(newchar.GetComponent<Character>());
        }
    }

    /// <summary>
    /// Returns whether any characters are currently active in the scene
    /// </summary>
    /// <returns></returns>
    public bool CharactersActive
    {
        get
        {
            foreach (Character c in characters)
            {
                if (c.CurrentState != CharacterState.Inactive)
                {
                    return true;
                }
            }
            return false;
        }        
    }
}
