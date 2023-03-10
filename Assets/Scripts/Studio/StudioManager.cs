using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;
using Devlike.Tasks;
using Devlike.UI;

/// <summary>
/// Manages the studio, the project, and the characters
/// </summary>
public class StudioManager : MonoBehaviour
{
    public static StudioManager instance;

    [SerializeField]
    private GameObject characterPrefab;
    public List<Character> Characters { get; private set; } = new List<Character>();

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
        List<Profile> profiles = RandomGeneration.instance.RandomProfiles(GlobalVariables.value.StudioSize);
        for (int i = 0; i < GlobalVariables.value.StudioSize; i++)
        {
            GameObject newchar = Instantiate(characterPrefab);
            newchar.transform.position = InteractableManager.instance.Home.thing.transform.position;
            Characters.Add(newchar.GetComponent<Character>());
            Characters[i].SetupCharacter(profiles[i]);
        }

        //Update the UI with all of the characters
        GameplayUI.instance.GenerateCharacterButtons();
    }

    /// <summary>
    /// Returns whether any characters are currently active in the scene
    /// </summary>
    /// <returns></returns>
    public bool CharactersActive
    {
        get
        {
            foreach (Character c in Characters)
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
