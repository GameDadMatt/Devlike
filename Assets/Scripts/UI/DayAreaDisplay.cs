using UnityEngine;
using TMPro;

public class DayAreaDisplay : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    public void Setup(string day)
    {
        day.ToUpper();
        tmp.SetText(day);
    }
}
