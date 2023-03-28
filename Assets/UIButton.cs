using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.UI
{
    public class UIButton : MonoBehaviour
    {
        public void CloseWindow()
        {
            Debug.Log("Closing window");
            EventManager.instance.CloseUI();
        }
    }
}