using UnityEngine;

namespace DataTypes
{
    //This class is specifically for Development tasks
    [CreateAssetMenu(fileName = "DevTask", menuName = "Devlike/Dev Task")]
    public class DevTask : Task, ITask
    {
        public TaskImportance importance = TaskImportance.None;
        public float baseBugChance = 0.05f;
        private float modBugChance = 0.05f;

        public void UpdateBugChance (float characterMod)
        {
            modBugChance = baseBugChance * characterMod;
        }

        public void GenerateBug()
        {
            Debug.LogWarning("Bug generated in " + name);
        }

        public new void DoTask()
        {
            doneTicks++;
            if (doneTicks > minTicks && doneTicks < maxTicks)
            {
                float complete = Random.Range(0f, 1f);
                if (complete <= completeChance)
                {
                    CompleteTask();
                }
            }
            else if (doneTicks >= maxTicks)
            {
                CompleteTask();
            }

            //Risk generating a bug every tick this task is incomplete
            float bug = Random.Range(0f, 1f);
            if(bug <= modBugChance)
            {
                GenerateBug();
            }
        }
    }
}