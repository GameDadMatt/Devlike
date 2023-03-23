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
    private GlobalStudio studio;
    private GlobalDialogue dialogue;
    private GlobalProject project;

    [SerializeField]
    private GameObject characterPrefab;

    protected override void SetProperties()
    {
        studio = GameManager.instance.GetGlobal("Studio") as GlobalStudio;
        dialogue = GameManager.instance.GetGlobal("Dialogue") as GlobalDialogue;
        project = GameManager.instance.GetGlobal("Project") as GlobalProject;
    }

    protected override void SetListeners()
    {
        EventManager.instance.OnTick += Tick;
        EventManager.instance.OnDayStart += DayStart;
        EventManager.instance.OnWeekStart += WeekStart;
    }

    protected override void Launch()
    {
        SpawnCharacters();
    }

    public void SpawnCharacters()
    {
        List<Profile> profiles = RandomGeneration.instance.RandomProfiles(studio.StudioSize);
        List<Character> characters = new List<Character>();

        for (int i = 0; i < studio.StudioSize; i++)
        {
            GameObject newchar = Instantiate(characterPrefab);
            newchar.transform.position = InteractableManager.instance.Home.thing.transform.position;
            characters.Add(newchar.GetComponent<Character>());
            characters[i].SetupCharacter(profiles[i]);
            characters[i].CurrentDialogue = dialogue.DefaultDialogue; //Give the character a default dialogue
            newchar.name = characters[i].Profile.FullName;
        }

        studio.SetCharacters(characters);
        //Our characters have been set
        EventManager.instance.SetCharacters();
    }

    private void DayStart()
    {

    }

    private void WeekStart()
    {

    }

    private void Tick()
    {
        //Debug.Log(studio.Alignment);
    }
}
