using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropArea : MonoBehaviour
{
    private void OnEnable()
    {
        DragManager.instance.RegisterDropArea(GetComponent<RectTransform>());
    }

    private void OnDisable()
    {
        DragManager.instance.UnregisterDropArea(GetComponent<RectTransform>());
    }
}
