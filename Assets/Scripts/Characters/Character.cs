using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;
using BehaviorDesigner.Runtime;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        //Personality
        public Profile profile = new Profile();

        //Tasks
        public List<Task> TaskList = new List<Task>();
        public TaskType CurrentTask { get; private set; }
        public EmotionType CurrentEmotion { get; private set; }
        public IncidentType RememberedIncident { get; private set; }

        //Needs
        public Need Rest { get; private set; } = new Need(NeedType.Rest, 1f, 0.3f);
        public Need Food { get; private set; } = new Need(NeedType.Food, 1f, 0.3f);
        public Need Insp { get; private set; } = new Need(NeedType.Inspiration, 1f, 0.3f);
        public Need Socl { get; private set; } = new Need(NeedType.Social, 1f, 0.3f);

        public float restBurnRate { get { return GlobalVariables.value.baseRestBurnTime * profile.restDropMultiplier; } }
        public float foodBurnRate { get { return GlobalVariables.value.baseFoodBurnTime * profile.foodDropMultiplier; } }
        public float inspBurnRate { get { return GlobalVariables.value.baseInspirationBurnTime * profile.inspirationDropMultiplier; } }
        public float soclBurnRate { get { return GlobalVariables.value.baseSocialBurnTime * profile.socialDropMultiplier; } }

        //Days
        public int dayStart { get { return GlobalVariables.value.dayStartTick; } }
        public int dayEnd { get { return GlobalVariables.value.dayEndTick; } }

        public void Start()
        {
            TimeManager.instance.OnTick += LowerNeeds;
            TimeManager.instance.OnDayStart += SetNeeds;
            profile.SetupProfile();
        }

        public void OnMouseOver()
        {
            Debug.Log("Mouse Over");
            if (Input.GetMouseButtonDown(0))
            {
                GameManager.instance.CharacterSelect(this);
            }
        }

        public void SetNeeds()
        {
            Rest.curValue = Random.Range(0.7f, 1f);
            Food.curValue = Random.Range(0.8f, 1f);
            Insp.curValue = Random.Range(0.1f, 1f);
            Socl.curValue = Random.Range(0.1f, 1f);
        }

        public void LowerNeeds()
        {
            Rest.curValue -= restBurnRate;
            Food.curValue -= foodBurnRate;
            Insp.curValue -= inspBurnRate;
            Socl.curValue -= soclBurnRate;
        }

        public void NeedTask(NeedType need)
        {
            throw new System.NotImplementedException();
        }

        public void RestoreNeed(NeedType need)
        {
            switch (need)
            {
                case NeedType.Rest:
                    Rest.curValue = 1f;
                    break;
                case NeedType.Food:
                    Food.curValue = 1f;
                    break;
                case NeedType.Inspiration:
                    Insp.curValue = 1f;
                    break;
                case NeedType.Social:
                    Socl.curValue = 1f;
                    break;
            }
        }
    }
}
