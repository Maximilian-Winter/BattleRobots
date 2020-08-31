using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabToggle : MonoBehaviour
{
    [SerializeField]
    List<GameObject> allTabButtonsExceptThis;

    private bool tabIsOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleTab()
    {
        if(tabIsOpen)
        {
            gameObject.SetActive(false);
            tabIsOpen = false;
            foreach(GameObject tabButton in allTabButtonsExceptThis)
            {
                tabButton.SetActive(true);
            }
        }
        else
        {
            gameObject.SetActive(true);
            tabIsOpen = true;
            foreach (GameObject tabButton in allTabButtonsExceptThis)
            {
                tabButton.SetActive(false);
            }
        }
    }
}
