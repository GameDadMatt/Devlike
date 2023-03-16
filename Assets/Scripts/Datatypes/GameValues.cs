using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Values that are updated during gameplay
public static class GameValues
{
    public static int CurrentTick { get; set; }
    public static int CurrentDayInt { get; set;}
    public static Day CurrentDay { get { return (Day)CurrentDayInt; } }
    public static int CurrentWeek { get; set; }
    public static GameState CurrentState { get; set; }

    private static float HoursMinutes
    {
        get
        {
            float time = (float)CurrentTick / (float)StartingValues.value.TicksPerHour;
            return time;
        }
    }

    public static string CurrentTime
    {
        get
        {
            string ampm = "am";
            float h = Mathf.FloorToInt(HoursMinutes);
            float m = HoursMinutes % 1;
            m = 60 * m;
            if (h > 12)
            {
                h -= 12;
                ampm = "pm";
            }
            string hours = h.ToString();
            string minutes = m.ToString();
            if (hours.Length < 2)
            {
                hours = "0" + hours;
            }
            if (minutes.Length < 2)
            {
                minutes = "0" + minutes;
            }
            return hours + ":" + minutes + ampm;
        }
    }
}
