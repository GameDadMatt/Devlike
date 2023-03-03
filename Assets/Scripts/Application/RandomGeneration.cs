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
        List<Profile> profiles = new List<Profile>();
        List<Tier> exp = AverageExperience(num);
        List<Profession> prof = BalancedProfessions(num);
        for(int i = 0; i < num; i++)
        {
            profiles.Add(RandomProfile(exp[i], prof[i], RandomTraits(GlobalVariables.value.totalTraits)));
            string traits = "";
            foreach (string name in profiles[i].TraitNames)
            {
                traits += name + ", ";
            }
            Debug.Log("Profile " + i + " Name: " + profiles[i].FirstName + " " + profiles[i].LastName + ", Nickname: " + profiles[i].Nickname + ", Hobby: " + profiles[i].Hobby + "\n Experience: " + profiles[i].Experience + ", Profession: " + profiles[i].Profession + ", Traits: " + traits);
        }

        return profiles;
    }

    /// <summary>
    /// Generate a number of experience tiers to a desired average
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private List<Tier> AverageExperience(int num)
    {
        List<Tier> tiers = new List<Tier>();
        for (int i = 0; i < num; i++)
        {
            int min = TierMod(tiers);
            int r = Random.Range(min, 5);
            tiers.Add((Tier)r);
        }

        Debug.Log("Average is " + AverageOfTiers(tiers));

        return tiers;
    }

    /// <summary>
    /// Rounds up from the average of a given list
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private int TierMod(List<Tier> list)
    {
        if(list.Count > 0)
        {
            return Mathf.CeilToInt(GlobalVariables.value.StudioExpTarget - AverageOfTiers(list));
        }
        return 0;
    }

    /// <summary>
    /// Returns the average tier of a given list
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private float AverageOfTiers(List<Tier> list)
    {
        float average = 0;
        foreach(Tier t in list)
        {
            average += (int)t;
        }
        average /= list.Count;
        return average;
    }

    /// <summary>
    /// Generate a number of professions that tries to balance studio makeup
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private List<Profession> BalancedProfessions(int num)
    {
        List<Profession> professions = new List<Profession>();
        TaskType lastGenerated = RandomType;
        for(int i = 0; i < num; i++)
        {
            //Ensure we don't repeatedly generate the same profession multiple times in a row
            TaskType random = RandomType;
            while (random == lastGenerated)
            {
                random = RandomType;
            }

            lastGenerated = random;
            professions.Add(RandomProfessionOftype(random));
        }

        return professions;
    }

    /// <summary>
    /// Generate a random profile
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="prof"></param>
    /// <param name="traits"></param>
    /// <returns></returns>
    private Profile RandomProfile(Tier exp, Profession prof, List<Trait> traits)
    {
        List<string> strings = RandomNameAndHobby();
        return new Profile(strings[0], strings[1], strings[2], strings[3], exp, prof, traits);
    }

    /// <summary>
    /// Generate a list of names and hobbies
    /// </summary>
    /// <returns></returns>
    private List<string> RandomNameAndHobby()
    {
        List<string> strings = new List<string>();
        strings.Add(DataValues.FirstNames[Random.Range(0, DataValues.FirstNames.Count)]);
        strings.Add(DataValues.LastNames[Random.Range(0, DataValues.LastNames.Count)]);
        strings.Add(DataValues.Nicknames[Random.Range(0, DataValues.Nicknames.Count)]);
        strings.Add(DataValues.Hobbies[Random.Range(0, DataValues.Hobbies.Count)]);
        return strings;
    }

    /// <summary>
    /// Generate a random profession from a given primary type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Profession RandomProfessionOftype(TaskType type)
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

    /// <summary>
    /// Generate a list of random traits
    /// </summary>
    /// <param name="total"></param>
    /// <returns></returns>
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
