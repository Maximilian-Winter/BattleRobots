using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabGroup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<GameObject> tabObjectsToSwap;
    [SerializeField] private Sprite tabIdle;
    [SerializeField] private Sprite tabHover;
    [SerializeField] private Sprite tabActive;
    [SerializeField] private Sprite tabDeactivated;

    private List<TabButton> tabButtons;
    private TabButton selectedButton;
    private bool mouseIsHoveringOneButton;

    public bool MouseIsHoveringTabGroup { get => mouseIsHoveringOneButton; set => mouseIsHoveringOneButton = value; }


    public void OnEnable()
    {
        ResetTabGroup();
    }

    public void OnDisable()
    {
        ResetTabGroup();
    }

    public void Subscribe(TabButton tabButton)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(tabButton);
    }
    public void OnTabStart(TabButton tabButton)
    {
        if (!tabButton.GetIsDeactivated())
        {
            tabButton.SetBackgroundSprite(tabIdle);
        }
        else
        {
            tabButton.SetBackgroundSprite(tabDeactivated);
        }
    }

    public void OnTabSetIsDeactivated(TabButton tabButton)
    {
        if(tabButton.GetIsDeactivated())
        {
            tabButton.SetBackgroundSprite(tabDeactivated);
            tabButton.gameObject.SetActive(false);
        }
        else
        {
            tabButton.SetBackgroundSprite(tabIdle);
            tabButton.gameObject.SetActive(true);
        }
    }

    public void OnTabEnter(TabButton tabButton)
    {
        ResetTabs();
        if(selectedButton == null || tabButton != selectedButton)
        {
            if(!tabButton.GetIsDeactivated())
            {
                tabButton.SetBackgroundSprite(tabHover);
            }
            else
            {
                tabButton.SetBackgroundSprite(tabDeactivated);
            }
            
        }  
    }

    public void OnTabExit(TabButton tabButton)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton tabButton)
    {
        if(selectedButton != tabButton || selectedButton == null)
        {
            if (selectedButton != null)
            {
                selectedButton.Deselect();
            }
            selectedButton = tabButton;
            selectedButton.Select();
            ResetTabs();
            tabButton.SetBackgroundSprite(tabActive);
            if(tabButton.HideAllTabButtonsExceptThisWhenSelected)
            {
                HideAllTabsExceptThis(tabButton);
            }
            
            int index = tabButton.transform.GetSiblingIndex();
            for (int i = 0; i < tabObjectsToSwap.Count; i++)
            {
                if (i == index)
                {
                    tabObjectsToSwap[i].SetActive(true);
                }
                else
                {
                    tabObjectsToSwap[i].SetActive(false);
                }
            }
        }
        else
        {
            selectedButton.Deselect();
            selectedButton = null;
            ShowAllTabs();
            ResetTabs();
            for (int i = 0; i < tabObjectsToSwap.Count; i++)
            {
                tabObjectsToSwap[i].SetActive(false);
            }
        }
       
    }

    public void ShowAllTabs()
    {
        if(tabButtons != null)
        {
            foreach (TabButton tabButton in tabButtons)
            {
                if (!tabButton.HideTillExplictActivated && !tabButton.GetIsDeactivated())
                {
                    tabButton.gameObject.SetActive(true);
                }
            }
        }
    }

    public void HideAllTabsExceptThis(TabButton button)
    {
        if (tabButtons != null)
        {
            foreach (TabButton tabButton in tabButtons)
            {
                if (tabButton != null && tabButton != button)
                {
                    tabButton.gameObject.SetActive(false);
                }
            }
        }
    }
    public void ResetTabGroup()
    {
        MouseIsHoveringTabGroup = false;
        if (selectedButton != null)
        {
            selectedButton.Deselect();
        }
        selectedButton = null;
        ShowAllTabs();
        ResetTabs();
        for (int i = 0; i < tabObjectsToSwap.Count; i++)
        {
            tabObjectsToSwap[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        if (tabButtons != null)
        {
            foreach (TabButton tabButton in tabButtons)
            {
                if (selectedButton != null && tabButton == selectedButton)
                {
                    continue;
                }
                if (!tabButton.GetIsDeactivated())
                {
                    tabButton.SetBackgroundSprite(tabIdle);
                }
                else
                {
                    tabButton.SetBackgroundSprite(tabDeactivated);
                }

            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseIsHoveringTabGroup = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseIsHoveringTabGroup = false;
    }
}
