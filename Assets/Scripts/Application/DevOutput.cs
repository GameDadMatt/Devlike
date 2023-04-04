using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Devlike.Characters;
using Devlike;

public class DevOutput : MonoBehaviour
{
    [SerializeField]
    private Canvas devConsole;
    private Character c;
    private Profile p;
    private CharacterTasker t;
    private bool selected = false;
    public TextMeshProUGUI output;

    public void SetupDevConsole()
    {
        EventManager.instance.OnCharacterSelect += SelectCharacter;
    }

    public void SelectCharacter(Character click)
    {
        Debug.Log("Character Selected");
        c = click;
        p = c.Profile;
        t = c.CharacterTasker;
        selected = true;

        if (!devConsole.enabled)
        {
            devConsole.enabled = true;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.Log("~");
            devConsole.enabled = !devConsole.enabled;
        }

        if (selected)
        {
            string disp;
            disp = "PROFILE OF " + p.FirstName + "  " + p.LastName + ", a " + p.Profession.name + ". Alias is " + p.Nickname + ". Likes " + p.Hobby + ". \n Traits = ";
            foreach(string trait in p.TraitNames)
            {
                disp += " " + trait + ", ";
            }
            disp += "\n RestMod " + p.RestDropMultiplier + ", FoodMod " + p.FoodDropMultiplier + ", InspMod " + p.InspDropMultiplier + ", SoclMod " + p.SoclDropMultiplier +
            "\n RestBurn " + c.RestBurnRate + ", FoodBurn " + c.FoodBurnRate + ", InspBurn " + c.InspBurnRate + ", SoclBurn " + c.SoclBurnRate +
            "\n EmpathyMod " + p.EmpathyBarrierMultiplier + ", MoodMod " + p.MoodImpactMultiplier + ", DayStartMod " + p.WorkStartMod + ", DayEndMod " + p.WorkEndMod + ", BurnoutpMod " + p.BurnoutMultiplier +
            "\n ArtSkill " + p.Art + ", EngineeringSkill " + p.Engineering + ", DesignSkill " + p.Design + ", VelocityMultiplier " + p.VelocityMultiplier + ", BugChanceMultiplier " + p.BugChanceMultiplier +
            "\n CURRENT VALUES" +
            "\n Rest " + c.Rest.curValue + ", Food " + c.Food.curValue + ", Insp " + c.Insp.curValue + ", Socl " + c.Socl.curValue + ", CappedMoodImpact " + c.CappedMoodImpact +
            "\n Velocity " + t.Velocity + ", Alignment " + t.Alignment + ", CrunchPressure " + t.CrunchPressure +
            "\n THRESHOLDS" +
            "\n Crunch " + c.CrunchThreshold + ", Good Mood " + c.GoodMoodThreadhold + ", Bad Mood " + c.BadMoodThreshold + ", Overwhelmed " + c.OverwhelmedThreshold + ", Low Velocity " + c.LowVelocityThreshold;

            output.text = disp;
        }
        else if (!selected)
        {
            output.text = "No Character Selected";
        }
    }
}
