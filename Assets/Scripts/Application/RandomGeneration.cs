using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;
using Devlike.Tasks;
using Devlike;

/// <summary>
/// Handles random generation of projects and characters
/// </summary>
public class RandomGeneration : ExecutableBehaviour
{
    public static RandomGeneration instance;

    //References
    [SerializeField]
    private GlobalGame gGame;
    [SerializeField]
    private GlobalTime gTime;
    [SerializeField]
    private GlobalStudio gStudio;
    [SerializeField]
    private GlobalProject gProject;
    [SerializeField]
    private GlobalCharacter gCharacter;
    [SerializeField]
    private GlobalDialogue gDialogue;

    public void Awake()
    {
        if(instance == null)
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
        Random.InitState(gGame.Seed.GetHashCode());
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
        List<Color> colors = BalancedRandomColors(num);
        for (int i = 0; i < num; i++)
        {
            profiles.Add(RandomProfile(exp[i], prof[i], RandomTraits(gCharacter.TotalTraits), colors[i]));
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

    public Tier RandomTier
    {
        get
        {
            return (Tier)Random.Range(0, 5);
        }
    }

    public TaskType RandomType
    {
        get
        {
            return (TaskType)Random.Range(0, 3);
        }
    }

    public int RandomRestoreTime
    {
        get
        {
            return Random.Range(gTime.MinNeedTicks, gTime.MaxNeedTicks + 1);
        }
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
            return Mathf.CeilToInt(gStudio.StudioExperienceTarget - AverageOfTiers(list));
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
        TaskType lastGenerated = gStudio.Weights.RandomFromWeights;
        for(int i = 0; i < num; i++)
        {
            //Ensure we don't repeatedly generate the same profession multiple times in a row
            TaskType random = gStudio.Weights.RandomFromWeights;
            while (random == lastGenerated)
            {
                random = gStudio.Weights.RandomFromWeights;
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
    private Profile RandomProfile(Tier exp, Profession prof, List<Trait> traits, Color color)
    {
        List<string> strings = RandomNameAndHobby();
        return new Profile(gCharacter, gTime, strings[0], strings[1], strings[2], strings[3], exp, prof, traits, color);
    }

    /// <summary>
    /// Generate a list of names and hobbies
    /// </summary>
    /// <returns></returns>
    private List<string> RandomNameAndHobby()
    {
        List<string> strings = new List<string>();
        strings.Add(CodeValues.FirstNames[Random.Range(0, CodeValues.FirstNames.Count)]);
        strings.Add(CodeValues.LastNames[Random.Range(0, CodeValues.LastNames.Count)]);
        strings.Add(CodeValues.Nicknames[Random.Range(0, CodeValues.Nicknames.Count)]);
        strings.Add(CodeValues.Hobbies[Random.Range(0, CodeValues.Hobbies.Count)]);
        return strings;
    }

    /// <summary>
    /// Returns a list of colors based on the gradient contained in Global Variables. Tries to keep colours spread.
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private List<Color> BalancedRandomColors(int num)
    {
        float r = 1f / num;
        List<Color> colors = new List<Color>();
        for(int i = 0; i < num; i++)
        {
            colors.Add(gCharacter.CharacterColours.Evaluate(Random.Range(r * i, r * (i + 1))));
        }

        return colors;
    }


    /// <summary>
    /// Generate a random profession from a given primary type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Profession RandomProfessionOftype(TaskType type)
    {
        List<Profession> professions = gCharacter.Professions;
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
        List<Trait> traits = gCharacter.Traits;
        List<Trait> selectedTraits = new List<Trait>();
        int i = 0;
        while(selectedTraits.Count < gCharacter.TotalTraits)
        {
            i = Random.Range(0, traits.Count);
            if (!selectedTraits.Contains(traits[i]))
            {
                selectedTraits.Add(traits[i]);
            }
        }

        return selectedTraits;
    }

    public List<TaskList> RandomProjectScope(int num)
    {
        float art = Random.Range(gStudio.Weights.art, gStudio.Weights.art * 1.3f);
        float des = Random.Range(gStudio.Weights.des, gStudio.Weights.des * 1.3f);
        float eng = Random.Range(gStudio.Weights.eng, gStudio.Weights.eng * 1.3f);
        ChanceWeights cw = new ChanceWeights(art, des, eng);

        List<TaskList> output = new List<TaskList>();
        output.Add(GenerateTaskList(TaskType.Art, Mathf.CeilToInt(num * cw.ArtWeightPercent)));
        output.Add(GenerateTaskList(TaskType.Design, Mathf.CeilToInt(num * cw.DesWeightPercent)));
        output.Add(GenerateTaskList(TaskType.Engineering, Mathf.CeilToInt(num * cw.EngWeightPercent)));

        return output;
    }

    private TaskList GenerateTaskList(TaskType type, int total)
    {
        Debug.Log("Generating " + total + " points of tasks");
        int cur = total;
        Queue<int> taskPoints = new Queue<int>();
        while (cur > 0)
        {
            int points = RandomPoints(cur);
            taskPoints.Enqueue(points);
            cur -= points;
        }
        return new TaskList(total, taskPoints);
    }

    public int RandomPoints(int limit)
    {
        int points = Random.Range(1, gProject.MaxTaskPoints);
        if (points >= limit)
        {
            points = limit;
        }
        return points;
    }

    public bool RandomGenerateBug(float taskChance, float characterChance)
    {
        float chance = (taskChance + characterChance) / 2;
        bool bug = Random.Range(0f, 1f) < chance;
        if (bug)
        {
            gProject.AddTask(new TaskContainer(RandomType, TaskImportance.Bug, Tier.Lowest, gProject.BaseBugChance, RandomPoints(gProject.MaxTaskPoints)));
        }

        return bug;
    }

    public Queue<DialogueCollection> ArtificialDramaQueue()
    {
        int num = Mathf.RoundToInt(Random.Range(gDialogue.MinWeeklyArtificialDramas, gDialogue.MaxWeeklyArtificialDramas));
        Queue<DialogueCollection> dramas = new Queue<DialogueCollection>();
        for(int i = 0; i < num; i++)
        {
            DialogueCollection drama = gDialogue.Dialogues[Random.Range(0, gDialogue.Dialogues.Count)];
            dramas.Enqueue(drama);
        }
        return dramas;
    }

    public List<int> RandomPositionsFromList(int num, int listCount)
    {
        if (num > listCount)
        {
            Debug.LogWarning("Unable to return " + num + " values, as list is only " + listCount + " long. Returning maximum of " + listCount + ".");
            num = listCount;
        }

        List<int> ints = new List<int>();
        for (int i = 0; i < num; i++)
        {
            int random = Random.Range(0, listCount);
            ints.Add(random);
        }
        return ints;
    }

    public List<int> RandomUnrepeatedPositionsFromList(int num, int listCount)
    {
        if(num > listCount)
        {
            Debug.LogWarning("Unable to return " + num + " values, as list is only " + listCount + " long. Returning maximum of " + listCount + ".");
            num = listCount;
        }

        List<int> ints = new List<int>();
        for (int i = 0; i < num; i++)
        {
            int random = Random.Range(0, listCount);
            while (ints.Contains(random))
            {
                random = Random.Range(0, listCount);
            }
            ints.Add(random);
        }
        return ints;
    }
}
