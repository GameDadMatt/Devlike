using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;

public class UIEvents : MonoBehaviour
{
    public static UIEvents instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public event Action<GameSpeed> onChangeSpeed;
    public void ChangeSpeedSlow()
    {
        onChangeSpeed?.Invoke(GameSpeed.Slow);
    }

    public void ChangeSpeedNormal()
    {
        onChangeSpeed?.Invoke(GameSpeed.Normal);
    }

    public void ChangeSpeedFast()
    {
        onChangeSpeed?.Invoke(GameSpeed.Fast);
    }
}
