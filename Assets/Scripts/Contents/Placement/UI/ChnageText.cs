using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChnageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI before;
    [SerializeField]
    private TextMeshProUGUI after;

    public void SetText(string title, string before, string after)
    {
        this.title.text = title;
        this.before.text = before;
        this.after.text = after;
    }
}
