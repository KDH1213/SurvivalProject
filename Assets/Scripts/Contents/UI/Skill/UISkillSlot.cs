using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour
{
    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TextMeshProUGUI skillNameText;

    [SerializeField]
    private TextMeshProUGUI skillDescriptionText;

    [SerializeField]
    private TextMeshProUGUI skillPointText;

    public UnityEvent<LifeSkillType> onLifeSkillUpEvent;
    private LifeSkillType lifeSkillType;
    public LifeSkillType LifeSkillType => lifeSkillType;

    [field: SerializeField]
    public Button SkillUpButton {  get; private set; }

    private static readonly string skillLevelFormat = "{0} / {1}";

    private void Awake()
    {
        SkillUpButton.onClick.AddListener(OnSkillLevelUp);

    }

    public void InitializedInfo(Sprite icon, LifeSkillType lifeSkillType, int nameID, int descriptionID)
    {
        skillIcon.sprite = icon;
        this.lifeSkillType = lifeSkillType;
        skillNameText.text = TypeName.LifeSkillTypeName[(int)lifeSkillType];// DataTableManager.StringTable.Get(nameID);
        skillDescriptionText.text = TypeName.LifeSkillTypeName[(int)lifeSkillType]; // DataTableManager.StringTable.Get(descriptionID);

        var lifeStat = GameObject.FindWithTag(Tags.Player).GetComponent<LifeStat>();
        OnChangeSkillLevel(lifeStat.SkillLevelTable[lifeSkillType], 100);
    }

    public void OnChangeSkillLevel(int currentLevel, int maxLevel)
    {
        skillPointText.text = string.Format(skillLevelFormat, currentLevel.ToString(), maxLevel.ToString());

        if(currentLevel == maxLevel)
        {
            SkillUpButton.interactable = false;
        }
    }

    public void OnSkillLevelUp()
    {
        onLifeSkillUpEvent?.Invoke(lifeSkillType);
    }
}
