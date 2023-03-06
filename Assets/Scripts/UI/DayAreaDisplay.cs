using UnityEngine;
using TMPro;

public class DayAreaDisplay : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    public void Setup(string day)
    {
        tmp.SetText(day);
    }
}
