using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartsManager : MonoBehaviour
{
    [SerializeField]
    private List<RobotPart> allRobotParts;

    [SerializeField]
    private RobotConstructionController robotConstructionController;

    [SerializeField]
    private TabButton corePartsButton;

    [SerializeField]
    private TabButton wheelPartsButton;

    [SerializeField]
    private TabButton hingePartsButton;

    private bool isPartsOpen;

    public bool IsPartsOpen { get => isPartsOpen; set => isPartsOpen = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenParts()
    {
        IsPartsOpen = true;
        if (robotConstructionController.GetRobotParts() == null)
        {
            wheelPartsButton.SetIsDeactivated(true);
            hingePartsButton.SetIsDeactivated(true);
            corePartsButton.SetIsDeactivated(false);
        }
        else
        {
            corePartsButton.SetIsDeactivated(true);
            hingePartsButton.SetIsDeactivated(false);
            wheelPartsButton.SetIsDeactivated(false);
        }
    }

    public void CloseParts()
    {
        IsPartsOpen = false;
    }

    public RobotPart GetRobotPartFromRobotDataEntry(RobotDataEntry robotDataEntry)
    {
        RobotPart part = allRobotParts.First(item => item.robotPartIdentifier == robotDataEntry.robotPartIdentifier);
        return part;
    }
}
