using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RobotPartRuntimeObject
{
    public int parentIndex;
    public RobotPart robotPart;
    public GameObject robotPartGameObject;

    public RobotPartRuntimeObject(int parentIndex, RobotPart robotPart, GameObject robotPartGameObject)
    {
        this.parentIndex = parentIndex;
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

    [SerializeField]
    private SaveLoadManager saveLoadManager;

    [SerializeField]
    private GameObject robotBodyGameObject;

    [SerializeField]
    private LayerMask buildingLayerMask;

    private GameObject robotRootObject;

    private RobotPart selectedRobotPart;
    private GameObject selectedRobotPartGameObject;

    [SerializeField]
    private List<RobotPartRuntimeObject> robotParts;

    private Vector3 placingPosition;
    private float placingDistance = 2.0f;

    private bool isInPlacingPartMode = false;

    private int rigidBodyCount = 0;

    public GameObject RobotRootObject { get => robotRootObject; set => robotRootObject = value; }

    public List<RobotPartRuntimeObject> GetRobotParts()
    {
        return robotParts;
    }

    public void SetRobotParts(List<RobotPartRuntimeObject> value)
    {
        if(value != null && value.Count > 0)
        {
            robotRootObject = value[0].robotPartGameObject;
        }
       
        robotParts = value;
    }

    public RobotPart SelectedRobotPart { get => selectedRobotPart; set => selectedRobotPart = value; }
    public GameObject SelectedRobotPartGameObject { get => selectedRobotPartGameObject; set => selectedRobotPartGameObject = value; }
    public GameObject RobotBodyGameObject { get => robotBodyGameObject; set => robotBodyGameObject = value; }
    public int RigidBodyCount { get => rigidBodyCount; set => rigidBodyCount = value; }

    // Start is called before the first frame update
    void Start()
    {
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

            if(GetRobotParts() != null)
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000.0f, buildingLayerMask);
                if (hit)
                {
                    if (hitInfo.transform.gameObject.tag == "RobotPart")
                    {
                        SelectedRobotPartGameObject.transform.position = new Vector3(hitInfo.point.x, SnapTo(hitInfo.point.y, 0.025f), hitInfo.point.z);
                        if (Input.GetMouseButtonDown(0) && !partsManager.IsPartsOpen && !partSettingsManager.IsPartSettingsOpen && !mainMenuTabGroup.MouseIsHoveringTabGroup && !partsMenuTabGroup.MouseIsHoveringTabGroup)
                        {
                            SelectedRobotPartGameObject.layer = LayerMask.NameToLayer("Default");
                            isInPlacingPartMode = false;
                            SelectedRobotPartGameObject.transform.parent = RobotBodyGameObject.transform;
                            SelectedRobotPartGameObject.GetComponent<FixedJoint>().connectedBody = hitInfo.transform.GetComponent<Rigidbody>();
                            SelectedRobotPartGameObject.GetComponent<Outline>().enabled = false;
                            AddRobotPartRuntimeObject(hitInfo.transform.gameObject.GetComponent<RigidbodyIdentifier>().Identifier, SelectedRobotPart, SelectedRobotPartGameObject);
                            SelectedRobotPartGameObject = null;
                            SelectedRobotPart = null;
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

                    SelectedRobotPartGameObject.layer = LayerMask.NameToLayer("Default");
                    isInPlacingPartMode = false;
                    SelectedRobotPartGameObject.transform.parent = RobotBodyGameObject.transform;
                    RobotRootObject = SelectedRobotPartGameObject;
                    SelectedRobotPartGameObject.GetComponent<Outline>().enabled = false;
                    AddRobotPartRuntimeObject(0, SelectedRobotPart, SelectedRobotPartGameObject);
                    SelectedRobotPartGameObject = null;
                    SelectedRobotPart = null;
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
                        SelectedRobotPartGameObject = hitInfo.transform.gameObject.GetComponent<RigidbodyIdentifier>().PartTransform.gameObject;
                        SelectedRobotPart = GetRobotPart(SelectedRobotPartGameObject);
                        SelectedRobotPartGameObject.GetComponent<Outline>().enabled = true;
                        partSettingsManager.ShowPartSettingsButton();
                        partSettingsManager.UpdatePartSettings();
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

    private void AddRobotPartRuntimeObject(int parentIndex, RobotPart robotPart, GameObject robotPartGameObject)
    {
        if (robotParts == null)
        {
            robotParts = new List<RobotPartRuntimeObject>();
        }

        robotParts.Add(new RobotPartRuntimeObject(parentIndex, robotPart, robotPartGameObject));

        if (robotPart.robotPartType == RobotPartType.CorePart)
        {
            robotPartGameObject.GetComponent<CorePart>().OnIsPlaced(ref rigidBodyCount);
        }

        if (robotPart.robotPartType == RobotPartType.WheelPart)
        {
            robotPartGameObject.GetComponent<WheelPart>().OnIsPlaced(ref rigidBodyCount);
        }

        if (robotPart.robotPartType == RobotPartType.HingePart)
        {
            robotPartGameObject.GetComponent<HingePart>().OnIsPlaced(ref rigidBodyCount);
        }
    }

    public void DeleteRobot()
    {
        if (GetRobotParts() != null)
        {
            foreach (RobotPartRuntimeObject robotPartRuntimeObject in GetRobotParts())
            {
                Destroy(robotPartRuntimeObject.robotPartGameObject);
            }
        }
        SetRobotParts(null);

        RigidBodyCount = 0;
    }

    void RecalculatePlacingPos()
    {
        placingPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, placingDistance));
    }

    private RobotPart GetRobotPart(GameObject robotPartGameObject)
    {
        if(GetRobotParts() != null)
        {
            foreach (RobotPartRuntimeObject robotPartRuntimeObject in GetRobotParts())
            {
                if (robotPartRuntimeObject.robotPartGameObject == robotPartGameObject)
                {
                    return robotPartRuntimeObject.robotPart;
                }
            }
        }

        return null;
    }
    
    public void PlaceNewPart(RobotPart robotPart)
    {
        if (SelectedRobotPartGameObject != null)
        {
            SelectedRobotPartGameObject.GetComponent<Outline>().enabled = false;
        }

        if (isInPlacingPartMode)
        {
            if (SelectedRobotPartGameObject != null)
            {
                Destroy(SelectedRobotPartGameObject);
            }  
        }

        partSettingsManager.HidePartSettingsButton();
        SelectedRobotPart = robotPart;
        SelectedRobotPartGameObject = Instantiate(robotPart.robotPartPrefab);
        SelectedRobotPartGameObject.GetComponent<Outline>().enabled = true;
        isInPlacingPartMode = true;
    }

    public void GoIntoTestMode()
    {
        if (GetRobotParts() != null)
        {
            if (robotRootObject != null)
            {
                robotBodyGameObject.tag = "Untagged";
                robotRootObject.tag = "PlayerTarget";
            }

            foreach (RobotPartRuntimeObject robotPartRuntimeObject in GetRobotParts())
            {
                if (robotPartRuntimeObject.robotPart.robotPartType == RobotPartType.CorePart)
                {
                    robotPartRuntimeObject.robotPartGameObject.GetComponent<CorePart>().OnEnterTestingMode();
                }
                else if (robotPartRuntimeObject.robotPart.robotPartType == RobotPartType.WheelPart)
                {
                    robotPartRuntimeObject.robotPartGameObject.GetComponent<WheelPart>().OnEnterTestingMode();
                }
                else if (robotPartRuntimeObject.robotPart.robotPartType == RobotPartType.HingePart)
                {
                    robotPartRuntimeObject.robotPartGameObject.GetComponent<HingePart>().OnEnterTestingMode();
                }
            }
        }
       
    }


    public static float SnapTo(float a, float snap)
    {
        return Mathf.Round(a / snap) * snap;
    }
}
