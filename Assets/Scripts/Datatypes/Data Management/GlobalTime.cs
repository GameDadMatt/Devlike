using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;

namespace Devlike
{
    [CreateAssetMenu(fileName = "Time", menuName = "Devlike/Properties/Time")]
    public class GlobalTime : GlobalObject
    {
        //SERIALIZED
        [SerializeField]
        private int currentTick, startTick;
        [SerializeField]
        private int currentDayInt, startDayInt;
        [SerializeField]
        private int currentWeek, startWeek;

        [SerializeField]
        private float tickLength, fastTickLength;
        [SerializeField]
        private int ticksPerHour, workStartHour, workEndHour, dayEndHour;
        [SerializeField]
        private int timeUnitTicks, minNeedRestoreTicks, maxNeedRestoreTicks;

        public int CurrentTick { get { return currentTick; } set { currentTick = value; } }
        public int CurrentDayInt { get { return currentDayInt; } set { currentDayInt = value; } }
        public int CurrentWeek { get { return currentWeek; } set { currentWeek = value; } }

        public float TickLength { get => tickLength; }
        public float FastTickLength { get => fastTickLength; }
        public int TicksPerHour { get => ticksPerHour; }
        public int WorkStartTick { get { return workStartHour * ticksPerHour; } }
        public int WorkEndTick { get { return workEndHour * ticksPerHour; } }
        public int DayEndTick { get { return dayEndHour * ticksPerHour; } }
        public int MinNeedTicks { get => minNeedRestoreTicks; }
        public int MaxNeedTicks { get => maxNeedRestoreTicks; }
        public Day CurrentDay { get { return (Day)CurrentDayInt; } }

        private float HoursMinutes
        {
            get
            {
                float time = (float)CurrentTick / (float)TicksPerHour;
                return time;
            }
        }

        public string CurrentTime
        {
            get
            {
                string ampm = "am";
                float h = Mathf.FloorToInt(HoursMinutes);
                float m = HoursMinutes % 1;
                m = Mathf.Round(60 * m);
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

        public override void ResetValues()
        {
            currentTick = startTick;
            currentDayInt = startDayInt;
            currentWeek = startWeek;
        }
    }
}
