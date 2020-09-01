using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RobotPartRuntimeObject
{
    public RobotPart robotPart;
    public GameObject robotPartGameObject;

    public RobotPartRuntimeObject(RobotPart robotPart, GameObject robotPartGameObject)
    {
        this.robotPart = robotPart;
        this.robotPartGameObject = robotPartGameObject;
    }
}
public class RobotConstructionController : MonoBehaviour
{
    


    [SerializeField]
    private TabGroup mainMenuTabGroup;

    [SerializeField]
    private TabGroup partsMenuTabGroup;

    [SerializeField]
    private PartsManager partsManager;

    [SerializeField]
    private PartSettingsManager partSettingsManager;

    [SerializeField]
    private TransformGizmoManager transformGizmoManager;


    private RobotData currentRobot;

    [SerializeField]
    private GameObject robotBodyGameObject;

    private GameObject robotRootObject;

    private RobotPart selectedRobotPart;
    private GameObject selectedRobotPartGameObject;

    private List<RobotPartRuntimeObject> robotParts;

    private Vector3 placingPosition;
    private float placingDistance = 2.0f;

    private bool isInPlacingPartMode = false;

    public RobotData CurrentRobot { get => currentRobot; set => currentRobot = value; }
    public GameObject RobotRootObject { get => robotRootObject; set => robotRootObject = value; }
    public List<RobotPartRuntimeObject> RobotParts { get => robotParts; set => robotParts = value; }
    public RobotPart SelectedRobotPart { get => selectedRobotPart; set => selectedRobotPart = value; }
    public GameObject SelectedRobotPartGameObject { get => selectedRobotPartGameObject; set => selectedRobotPartGameObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentRobot = new RobotData();
        //AddRobotPartRuntimeObject(robotCorePart, robotCorePartGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(isInPlacingPartMode)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                SelectedRobotPartGameObject.transform.rotation *= Quaternion.Euler(Vector3.up * 90);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                SelectedRobotPartGameObject.transform.rotation *= Quaternion.Euler(Vector3.up * -90);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                placingDistance += 0.1f;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                placingDistance -= 0.1f;
            }

            RecalculatePlacingPos();

            SelectedRobotPartGameObject.transform.position = placingPosition;

