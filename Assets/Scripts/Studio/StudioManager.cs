using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;
using Devlike;

/// <summary>
/// Manages the studio, the project, and the characters
/// </summary>
public class StudioManager : ExecutableBehaviour
{
    [SerializeField]
    private GameObject characterPrefab;

    protected override void Launch()
    {
        SpawnCharacters();
    }

    public void SpawnCharacters()
    {
        List<Profile> profiles = RandomGeneration.instance.RandomProfiles(StartingValues.value.StudioSize);
        for (int i = 0; i < StartingValues.value.StudioSize; i++)
        {
            GameObject newchar = Instantiate(characterPrefab);
            newchar.transform.position = InteractableManager.instance.Home.thing.transform.position;
            GameValues.Characters.Add(newchar.GetComponent<Character>());
            GameValues.Characters[i].SetupCharacter(profiles[i]);
            GameValues.Characters[i].CurrentDialogue = StartingValues.value.DefaultDialogue; //Give the character a default dialogue
            newchar.name = GameValues.Characters[i].Profile.FullName;
        }

        //Our characters have been set
        EventManager.instance.SetCharacters();
    }
}
