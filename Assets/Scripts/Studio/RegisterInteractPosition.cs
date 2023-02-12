using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;

public class RegisterInteractPosition : MonoBehaviour
{
    public DoingType type;

    private void OnEnable()
    {
        InteractableManager.instance.RegisterInteractable(this);
    }
}
