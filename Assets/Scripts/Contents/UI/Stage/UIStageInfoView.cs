using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStageInfoView : MonoBehaviour
{
    //[SerializeField]
    //private TextMeshProUGUI timeText;

    [SerializeField]
    private TextMeshProUGUI temperatureText;

    [SerializeField]
    private Image temperatureImage;

    [SerializeField]
    private Sprite sunIcon;
    [SerializeField]
    private Sprite snowIcon;

    [SerializeField]
    private Sprite defalutIcon;


    //public void OnChangeTime()
    //{
    //    timeText.text = 
    //}

    // TODO :: 에디터에 Stage매니저와 연동
    public void OnChangeTemperature(int temperature)
    {
        if(temperature == 0)
        {
            temperatureImage.sprite = defalutIcon;
        }
        else if(temperature > 0)
        {
            temperatureImage.sprite = sunIcon;
            temperature *= -1;
        }
        else
        {
            temperatureImage.sprite = snowIcon;
        }

        temperatureText.text = temperature.ToString();
    }
}
