using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModuleSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private ModulePopup modulesPopup;
    [SerializeField] private PopupPanel popupPanel;

    public Image icon;
    public Image demountIcon;
    public Module module;

    public void Setup(Module newModule)
    {
        icon.enabled = true;
        demountIcon.enabled = true;

        module = newModule;

        if (module.icon != null)
            icon.sprite = module.icon;
    }

    public void Reset()
    {
        module = null;
        icon.sprite = null;
        icon.enabled = false;
        demountIcon.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (popupPanel.lastModuleButton == this)
            modulesPopup.gameObject.SetActive(!modulesPopup.gameObject.activeSelf);
        else
            modulesPopup.gameObject.SetActive(true);

        popupPanel.lastModuleButton = this;
    }

    public void Demount()
    {
        popupPanel.DemountUpgrade(module);
    }
}
