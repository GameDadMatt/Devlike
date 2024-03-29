using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;
using Devlike;

public class InteractPosition : MonoBehaviour
{
    public DoingType type;

    private void OnEnable()
    {
        InteractableManager.instance.OnGetInteractables += RegisterInteractable;
    }

    public void RegisterInteractable()
    {
        InteractableManager.instance.RegisterInteractable(this);
    }
}
