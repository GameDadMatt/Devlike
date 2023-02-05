using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;
using Characters;

public class TempSetTraits : MonoBehaviour
{
    public List<TraitType> traits = new List<TraitType>();

    // Start is called before the first frame update
    void OnEnable()
    {
        Profile p = GetComponent<Character>().profile;
        foreach(TraitType type in traits)
        {
            p.Traits.Add(type);
        }
    }
}
