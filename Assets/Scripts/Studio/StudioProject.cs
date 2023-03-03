using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets up the project the studio will be undertaking, and tracks its completion
/// </summary>
public class StudioProject : MonoBehaviour
{
    [SerializeField]
    private int DesiredProjectDays;
    [SerializeField]
    private int ExpectedTasksPerPersonPerDay;

    //Project Scope = (Team Num * Base Velocity) * Total Project Days
    //Base Velocity = How many tasks per day???
    public int ProjectScope { get { return (GlobalVariables.value.StudioSize * ExpectedTasksPerPersonPerDay) * DesiredProjectDays; } }

    //GENERATE RANDOM TOTAL TASK NUMBERS AND TASK SIZE
    //REDUCE TOTAL BY NUMBER PULLED INTO BACKLOG
}
