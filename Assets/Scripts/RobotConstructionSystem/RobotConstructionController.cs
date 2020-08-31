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
    private List<RobotPart> allRobotParts;


    [SerializeField]
    private CameraFollow cameraFollow;



    [SerializeField]
    private TabGroup mainMenuTabGroup;

    [SerializeField]
    private TabGroup partsMenuTabGroup;



    [SerializeField]
    private GameObject partsTab;

    [SerializeField]
    private TabButton corePartsButton;

    [SerializeField]
    private TabButton wheelPartsButton;



    [SerializeField]
    private GameObject partSettingsTab;

    [SerializeField]
    private Text partSettingsPartName;

    [SerializeField]
    private Text partSettingsPartType;

    [SerializeField]
    private GameObject partSettingsWheelSettings;

    [SerializeField]
    private Toggle partSettingsWheelDirectionToggle;

    [SerializeField]
    private Toggle partSettingsWheelActivateMotorToggle;

    [SerializeField]
    private Toggle partSettingsWheelActivateSteeringToggle;

    [SerializeField]
    private GameObject partSettingsButton;

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

    private bool isPartsOpen = false;

    private bool isPartSettingsOpen = false;

    public RobotData CurrentRobot { get => currentRobot; set => currentRobot = value; }

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
                selectedRobotPartGameObject.transform.rotation *= Quaternion.Euler(Vector3.up * 90);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                selectedRobotPartGameObject.transform.rotation *= Quaternion.Euler(Vector3.up * -90);
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

            selectedRobotPartGameObject.transform.position = placingPosition;

            if(robotParts != null)
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    if (hitInfo.transform.gameObject.tag == "RobotPart")
                    {
                        selectedRobotPartGameObject.transform.position = new Vector3(hitInfo.point.x, SnapTo(hitInfo.point.y, 0.025f), hitInfo.point.z);
                        if (Input.GetMouseButtonDown(0) && !isPartsOpen && !isPartSettingsOpen && !mainMenuTabGroup.MouseIsHoveringTabGroup && !partsMenuTabGroup.MouseIsHoveringTabGroup)
                        {
                            isInPlacingPartMode = false;
                            selectedRobotPartGameObject.transform.parent = hitInfo.transform.parent;
                            selectedRobotPartGameObject.GetComponent<FixedJoint>().connectedBody = hitInfo.transform.GetComponent<Rigidbody>();
                            AddRobotPartRuntimeObject(selectedRobotPart, selectedRobotPartGameObject);

                            if (selectedRobotPart.robotPartType == RobotPartType.CorePart)
                            {
                                selectedRobotPartGameObject.GetComponent<CorePart>().OnIsPlaced();
                            }

                            if (selectedRobotPart.robotPartType == RobotPartType.WheelPart)
                            {
                                selectedRobotPartGameObject.GetComponent<WheelPart>().OnIsPlaced();
                            }
                        }
                    }
                    else
                    {
                        selectedRobotPartGameObject.transform.position = placingPosition;
                    }
                }
                else
                {
                    selectedRobotPartGameObject.transform.position = placingPosition;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && !isPartsOpen && !isPartSettingsOpen && !mainMenuTabGroup.MouseIsHoveringTabGroup && !partsMenuTabGroup.MouseIsHoveringTabGroup)
                {
                    isInPlacingPartMode = false;
                    if (selectedRobotPart.robotPartType == RobotPartType.CorePart)
                    {
                        selectedRobotPartGameObject.GetComponent<CorePart>().OnIsPlaced();
                    }

                    if (selectedRobotPart.robotPartType == RobotPartType.WheelPart)
                    {
                        selectedRobotPartGameObject.GetComponent<WheelPart>().OnIsPlaced();
                    }
                    cameraFollow.Target = selectedRobotPartGameObject.transform;
                    selectedRobotPartGameObject.transform.parent = robotBodyGameObject.transform;
                    AddRobotPartRuntimeObject(selectedRobotPart, selectedRobotPartGameObject);
                }
            }
           
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !isPartsOpen && !mainMenuTabGroup.MouseIsHoveringTabGroup && !partsMenuTabGroup.MouseIsHoveringTabGroup)
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    if (hitInfo.transform.gameObject.tag == "RobotPart")
                    {
                        selectedRobotPartGameObject = hitInfo.transform.gameObject;
                        selectedRobotPart = GetRobotPart(selectedRobotPartGameObject);
                        partSettingsButton.SetActive(true);
                        OpenPartSettings();
                    }
                    else
                    {
                        selectedRobotPartGameObject = null;
                        selectedRobotPart = null;
                        partSettingsButton.SetActive(false);
                        mainMenuTabGroup.ResetTabGroup();
                    }
                }
                else
                {
                    selectedRobotPartGameObject = null;
                    selectedRobotPart = null;
                    partSettingsButton.SetActive(false);
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
        if(robotParts == null)
        {
            robotParts = new List<RobotPartRuntimeObject>();
        }

        robotParts.Add(new RobotPartRuntimeObject(robotPart, robotPartGameObject));
    }

    private RobotPart GetRobotPart(GameObject robotPartGameObject)
    {
        foreach (RobotPartRuntimeObject robotPartRuntimeObject in robotParts)
        {
            if (robotPartRuntimeObject.robotPartGameObject == robotPartGameObject)
            {
                return robotPartRuntimeObject.robotPart;
            }
        }

        return null;
    }

    private RobotPart GetRobotPartFromRobotDataEntry(RobotDataEntry robotDataEntry)
    {
        RobotPart part = allRobotParts.First(item => item.robotPartIdentifier == robotDataEntry.robotPartIdentifier);
        return part;
    }

    public RobotData SaveCurrentRobot()
    {
        if (robotParts != null)
        {
            CurrentRobot.robotDataEntries = new List<RobotDataEntry>();
            foreach (RobotPartRuntimeObject robotPartRuntimeObject in robotParts)
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

        return CurrentRobot;
    }

    public void LoadRobot()
    {
        if (CurrentRobot.robotDataEntries != null)
        {
            if(robotParts != null)
            {
                foreach (RobotPartRuntimeObject robotPartRuntimeObject in robotParts)
                {
                    Destroy(robotPartRuntimeObject.robotPartGameObject);
                }
            }
            robotParts = new List<RobotPartRuntimeObject>();
            bool firstEntry = true;
            foreach (RobotDataEntry robotDataEntry in CurrentRobot.robotDataEntries)
            {
                selectedRobotPart = GetRobotPartFromRobotDataEntry(robotDataEntry);
                selectedRobotPartGameObject = Instantiate(selectedRobotPart.robotPartPrefab);
                selectedRobotPartGameObject.transform.parent = robotBodyGameObject.transform;
                selectedRobotPartGameObject.transform.localPosition = robotDataEntry.robotPartLocalPosition;
                selectedRobotPartGameObject.transform.localRotation = robotDataEntry.robotPartLocalRotation;
                if (selectedRobotPart.robotPartType == RobotPartType.WheelPart)
                {
                    selectedRobotPartGameObject.GetComponent<FixedJoint>().connectedBody = robotRootObject.GetComponent<Rigidbody>();

                    selectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetActivateMotor(robotDataEntry.robotPartSettings.boolSettings[0]);
                    selectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetActivateSteering(robotDataEntry.robotPartSettings.boolSettings[1]);
                    selectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetReverseSpinDirection(robotDataEntry.robotPartSettings.boolSettings[2]);
                }


                if (firstEntry)
                {
                    robotRootObject = selectedRobotPartGameObject;
                    firstEntry = false;
                }
                AddRobotPartRuntimeObject(selectedRobotPart, selectedRobotPartGameObject);
            }

        }
    }

    public void PlaceNewPart(RobotPart robotPart)
    {
        partSettingsButton.SetActive(false);
        selectedRobotPart = robotPart;
        selectedRobotPartGameObject = Instantiate(robotPart.robotPartPrefab);
        isInPlacingPartMode = true;
    }

    public void GoIntoTestMode()
    {
        foreach(RobotPartRuntimeObject robotPartRuntimeObject in robotParts)
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

    public void OpenParts()
    {
        isPartsOpen = true;
        if (robotParts == null)
        {
            wheelPartsButton.SetIsDeactivated(true);
        }
        else
        {
            corePartsButton.SetIsDeactivated(true);
            wheelPartsButton.SetIsDeactivated(false);
        }
    }

    public void CloseParts()
    {
        isPartsOpen = false;
    }

    public void OpenPartSettings()
    {
        isPartSettingsOpen = true;
        if (selectedRobotPart != null && selectedRobotPartGameObject != null)
        {
            partSettingsPartName.text = "Part Name: " + selectedRobotPart.robotPartName;

            if (selectedRobotPart.robotPartType == RobotPartType.CorePart)
            {
                partSettingsPartType.text = "Part Type: Core Part";
                partSettingsWheelSettings.SetActive(false);
            }
            else if (selectedRobotPart.robotPartType == RobotPartType.WheelPart)
            {
                partSettingsPartType.text = "Part Type: Wheel";
                partSettingsWheelSettings.SetActive(true);
                partSettingsWheelActivateMotorToggle.isOn = selectedRobotPartGameObject.GetComponent<SimpleWheelController>().GetActivateMotor();
                partSettingsWheelActivateSteeringToggle.isOn = selectedRobotPartGameObject.GetComponent<SimpleWheelController>().GetActivateSteering();
                partSettingsWheelDirectionToggle.isOn = selectedRobotPartGameObject.GetComponent<SimpleWheelController>().GetReverseSpinDirection();
            }
        }
    }

    public void ClosePartSettings()
    {
        isPartSettingsOpen = false;
    }

    public void PartSettingActivateMotorHasChanged(bool value)
    {
        if (selectedRobotPart != null && selectedRobotPartGameObject != null)
        {
            if (selectedRobotPart.robotPartType == RobotPartType.WheelPart)
            {
                selectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetActivateMotor(value);
            }
        }
    }

    public void PartSettingActivateSteeringHasChanged(bool value)
    {
        if (selectedRobotPart != null && selectedRobotPartGameObject != null)
        {
            if (selectedRobotPart.robotPartType == RobotPartType.WheelPart)
            {
                selectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetActivateSteering(value);
            }
        }
    }

    public void PartSettingReverseWheelDirectionHasChanged(bool value)
    {
        if (selectedRobotPart != null && selectedRobotPartGameObject != null)
        {
            if (selectedRobotPart.robotPartType == RobotPartType.WheelPart)
            {
                selectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetReverseSpinDirection(value);
            }
        }
    }

    public static float SnapTo(float a, float snap)
    {
        return Mathf.Round(a / snap) * snap;
    }
}
