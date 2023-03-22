using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;
using Devlike.Tasks;
using Devlike.Project;
using Devlike;

/// <summary>
/// Handles random generation of projects and characters
/// </summary>
public class RandomGeneration : ExecutableBehaviour
{
    public static RandomGeneration instance;

    private GlobalGame game;
    private GlobalStudio studio;
    private GlobalCharacter character;
    private GlobalTime time;
    private GlobalProject project;

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
        game = GameManager.instance.GetGlobal("Game") as GlobalGame;
        studio = GameManager.instance.GetGlobal("Studio") as GlobalStudio;
        character = GameManager.instance.GetGlobal("Character") as GlobalCharacter;
        time = GameManager.instance.GetGlobal("Time") as GlobalTime;
        project = GameManager.instance.GetGlobal("Project") as GlobalProject;

        Random.InitState(game.Seed.GetHashCode());
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
            profiles.Add(RandomProfile(exp[i], prof[i], RandomTraits(character.TotalTraits), colors[i]));
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
            return Random.Range(time.MinNeedTicks, time.MaxNeedTicks + 1);
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
            return Mathf.CeilToInt(studio.StudioExperienceTarget - AverageOfTiers(list));
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
        TaskType lastGenerated = studio.Weights.RandomFromWeights;
        for(int i = 0; i < num; i++)
        {
            //Ensure we don't repeatedly generate the same profession multiple times in a row
            TaskType random = studio.Weights.RandomFromWeights;
            while (random == lastGenerated)
            {
                random = studio.Weights.RandomFromWeights;
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
        return new Profile(character, time.TicksPerHour, strings[0], strings[1], strings[2], strings[3], exp, prof, traits, color);
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
    /// Generate a random color from the gradient contained on GlobalVariables
    /// </summary>
    /// <returns></returns>
    private Color RandomColor
    {
        get
        {
            return character.CharacterColours.Evaluate(Random.Range(0f, 1f));
        }
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
            colors.Add(character.CharacterColours.Evaluate(Random.Range(r * i, r * (i + 1))));
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
        List<Profession> professions = character.Professions;
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
        List<Trait> traits = character.Traits;
        List<Trait> selectedTraits = new List<Trait>();
        int i = 0;
        while(selectedTraits.Count < character.TotalTraits)
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
        float art = Random.Range(studio.Weights.art, studio.Weights.art * 1.3f);
        float des = Random.Range(studio.Weights.des, studio.Weights.des * 1.3f);
        float eng = Random.Range(studio.Weights.eng, studio.Weights.eng * 1.3f);
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
        int points = Random.Range(1, project.MaxTaskPoints);
        if (points >= limit)
        {
            points = limit;
        }
        return points;
    }

    public void RandomGenerateBug(float taskChance, float characterChance)
    {
        float chance = (taskChance + characterChance) / 2;
        bool bug = Random.Range(0f, 1f) > chance;
        if (bug)
        {
            StudioProject.instance.AddTask(new TaskContainer(RandomType, TaskImportance.Bug, Tier.Lowest, project.BaseBugChance, RandomPoints(project.MaxTaskPoints)));
        }
    }
}
