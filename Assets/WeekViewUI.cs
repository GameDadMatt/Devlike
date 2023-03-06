using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Timing;

namespace Devlike.UI
{
    public class WeekViewUI : MonoBehaviour
    {
        [SerializeField]
        private Rect displayArea;
        private Vector2 Midday { get { return displayArea.center; } }
    }
}