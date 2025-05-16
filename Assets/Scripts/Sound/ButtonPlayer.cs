using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ButtonPlayer : MonoBehaviour
{
    public void ButtonPlay()
    {
        SoundManager.Instance.OnSFXPlay((int)SoundType.ButtonClick);
    }
}
