using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField]
    private GameObject saveAndLoadTab;

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
    private GameObject loadFileButton;

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
        saveButton.SetActive(true);
        loadButton.SetActive(true);
        backButton.SetActive(false);
        fileNameInputField.SetActive(false);
        filesScrollView.SetActive(false);
        saveFileButton.SetActive(false);
        loadFileButton.SetActive(false);
        
        foreach (GameObject fileEntry in tempFileEntrys)
        {
            fileEntry.GetComponent<FileEntryUI>().OnClickFileEntry -= LoadFile;
            Destroy(fileEntry);
        }
        tempFileEntrys.Clear();
    }

    public void OpenSaveDialog()
    {
        saveButton.SetActive(false);
        loadButton.SetActive(false);
        backButton.SetActive(true);
        fileNameInputField.SetActive(true);
        filesScrollView.SetActive(false);
        saveFileButton.SetActive(true);
        loadFileButton.SetActive(false);
    }

    public void OpenLoadDialog()
    {
        saveButton.SetActive(false);
        loadButton.SetActive(false);
        backButton.SetActive(true);
        fileNameInputField.SetActive(false);
        filesScrollView.SetActive(true);
        saveFileButton.SetActive(false);
        loadFileButton.SetActive(true);

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

    public void LoadFile(string path)
    {
        Debug.Log(path);
    }

}
