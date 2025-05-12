using DG.Tweening;
using TMPro;
using UnityEngine;

public class ToastMsg : Singleton<ToastMsg>
{
    [SerializeField]
    private TextMeshProUGUI txt;
    private float fadeInOutTime = 0.5f;
    private static ToastMsg instance = null;
    Sequence sequence;

    public void ShowMessage(string msg)
    {
        Color originalColor = txt.color;
        txt.text = msg;
        
        if(sequence != null)
        {
            sequence.Kill(true);
        }

        sequence = DOTween.Sequence();
        sequence.SetAutoKill(true);
        sequence.OnStart(() => { txt.enabled = true;});
        sequence.Append(FadeInOut(txt, fadeInOutTime, true));
        sequence.Insert(1f, FadeInOut(txt, fadeInOutTime, false));
        sequence.OnComplete(() => { txt.enabled = false; sequence = null; });
        sequence.Play();

        //;
        txt.color = originalColor;
    }

    private Tween FadeInOut(TextMeshProUGUI target, float durationTime, bool inOut)
    {

        float end;
        if (inOut)
        {
            end = 1.0f;
        }
        else
        {
            end = 0f;
        }

        return target.DOFade(end, durationTime);
    }

}
