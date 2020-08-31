using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileEntryUI : MonoBehaviour
{
    public delegate void OnClickFileEntryDelegate(string filePath);
    public event OnClickFileEntryDelegate OnClickFileEntry;

    [SerializeField]
    private Button fileEntryButton;

    [SerializeField]
    private Text fileEntryText;

    [SerializeField]
    private string fileEntryPath;

    public Button FileEntryButton { get => fileEntryButton; set => fileEntryButton = value; }

    private void OnEnable()
    {
        fileEntryButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        fileEntryButton.onClick.RemoveListener(OnClick);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFileEntryText(string text)
    {
        fileEntryText.text = text;
    }

    public void SetFileEntryPath(string path)
    {
        fileEntryPath = path;
    }

    void OnClick()
    {
        OnClickFileEntry(this.fileEntryPath);
    }

}
