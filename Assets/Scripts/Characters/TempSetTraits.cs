using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;

public class TempSetTraits : MonoBehaviour
{
    public List<string> traits = new List<string>();

    // Start is called before the first frame update
    void OnEnable()
    {
        Profile p = GetComponent<Character>().profile;
        foreach(string type in traits)
        {
            p.traitNames.Add(type);
        }
    }
}
