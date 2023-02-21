using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.UI
{
    public class DragDropArea : MonoBehaviour
    {
        private void OnEnable()
        {
            DragTaskManager.instance.RegisterDropArea(this);
        }

        private void OnDisable()
        {
            DragTaskManager.instance.UnregisterDropArea(this);
        }
    }
}