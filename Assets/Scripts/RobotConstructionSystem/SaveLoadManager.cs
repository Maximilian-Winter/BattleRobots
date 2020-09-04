using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField]
    private TabGroup mainMenuTabGroup;

    [SerializeField]
    private RobotConstructionController robotConstructionController;

    [SerializeField]
    private PartsManager partsManager;

    [SerializeField]
    private GameObject saveAndLoadTab;

    [SerializeField]
    private GameObject newButton;

    [SerializeField]
    private GameObject saveButton;

    [SerializeField]
    private GameObject loadButton;

    [SerializeField]
    private GameObject backButton;

    [SerializeField]
    private GameObject fileNameInputField;

    [SerializeField]
    private GameObject filesScrollView;

    [SerializeField]
    private GameObject saveFileButton;

    [SerializeField]
    private GameObject fileEntryContainer;

    private int rigidBodyCount = 0;

    [SerializeField]
    private GameObject fileEntryPrefab;

    private List<GameObject> tempFileEntrys;

    // Start is called before the first frame update
    void Start()
    {
        tempFileEntrys = new List<GameObject>();
        if (!Directory.Exists(Application.persistentDataPath + "/" + "Saves/Robots/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + "Saves/Robots/");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[] GetHardDriveFolderContent(string path, string fileType)
    {
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath + "/" + path, "*." + fileType);
        return filePaths;
    }

    public void GoBack()
    {
        newButton.SetActive(true);
        saveButton.SetActive(true);
        loadButton.SetActive(true);
        backButton.SetActive(false);
        fileNameInputField.SetActive(false);
        filesScrollView.SetActive(false);
        saveFileButton.SetActive(false);

        fileNameInputField.GetComponentInChildren<InputField>().text = "";

        foreach (GameObject fileEntry in tempFileEntrys)
        {
            fileEntry.GetComponent<FileEntryUI>().OnClickFileEntry -= LoadPlayerFile;
            Destroy(fileEntry);
        }
        tempFileEntrys.Clear();
    }

    public void OpenSaveDialog()
    {
        newButton.SetActive(false);
        saveButton.SetActive(false);
        loadButton.SetActive(false);
        backButton.SetActive(true);
        fileNameInputField.SetActive(true);
        filesScrollView.SetActive(false);
        saveFileButton.SetActive(true);
    }

    public void OpenLoadDialog()
    {
        newButton.SetActive(false);
        saveButton.SetActive(false);
        loadButton.SetActive(false);
        backButton.SetActive(true);
        fileNameInputField.SetActive(false);
        filesScrollView.SetActive(true);
        saveFileButton.SetActive(false);

        string[] filePaths = GetHardDriveFolderContent("Saves/Robots/", "robot");
        foreach (string file in filePaths)
        {
            GameObject fileEntryUI = Instantiate(fileEntryPrefab, fileEntryContainer.transform);
            fileEntryUI.GetComponent<FileEntryUI>().SetFileEntryPath(file);
            fileEntryUI.GetComponent<FileEntryUI>().SetFileEntryText(Path.GetFileNameWithoutExtension(file));
            fileEntryUI.GetComponent<FileEntryUI>().OnClickFileEntry += LoadPlayerFile; 
            tempFileEntrys.Add(fileEntryUI);
        }
    }

    public void SavePlayerFile()
    {
        string path = Application.persistentDataPath + "/Saves/Robots/" + fileNameInputField.GetComponentInChildren<InputField>().text + ".robot";
        SaveFile(path, robotConstructionController.GetRobotParts());
        mainMenuTabGroup.ResetTabGroup();
    }

    public void LoadPlayerFile(string path)
    {
        robotConstructionController.SetRobotParts(LoadFile(path, robotConstructionController.RobotBodyGameObject));
        robotConstructionController.RigidBodyCount = rigidBodyCount;
        mainMenuTabGroup.ResetTabGroup();
    }

    public void SaveFile(string path, List<RobotPartRuntimeObject> robotParts)
    {
        RobotData robotData = SaveRobot(robotParts);
        if (robotData != null)
        {
            robotData.SaveRobotData(path);
        }
    }

    public void SaveAsScriptableObject(RobotDataSO robotDataSO, List<RobotPartRuntimeObject> robotParts)
    {
        robotDataSO.SetRobotData(SaveRobot(robotParts));
    }

    public List<RobotPartRuntimeObject> LoadFile(string path, GameObject robotBodyGameObject)
    {
        List<RobotPartRuntimeObject> robotParts = new List<RobotPartRuntimeObject>();

        RobotData robotData = new RobotData();
        robotData.LoadRobotData(path);
        if (robotData != null)
        {
            robotParts = LoadRobot(robotData, robotBodyGameObject);
        }

        return robotParts;
    }

    public List<RobotPartRuntimeObject> LoadFromScriptableObject(RobotDataSO robotDataSO, GameObject robotBodyGameObject)
    {
        List<RobotPartRuntimeObject> robotParts = new List<RobotPartRuntimeObject>();

        RobotData robotData = robotDataSO.GetRobotData();
        if (robotData != null)
        {
            robotParts = LoadRobot(robotData, robotBodyGameObject);
        }

        return robotParts;
    }

    public RobotData SaveRobot(List<RobotPartRuntimeObject> robotParts)
    {
        RobotData CurrentRobot;

        CurrentRobot = new RobotData();

        if (robotParts != null)
        {
            CurrentRobot.robotDataEntries = new List<RobotDataEntry>();

            foreach (RobotPartRuntimeObject robotPartRuntimeObject in robotParts)
            {
                List<float> floatPartSettings = new List<float>();
                List<bool> boolPartSettings = new List<bool>();
                List<string> stringPartSettings = new List<string>();

                if (robotPartRuntimeObject.robotPart.robotPartType == RobotPartType.WheelPart)
                {
                    boolPartSettings.Add(robotPartRuntimeObject.robotPartGameObject.GetComponent<SimpleWheelController>().GetActivateMotor());
                    boolPartSettings.Add(robotPartRuntimeObject.robotPartGameObject.GetComponent<SimpleWheelController>().GetActivateSteering());
                    boolPartSettings.Add(robotPartRuntimeObject.robotPartGameObject.GetComponent<SimpleWheelController>().GetReverseSpinDirection());
                }
                CurrentRobot.robotDataEntries.Add(new RobotDataEntry(robotPartRuntimeObject.parentIndex, robotPartRuntimeObject.robotPart.robotPartIdentifier, robotPartRuntimeObject.robotPartGameObject.transform.localPosition, robotPartRuntimeObject.robotPartGameObject.transform.localRotation, new RobotDataEntrySettings(floatPartSettings, boolPartSettings, stringPartSettings)));
            }
        }
        else
        {
            CurrentRobot = null;
        }

        return CurrentRobot;
    }

    public List<RobotPartRuntimeObject> LoadRobot(RobotData CurrentRobot, GameObject robotBodyGameObject)
    {
        List<RobotPartRuntimeObject> RobotParts;
        RobotParts = new List<RobotPartRuntimeObject>();
        RobotPart SelectedRobotPart = new RobotPart();
        GameObject SelectedRobotPartGameObject = new GameObject();
        GameObject RobotRootObject = new GameObject();

        rigidBodyCount = 0;

        if (CurrentRobot.robotDataEntries != null)
        {
            bool firstEntry = true;
            foreach (RobotDataEntry robotDataEntry in CurrentRobot.robotDataEntries)
            {
                SelectedRobotPart = partsManager.GetRobotPartFromRobotDataEntry(robotDataEntry);
                SelectedRobotPartGameObject = Instantiate(SelectedRobotPart.robotPartPrefab);
                SetLayerRecursively(SelectedRobotPartGameObject, LayerMask.NameToLayer("Default"));
                SelectedRobotPartGameObject.transform.parent = robotBodyGameObject.transform;
                SelectedRobotPartGameObject.transform.localPosition = robotDataEntry.robotPartLocalPosition;
                SelectedRobotPartGameObject.transform.localRotation = robotDataEntry.robotPartLocalRotation;
                if (SelectedRobotPart.robotPartType == RobotPartType.WheelPart)
                {
                    SelectedRobotPartGameObject.GetComponent<FixedJoint>().connectedBody = GetRigidbodyByIndex(robotBodyGameObject, robotDataEntry.parentIndex);

                    SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetActivateMotor(robotDataEntry.robotPartSettings.boolSettings[0]);
                    SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetActivateSteering(robotDataEntry.robotPartSettings.boolSettings[1]);
                    SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetReverseSpinDirection(robotDataEntry.robotPartSettings.boolSettings[2]);
                }
                if (SelectedRobotPart.robotPartType == RobotPartType.HingePart)
                {
                    SelectedRobotPartGameObject.GetComponent<FixedJoint>().connectedBody = GetRigidbodyByIndex(robotBodyGameObject, robotDataEntry.parentIndex);
                }


                if (firstEntry)
                {
                    RobotRootObject = SelectedRobotPartGameObject;
                    firstEntry = false;
                }
                AddRobotPartRuntimeObject(RobotParts, robotDataEntry.parentIndex, SelectedRobotPart, SelectedRobotPartGameObject);
            }

        }
        return RobotParts;
    }

    public Rigidbody GetRigidbodyByIndex(GameObject robotBodyGameObject, int index)
    {
        RigidbodyIdentifier[] identifier = robotBodyGameObject.GetComponentsInChildren<RigidbodyIdentifier>();

        foreach(RigidbodyIdentifier id in identifier)
        {
            if(id.Identifier == index)
            {
                return id.GetComponent<Rigidbody>();
            }
        }

        return null;
    }

    private void AddRobotPartRuntimeObject(List<RobotPartRuntimeObject> robotParts, int parentIndex, RobotPart robotPart, GameObject robotPartGameObject)
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

    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

}
