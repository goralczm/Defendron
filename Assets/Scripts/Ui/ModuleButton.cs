using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModuleButton : MonoBehaviour, IPointerDownHandler
{
    public PopupPanel popupPanel;
    public GameObject modulePopup;
    private Image icon;
    private Image xIcon;
    private Module module;

    private bool disabled = false;

    public void DisableButton()
    {
        disabled = true;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, .5f);
        xIcon.enabled = true;
    }

    public void EnableButton()
    {
        disabled = false;
        icon.color = Color.white;
        xIcon.enabled = false;
    }

    public void Setup(Module newModule)
    {
        if (icon == null)
            icon = GetComponent<Image>();

        if (xIcon == null)
            xIcon = transform.GetChild(0).GetComponent<Image>();

        module = newModule;

        if (module.icon != null)
            icon.sprite = module.icon;
        else
            icon.sprite = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (disabled)
            return;

        modulePopup.SetActive(false);
        popupPanel.MountUpgrade(module);
    }
}
