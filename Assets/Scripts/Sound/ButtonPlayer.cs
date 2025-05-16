using UnityEngine;
using UnityEngine.UI;

public  class ButtonPlayer : MonoBehaviour
{
    public void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => SoundManager.Instance.OnSFXPlay((int)SoundType.ButtonClick));
    }
}
