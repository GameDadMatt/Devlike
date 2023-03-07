using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Timing;

namespace Devlike.UI
{
    public class WeekViewUI : MonoBehaviour
    {
        public static WeekViewUI instance;

        [SerializeField]
        private GameObject dayPrefab;
        [SerializeField]
        private RectTransform displayArea;
        private Vector2 AreaSize { get { return displayArea.rect.size; } }
        private float MoveAmount { get { return AreaSize.x / GlobalVariables.value.DayEndTick; } }
        private float DayWidth { get { return dayPrefab.GetComponent<RectTransform>().sizeDelta.x; } }
        private float DayVerticalOffset { get { return dayPrefab.GetComponent<RectTransform>().sizeDelta.y / 2; } }
        private float Midday { get { return AreaSize.x / 2; } }
        private float Midnight { get { return (DayWidth / 2) * - 1; } }
        private List<GameObject> spawnedDays = new List<GameObject>();

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        public void Setup(Day prev, Day cur, Day next)
        {
            TimeManager.instance.OnTick += OnTick;

            SpawnDay(prev);
            SpawnDay(cur);
            SpawnDay(next);
        }

        private void OnTick()
        {
            for (int i = 0; i < spawnedDays.Count; i++)
            {
                MoveDay(spawnedDays[i], i);
            }

            RectTransform rtzero = spawnedDays[0].GetComponent<RectTransform>();
            if (rtzero.anchoredPosition.x <= Midnight)
            {
                Debug.Log(rtzero.gameObject.name + "Being removed because it is at position " + rtzero.anchoredPosition);
                DespawnDay();
            }
        }

        private void MoveDay(GameObject day, int lpos)
        {
            RectTransform rt = day.GetComponent<RectTransform>();
            rt.anchoredPosition = NewPosition(lpos);
        }

        private void SpawnDay(Day day)
        {
            Debug.Log("Spawning a new day");

            GameObject newDay = Instantiate(dayPrefab, displayArea.transform);
            DayAreaDisplay disp = newDay.GetComponent<DayAreaDisplay>();
            disp.Setup(day.ToString());
            //Offset the position of this new day
            if(spawnedDays.Count > 0)
            {
                Vector2 pos = spawnedDays[spawnedDays.Count - 1].GetComponent<RectTransform>().anchoredPosition;
                pos.x += DayWidth;
                newDay.GetComponent<RectTransform>().anchoredPosition = pos;
            }
            else
            {
                Vector2 pos = new Vector2(0f, -25f);
                newDay.GetComponent<RectTransform>().anchoredPosition = pos;
            }            
            //Add the day to the spawned days
            spawnedDays.Add(newDay);
        }

        private Vector2 NewPosition(int pos)
        {
            float dayCenter = DayWidth / 2;
            float dayOffset = pos * DayWidth;
            return new Vector2(PercentToPos + dayCenter + dayOffset, DayVerticalOffset);
        }

        private float PercentToPos
        {
            get
            {
                float pos = AreaSize.x;
                pos *= TimeManager.instance.ProgressToNextDay * - 1;
                return pos;
            }
        }

        private void DespawnDay()
        {
            GameObject day = spawnedDays[0];
            spawnedDays.RemoveAt(0);
            Destroy(day);
        }
    }
}