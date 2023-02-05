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
        p = c.characterProfile;
        selected = true;
    }

    public void Update()
    {
        if (selected)
        {
            output.text = "PROFILE" +
            "\n Traits = " + p.Traits[0] + ", " + p.Traits[1] + ", " + p.Traits[2] +
            "\n RestMod " + p.restDropMultiplier + ", FoodMod " + p.foodDropMultiplier + ", InspMod " + p.inspirationDropMultiplier + ", SoclMod " + p.socialDropMultiplier +
            "\n EmpathyMod " + p.empathyBarrierMultiplier + ", MoodMod " + p.moodImpactMultiplier + ", DayStartMod " + p.dayStartMod + ", DayEndMod " + p.dayEndMod + ", VelocityMod " + p.velocityMultiplier + ", BuglocityMod " + p.buglocityMultiplier + ", BurnoutpMod " + p.burnoutMultiplier +
            "\n PrgLike " + p.likesProgramming + ", ArtLike " + p.likesArt + ", AudLike " + p.likesAudio + ", WrtLike " + p.likesWriting + ", DesLike " + p.likesDesign +
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
