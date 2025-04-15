using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image slotImage;
    [SerializeField]
    private Image disableImage;

    public void SetSlot()
    {
        disableImage.gameObject.SetActive(false);
        disableImage.color = new Color(1, 1, 1, 1);
        slotImage.sprite = null;
        slotImage.color = new Color(1, 1, 1, 1);
    }

    private void SetButtonDisable()
    {
        GetComponent<Button>().enabled = true;
        disableImage.gameObject.SetActive(true);
        slotImage.gameObject.SetActive(false);
    }
}
