using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStageInfoView : MonoBehaviour
{
    //[SerializeField]
    //private TextMeshProUGUI timeText;

    [SerializeField]
    private TextMeshProUGUI temperatureText;

    //public void OnChangeTime()
    //{
    //    timeText.text = 
    //}

    // TODO :: �����Ϳ� Stage�Ŵ����� ����
    public void OnChangeTemperature(int temperature)
    {
        temperatureText.text = temperature.ToString();
    }
}
