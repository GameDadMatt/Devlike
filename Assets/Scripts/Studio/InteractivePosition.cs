using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;

public class InteractivePosition : MonoBehaviour
{
    public TaskType restores;

    private void OnEnable()
    {
        InteractableManager.instance.RegisterInteractable(this);
    }

    private void OnDisable()
    {
        InteractableManager.instance.UnregisterInteractable(this);
    }
}
