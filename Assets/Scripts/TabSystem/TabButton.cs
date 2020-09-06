using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup tapGroup;
    [SerializeField] private Image background;
    [SerializeField] private bool hideAllTabButtonsExceptThisWhenSelected;
    [SerializeField] private bool hideTillExplictActivated;
    [SerializeField] private bool isDeactivated;
    [SerializeField] private UnityEvent onTabSelected;
    [SerializeField] private UnityEvent onTabDeselected;

    public bool HideAllTabButtonsExceptThisWhenSelected { get => hideAllTabButtonsExceptThisWhenSelected; set => hideAllTabButtonsExceptThisWhenSelected = value; }
    public bool HideTillExplictActivated { get => hideTillExplictActivated; set => hideTillExplictActivated = value; }

    public bool GetIsDeactivated()
    {
        return isDeactivated;
    }

    public void SetIsDeactivated(bool value)
    {
        isDeactivated = value;
        tapGroup.OnTabSetIsDeactivated(this);
    }

    void OnEnable()
    {
        tapGroup.OnTabStart(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        tapGroup.Subscribe(this);
        tapGroup.OnTabStart(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!GetIsDeactivated())
        {
            tapGroup.OnTabSelected(this);
        } 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GetIsDeactivated())
        {
            tapGroup.OnTabEnter(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!GetIsDeactivated())
        {
            tapGroup.OnTabExit(this);
        }  
    }

    public void SetBackgroundSprite(Sprite sprite)
    {
        background.sprite = sprite;
    }

    public void Select()
    {
        if (!GetIsDeactivated())
        {
            if (onTabSelected != null)
            {
                onTabSelected.Invoke();
            }
        } 
    }

    public void Deselect()
    {
        if (!GetIsDeactivated())
        {
            if (onTabDeselected != null)
            {
                onTabDeselected.Invoke();
            }
        }
    }
}
