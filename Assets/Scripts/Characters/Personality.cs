using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;

[CreateAssetMenu(fileName = "Personality", menuName = "Devlike/Personality")]
public class Personality : ScriptableObject
{
    public PersonalityType personalityType;
    public float restThreshold;
    public float foodThreshold;
    public float thinkThreshold;
    public float prefersTalkOrThink;
    public float workHourFlexibility;
}
