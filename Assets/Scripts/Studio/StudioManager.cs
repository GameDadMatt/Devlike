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

    private Queue<DialogueCollection> weekDramas = new Queue<DialogueCollection>();
    private List<int> dramaDays = new List<int>();

    protected override void SetListeners()
    {
        EventManager.instance.OnTick += Tick;
        EventManager.instance.OnDayStart += DayStart;
        EventManager.instance.OnWeekStart += WeekStart;
    }

    protected override void SetProperties()
    {
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
            characters[i].CharacterDialogue.SetDefault(gDialogue.DefaultDialogue); //Give the character a default dialogue
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
                    DialogueCollection dialogue = weekDramas.Dequeue();
                    availableCharacters[i].SetMoodAndDialogue(dialogue.moodletType, new DialogueContainer(dialogue));
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

    public List<Character> CharactersWithoutDrama
    {
        get
        {
            List<Character> availableCharacters = new List<Character>();
            foreach(Character character in gStudio.Characters)
            {
                if(character.CharacterDialogue.CurrentDialogue.dramaType == DialogueType.None)
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
                if (character.CharacterDialogue.CurrentDialogue.dramaType != DialogueType.None)
                {
                    dramaCharacters.Add(character);
                }
            }
            return dramaCharacters;
        }
    }
}
