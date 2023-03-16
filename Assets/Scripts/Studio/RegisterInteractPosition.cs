using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;

public class RegisterInteractPosition : ExecutableBehaviour
{
    public DoingType type;

    protected override void OnStart()
    {
        InteractableManager.instance.RegisterInteractable(this);
    }
}
