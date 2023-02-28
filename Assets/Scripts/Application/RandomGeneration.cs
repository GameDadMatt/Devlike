using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;
using Devlike.Tasks;

/// <summary>
/// Handles random generation of projects and characters
/// </summary>
public class RandomGeneration : MonoBehaviour
{
    public static RandomGeneration instance;

    [SerializeField]
    private string seed;
    public string Seed { get { return seed; } }

    [SerializeField]
    private float artChance;
    [SerializeField]
    private float desChance;
    [SerializeField]
    private float engChance;

    private TaskType RandomType
    {
        get
        {
            float total = artChance + desChance + engChance;
            float val = Random.Range(0, total);
            if(val < artChance)
            {
                return TaskType.Art;
            }
            else if (val < artChance + desChance)
            {
                return TaskType.Design;
            }
            else
            {
                return TaskType.Engineering;
            }
        }
    }

    private int p = 0;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        Random.InitState(seed.GetHashCode());
    }

    /// <summary>
    /// Generates a number of random profiles
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public List<Profile> RandomProfiles(int num)
    {

    }

    private List<Tier> AverageExperience(int num)
    {

    }

    private List<Profession> BalancedProfessions(int num)
    {

    }

    private Profile RandomProfile(Tier exp, Profession prof, List<Trait> traits)
    {
        string firstName = "Dev " + p;
        string lastName = "Person";
        string nickName = "Mint";
        string hobby = "Painting";
        p++;

        return new Profile(firstName, lastName, nickName, hobby, exp, prof, traits);
    }

    private string RandomProfessionOftype(TaskType type)
    {
        List<Profession> professions = GlobalVariables.value.allProfessions;
        bool selected = false;
        int i = 0;

        while (!selected)
        {
            i = Random.Range(0, professions.Count);
            if (professions[i].primarySkill.type == type)
            {
                selected = true;
            }
        }

        return professions[i];
    }

    private List<Trait> RandomTraits(int total)
    {
        List<Trait> traits = GlobalVariables.value.allTraits;
        List<Trait> selectedTraits = new List<Trait>();
        int i = 0;
        while(selectedTraits.Count < GlobalVariables.value.totalTraits)
        {
            i = Random.Range(0, traits.Count);
            if (!selectedTraits.Contains(traits[i]))
            {
                selectedTraits.Add(traits[i]);
            }
        }

        return selectedTraits;
    }
}
