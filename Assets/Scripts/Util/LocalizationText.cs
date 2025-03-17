//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//// Editor에서 업데이트를 돌릴 수 있는 어트리뷰트
//[ExecuteInEditMode]
//[RequireComponent(typeof(TextMeshProUGUI))]
//public class LocalizationText : MonoBehaviour
//{
//    public string stringId;

//#if UNITY_EDITOR
//    public Languages editorLanguage;
//#endif

//    private TextMeshProUGUI text;

//    private void Awake()
//    {
//        text = GetComponent<TextMeshProUGUI>();
//    }

//    private void OnEnable()
//    {
//        if(Application.isPlaying)
//            OnChangeLanguage(Varibalbes.currentLanguage);
//        else
//        {
//#if UNITY_EDITOR
//            OnChangeLanguage(editorLanguage);
//#endif
//        }
//    }

//    public void OnChangeLanguage(Languages languages)
//    {
//        var stringTableId = DataTableIds.String[(int)languages];
//        var stringTable = DataTableManager.Get<StringTable>(stringTableId);

//        text.text = stringTable.Get(stringId);
//    }
//}
