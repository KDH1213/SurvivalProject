using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour
{
    [SerializeField]
    private Image objectImage;
    [SerializeField]
    private TextMeshProUGUI objectName;
    [SerializeField]
    private TextMeshProUGUI objectSize;
    [SerializeField]
    private TextMeshProUGUI objectHp;
    [SerializeField]
    private TextMeshProUGUI objectDescription1;
    [SerializeField]
    private TextMeshProUGUI objectDescription2;

    public void SetUI(TurretStructure selectedObject)
    {
        objectDescription1.text = $"공격력 : {selectedObject.damage}";
        objectDescription2.text = $"공격속도 : {selectedObject.attackTerm}";
    }
}
