using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Characters;

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
            "\n Traits = " + p.Traits[0] + ", " + p.Traits[1] + ", " + p.Traits[2] +
            "\n RestMod " + p.RestDropMultiplier + ", FoodMod " + p.FoodDropMultiplier + ", InspMod " + p.InspDropMultiplier + ", SoclMod " + p.SoclDropMultiplier +
            "\n EmpathyMod " + p.EmpathyBarrierMultiplier + ", MoodMod " + p.MoodImpactMultiplier + ", DayStartMod " + p.WorkStartMod + ", DayEndMod " + p.WorkEndMod + ", VelocityMod " + p.VelocityMultiplier + ", BuglocityMod " + p.BuglocityMultiplier + ", BurnoutpMod " + p.BurnoutMultiplier +
            "\n PrgLike " + p.LikesProgramming + ", ArtLike " + p.LikesArt + ", AudLike " + p.LikesAudio + ", WrtLike " + p.LikesWriting + ", DesLike " + p.LikesDesign +
            "\n CURRENT VALUES" +
            "\n Rest " + c.Rest.curValue + ", Food " + c.Food.curValue + ", Insp " + c.Insp.curValue + ", Socl " + c.Socl.curValue +
            "\n RestBurn " + c.restBurnRate + ", FoodBurn " + c.foodBurnRate + ", InspBurn " + c.inspBurnRate + ", SoclBurn " + c.soclBurnRate;
        }
        else
        {
            output.text = "No Character Selected";
        }
    }
}
