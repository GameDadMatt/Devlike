using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Devlike.Characters;

public class DevOutput : MonoBehaviour
{
    private Character c;
    private Profile p;
    private bool selected = false;
    private bool displayed = false;
    public TextMeshProUGUI output;

    public void Start()
    {
        GameManager.instance.OnCharacterSelect += SelectCharacter;
    }

    public void SelectCharacter(Character click)
    {
        c = click;
        p = c.Profile;
        selected = true;
        displayed = false;
    }

    public void Update()
    {
        if (selected && !displayed)
        {
            string disp;
            disp = "PROFILE" + "\n Traits = ";
            foreach(string trait in p.TraitNames)
            {
                disp += " " + trait + ", ";
            }
            disp += "\n RestMod " + p.RestDropMultiplier + ", FoodMod " + p.FoodDropMultiplier + ", InspMod " + p.InspDropMultiplier + ", SoclMod " + p.SoclDropMultiplier +
            "\n EmpathyMod " + p.EmpathyBarrierMultiplier + ", MoodMod " + p.MoodImpactMultiplier + ", DayStartMod " + p.WorkStartMod + ", DayEndMod " + p.WorkEndMod + ", VelocityMod " + p.VelocityMultiplier + ", BuglocityMod " + p.BugChanceMultiplier + ", BurnoutpMod " + p.BurnoutMultiplier +
            "\n ArtSkill " + p.Art + ", EngineeringSkill " + p.Engineering + ", DesignSkill " + p.Design +
            "\n CURRENT VALUES" +
            "\n Rest " + c.Rest.curValue + ", Food " + c.Food.curValue + ", Insp " + c.Insp.curValue + ", Socl " + c.Socl.curValue +
            "\n RestBurn " + c.RestBurnRate + ", FoodBurn " + c.FoodBurnRate + ", InspBurn " + c.InspBurnRate + ", SoclBurn " + c.SoclBurnRate;

            output.text = disp;
            displayed = true;
        }
        else
        {
            output.text = "No Character Selected";
        }
    }
}
