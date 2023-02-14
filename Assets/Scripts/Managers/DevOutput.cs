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
    public TextMeshProUGUI output;

    public void Start()
    {
        GameManager.instance.OnCharacterSelect += SelectCharacter;
    }

    public void SelectCharacter(Character click)
    {
        c = click;
        p = c.profile;
        selected = true;
    }

    public void Update()
    {
        if (selected)
        {
            output.text = "PROFILE" +
            "\n Traits = " + p.traitNames[0] + ", " + p.traitNames[1] + ", " + p.traitNames[2] +
            "\n RestMod " + p.RestDropMultiplier + ", FoodMod " + p.FoodDropMultiplier + ", InspMod " + p.InspDropMultiplier + ", SoclMod " + p.SoclDropMultiplier +
            "\n EmpathyMod " + p.EmpathyBarrierMultiplier + ", MoodMod " + p.MoodImpactMultiplier + ", DayStartMod " + p.WorkStartMod + ", DayEndMod " + p.WorkEndMod + ", VelocityMod " + p.VelocityMultiplier + ", BuglocityMod " + p.BugChanceMultiplier + ", BurnoutpMod " + p.BurnoutMultiplier +
            "\n PrgLike " + p.Programming + ", ArtLike " + p.Art + ", AudLike " + p.Audio + ", WrtLike " + p.Writing + ", DesLike " + p.Design +
            "\n CURRENT VALUES" +
            "\n Rest " + c.Rest.curValue + ", Food " + c.Food.curValue + ", Insp " + c.Insp.curValue + ", Socl " + c.Socl.curValue +
            "\n RestBurn " + c.RestBurnRate + ", FoodBurn " + c.FoodBurnRate + ", InspBurn " + c.InspBurnRate + ", SoclBurn " + c.SoclBurnRate;
        }
        else
        {
            output.text = "No Character Selected";
        }
    }
}
