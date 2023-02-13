using UnityEngine;
using TMPro;

public class DayAreaDisplay : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    public void Setup(string day)
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = day;
    }
}
