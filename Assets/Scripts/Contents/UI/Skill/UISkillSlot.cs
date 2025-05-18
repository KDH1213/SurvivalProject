using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TextMeshProUGUI skillNameText;

    [SerializeField]
    private TextMeshProUGUI skillDescriptionText;

    [SerializeField]
    private TextMeshProUGUI skillPointText;

    public UnityEvent<SkillType> onLifeSkillUpEvent;
    private SkillType lifeSkillType;
    public SkillType SkillType => lifeSkillType;

    [field: SerializeField]
    public Button SkillUpButton {  get; private set; }

    public UnityAction<SkillType> onClickAction;

    private static readonly string skillLevelFormat = "{0} / {1}";

    private void Awake()
    {
        SkillUpButton.onClick.AddListener(OnSkillLevelUp);

    }

    public void InitializedInfo(Sprite icon, SkillType lifeSkillType, int nameID, int descriptionID)
    {
        skillIcon.sprite = icon;
        this.lifeSkillType = lifeSkillType;
        skillNameText.text = TypeName.LifeSkillTypeName[(int)lifeSkillType];// DataTableManager.StringTable.Get(nameID);
        skillDescriptionText.text = TypeName.LifeSkillTypeName[(int)lifeSkillType]; // DataTableManager.StringTable.Get(descriptionID);

        var lifeStat = GameObject.FindWithTag(Tags.Player).GetComponent<LifeStat>();

        if(lifeStat.SkillLevelTable.TryGetValue(lifeSkillType, out var value))
        {
            OnChangeSkillLevel(value, 100);
        }
        else
        {
            OnChangeSkillLevel(0, 100);
        }
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
        onClickAction?.Invoke(lifeSkillType);
        onLifeSkillUpEvent?.Invoke(lifeSkillType);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onClickAction?.Invoke(lifeSkillType);
    }
}
