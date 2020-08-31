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


    [SerializeField]
    private GameObject fileEntryPrefab;

    private List<GameObject> tempFileEntrys;

    // Start is called before the first frame update
    void Start()
    {
        tempFileEntrys = new List<GameObject>();
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
            fileEntry.GetComponent<FileEntryUI>().OnClickFileEntry -= LoadFile;
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
            fileEntryUI.GetComponent<FileEntryUI>().OnClickFileEntry += LoadFile; 
            tempFileEntrys.Add(fileEntryUI);
        }
    }

    public void SaveFile()
    {
        string path = Application.persistentDataPath + "/Saves/Robots/" + fileNameInputField.GetComponentInChildren<InputField>().text + ".robot";
        RobotData robotData = robotConstructionController.SaveRobot();
        if(robotData != null)
        {
            robotData.SaveState(path);
        }
        mainMenuTabGroup.ResetTabGroup();
    }

    public void LoadFile(string path)
    {
        RobotData robotData = new RobotData();
        robotData.LoadState(path);
        if (robotData != null)
        {
            robotConstructionController.CurrentRobot = robotData;
            robotConstructionController.LoadRobot();
        }
        mainMenuTabGroup.ResetTabGroup();
    }

}