            if(RobotParts != null)
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    if (hitInfo.transform.gameObject.tag == "RobotPart")
                    {
                        SelectedRobotPartGameObject.transform.position = new Vector3(hitInfo.point.x, SnapTo(hitInfo.point.y, 0.025f), hitInfo.point.z);
                        if (Input.GetMouseButtonDown(0) && !partsManager.IsPartsOpen && !partSettingsManager.IsPartSettingsOpen && !mainMenuTabGroup.MouseIsHoveringTabGroup && !partsMenuTabGroup.MouseIsHoveringTabGroup)
                        {
                            isInPlacingPartMode = false;
                            SelectedRobotPartGameObject.transform.parent = hitInfo.transform.parent;
                            SelectedRobotPartGameObject.GetComponent<FixedJoint>().connectedBody = hitInfo.transform.GetComponent<Rigidbody>();
                            //SelectedRobotPartGameObject.GetComponent<Outline>().enabled = false;
                            AddRobotPartRuntimeObject(SelectedRobotPart, SelectedRobotPartGameObject);
                        }
                    }
                    else
                    {
                        SelectedRobotPartGameObject.transform.position = placingPosition;
                    }
                }
                else
                {
                    SelectedRobotPartGameObject.transform.position = placingPosition;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && !partsManager.IsPartsOpen && !partSettingsManager.IsPartSettingsOpen && !mainMenuTabGroup.MouseIsHoveringTabGroup && !partsMenuTabGroup.MouseIsHoveringTabGroup)
                {
                    isInPlacingPartMode = false;
                    SelectedRobotPartGameObject.transform.parent = robotBodyGameObject.transform;
                    RobotRootObject = SelectedRobotPartGameObject;
                    SelectedRobotPartGameObject.GetComponent<Outline>().enabled = false;
                    AddRobotPartRuntimeObject(SelectedRobotPart, SelectedRobotPartGameObject);
                }
            }
           
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !partsManager.IsPartsOpen && !transformGizmoManager.IsHoveringGizmo && !mainMenuTabGroup.MouseIsHoveringTabGroup && !partsMenuTabGroup.MouseIsHoveringTabGroup)
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    if (hitInfo.transform.gameObject.tag == "RobotPart")
                    {
                        if(selectedRobotPartGameObject != null)
                        {
                            SelectedRobotPartGameObject.GetComponent<Outline>().enabled = false;
                        }
                        SelectedRobotPartGameObject = hitInfo.transform.gameObject;
                        SelectedRobotPart = GetRobotPart(SelectedRobotPartGameObject);
                        SelectedRobotPartGameObject.GetComponent<Outline>().enabled = true;
                        partSettingsManager.ShowPartSettingsButton();
                        partSettingsManager.OpenPartSettings();
                    }
                    else
                    {
                        if(SelectedRobotPartGameObject != null)
                        {
                            SelectedRobotPartGameObject.GetComponent<Outline>().enabled = false;
                        }
                       
                        SelectedRobotPartGameObject = null;
                        SelectedRobotPart = null;
                        partSettingsManager.HidePartSettingsButton();
                        mainMenuTabGroup.ResetTabGroup();
                    }
                }
                else
                {
                    if (SelectedRobotPartGameObject != null)
                    {
                        SelectedRobotPartGameObject.GetComponent<Outline>().enabled = false;
                    }
                    SelectedRobotPartGameObject = null;
                    SelectedRobotPart = null;
                    partSettingsManager.HidePartSettingsButton();
                    mainMenuTabGroup.ResetTabGroup();
                }
            }
        }
        
    }

    void RecalculatePlacingPos()
    {
        placingPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, placingDistance));
    }

    private void AddRobotPartRuntimeObject(RobotPart robotPart, GameObject robotPartGameObject)
    {
        if(RobotParts == null)
        {
            RobotParts = new List<RobotPartRuntimeObject>();
        }

        RobotParts.Add(new RobotPartRuntimeObject(robotPart, robotPartGameObject));

        if (robotPart.robotPartType == RobotPartType.CorePart)
        {
            robotPartGameObject.GetComponent<CorePart>().OnIsPlaced();
        }

        if (robotPart.robotPartType == RobotPartType.WheelPart)
        {
            robotPartGameObject.GetComponent<WheelPart>().OnIsPlaced();
        }
    }

    private RobotPart GetRobotPart(GameObject robotPartGameObject)
    {
        if(RobotParts != null)
        {
            foreach (RobotPartRuntimeObject robotPartRuntimeObject in RobotParts)
            {
                if (robotPartRuntimeObject.robotPartGameObject == robotPartGameObject)
                {
                    return robotPartRuntimeObject.robotPart;
                }
            }
        }

        return null;
    }

    public void DeleteRobot()
    {
        if (RobotParts != null)
        {
            foreach (RobotPartRuntimeObject robotPartRuntimeObject in RobotParts)
            {
                Destroy(robotPartRuntimeObject.robotPartGameObject);
            }
        }
        RobotParts = null;
    }

    public RobotData SaveRobot()
    {
        if (RobotParts != null)
        {
            CurrentRobot.robotDataEntries = new List<RobotDataEntry>();
            foreach (RobotPartRuntimeObject robotPartRuntimeObject in RobotParts)
            {
                List<bool> boolPartSettings = new List<bool>();

                if (robotPartRuntimeObject.robotPart.robotPartType == RobotPartType.WheelPart)
                {
                    boolPartSettings.Add(robotPartRuntimeObject.robotPartGameObject.GetComponent<SimpleWheelController>().GetActivateMotor());
                    boolPartSettings.Add(robotPartRuntimeObject.robotPartGameObject.GetComponent<SimpleWheelController>().GetActivateSteering());
                    boolPartSettings.Add(robotPartRuntimeObject.robotPartGameObject.GetComponent<SimpleWheelController>().GetReverseSpinDirection());
                }
                CurrentRobot.robotDataEntries.Add(new RobotDataEntry(robotPartRuntimeObject.robotPart.robotPartIdentifier, robotPartRuntimeObject.robotPartGameObject.transform.localPosition, robotPartRuntimeObject.robotPartGameObject.transform.localRotation, new RobotDataEntrySettings(boolPartSettings)));
            }
        }
        else
        {
            currentRobot = null;
        }

        return CurrentRobot;
    }


    public void LoadRobot()
    {
        if (CurrentRobot.robotDataEntries != null)
        {
            DeleteRobot();
            RobotParts = new List<RobotPartRuntimeObject>();
            bool firstEntry = true;
            foreach (RobotDataEntry robotDataEntry in CurrentRobot.robotDataEntries)
            {
                SelectedRobotPart = partsManager.GetRobotPartFromRobotDataEntry(robotDataEntry);
                SelectedRobotPartGameObject = Instantiate(SelectedRobotPart.robotPartPrefab);
                SelectedRobotPartGameObject.transform.parent = robotBodyGameObject.transform;
                SelectedRobotPartGameObject.transform.localPosition = robotDataEntry.robotPartLocalPosition;
                SelectedRobotPartGameObject.transform.localRotation = robotDataEntry.robotPartLocalRotation;
                if (SelectedRobotPart.robotPartType == RobotPartType.WheelPart)
                {
                    SelectedRobotPartGameObject.GetComponent<FixedJoint>().connectedBody = RobotRootObject.GetComponent<Rigidbody>();

                    SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetActivateMotor(robotDataEntry.robotPartSettings.boolSettings[0]);
                    SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetActivateSteering(robotDataEntry.robotPartSettings.boolSettings[1]);
                    SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetReverseSpinDirection(robotDataEntry.robotPartSettings.boolSettings[2]);
                }


                if (firstEntry)
                {
                    RobotRootObject = SelectedRobotPartGameObject;
                    firstEntry = false;
                }
                AddRobotPartRuntimeObject(SelectedRobotPart, SelectedRobotPartGameObject);
            }

        }
    }

    public void PlaceNewPart(RobotPart robotPart)
    {
        partSettingsManager.HidePartSettingsButton();
        SelectedRobotPart = robotPart;
        SelectedRobotPartGameObject = Instantiate(robotPart.robotPartPrefab);
        SelectedRobotPartGameObject.GetComponent<Outline>().enabled = true;
        isInPlacingPartMode = true;
    }

    public void GoIntoTestMode()
    {
        foreach(RobotPartRuntimeObject robotPartRuntimeObject in RobotParts)
        {
            if(robotPartRuntimeObject.robotPart.robotPartType == RobotPartType.CorePart)
            {
                robotPartRuntimeObject.robotPartGameObject.GetComponent<CorePart>().OnEnterTestingMode();
            }
            else if (robotPartRuntimeObject.robotPart.robotPartType == RobotPartType.WheelPart)
            {
                robotPartRuntimeObject.robotPartGameObject.GetComponent<WheelPart>().OnEnterTestingMode();
            }
        }
    }


    public static float SnapTo(float a, float snap)
    {
        return Mathf.Round(a / snap) * snap;
    }
}
