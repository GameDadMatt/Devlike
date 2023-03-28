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
    private GlobalStudio gStudio;
    [SerializeField]
    private GlobalDialogue gDialogue;
    [SerializeField]
    private GlobalProject gProject;
    [SerializeField]
    private GlobalTime gTime;

    [SerializeField]
    private GameObject characterPrefab;

    private Queue<DialogueContainer> weekDramas = new Queue<DialogueContainer>();
    private List<int> dramaDays = new List<int>();

    protected override void SetListeners()
    {
        EventManager.instance.OnTick += Tick;
        EventManager.instance.OnDayStart += DayStart;
        EventManager.instance.OnWeekStart += WeekStart;
    }

    protected override void SetProperties()
    {
        Debug.Log("TEST");
        gProject.GenerateProjectScope(gStudio.StudioSize);
    }

    protected override void Launch()
    {
        SpawnCharacters();
    }

    public void SpawnCharacters()
    {
        List<Profile> profiles = RandomGeneration.instance.RandomProfiles(gStudio.StudioSize);
        List<Character> characters = new List<Character>();

        for (int i = 0; i < gStudio.StudioSize; i++)
        {
            GameObject newchar = Instantiate(characterPrefab);
            newchar.transform.position = InteractableManager.instance.Home.thing.transform.position;
            characters.Add(newchar.GetComponent<Character>());
            characters[i].SetupCharacter(profiles[i]);
            characters[i].Dialogue.SetDialogue(gDialogue.DefaultDialogue); //Give the character a default dialogue
            newchar.name = characters[i].Profile.FullName;
        }

        gStudio.SetCharacters(characters);

        //As it's an initial setup, generate dramas
        WeekStart();
        DayStart();

        //Our characters have been set
        EventManager.instance.SetCharacters();
    }

    private void Tick()
    {
        //Debug.Log(studio.Alignment);
    }

    private void DayStart()
    {
        Debug.LogWarning("Day Start: Check character moods");
        SetNaturalCharacterMoods();
        if (dramaDays.Contains(gTime.CurrentDayInt))
        {
            Debug.LogWarning("Dramas!");
            List<Character> availableCharacters = CharactersWithoutDrama;
            if (availableCharacters.Count > 0)
            {
                List<int> dramas = new List<int>();
                for (int i = 0; i < dramaDays.Count; i++)
                {
                    if (dramaDays[i] == gTime.CurrentDayInt)
                    {
                        dramas.Add(i);
                    }
                }
                List<int> characterPos = RandomGeneration.instance.RandomUnrepeatedPositionsFromList(dramas.Count, availableCharacters.Count);
                foreach(int i in characterPos)
                {
                    availableCharacters[i].Dialogue.SetDialogue(weekDramas.Dequeue());
                }
            }
        }
    }

    private void WeekStart()
    {
        Debug.LogWarning("Week Start");
        weekDramas = RandomGeneration.instance.ArtificialDramaQueue();
        dramaDays = RandomGeneration.instance.RandomPositionsFromList(weekDramas.Count, 7);
    }

    public void SetNaturalCharacterMoods()
    {
        foreach(Character character in gStudio.Characters)
        {
            character.CheckMood();
        }
    }

    public List<Character> CharactersWithoutDrama
    {
        get
        {
            List<Character> availableCharacters = new List<Character>();
            foreach(Character character in gStudio.Characters)
            {
                if(character.Dialogue.CurrentDialogue.dramaType == DramaType.None)
                {
                    availableCharacters.Add(character);
                }
            }
            return availableCharacters;
        }
    }

    public List<Character> CharactersWithDrama
    {
        get
        {
            List<Character> dramaCharacters = new List<Character>();
            foreach (Character character in gStudio.Characters)
            {
                if (character.Dialogue.CurrentDialogue.dramaType != DramaType.None)
                {
                    dramaCharacters.Add(character);
                }
            }
            return dramaCharacters;
        }
    }
}
