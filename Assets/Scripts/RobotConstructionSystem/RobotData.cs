using OdinSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class RobotData
{
    public RobotData()
    {
        this.robotDataEntries = null;
    }

    public RobotData(List<RobotDataEntry> robotDataEntries)
    {
        this.robotDataEntries = robotDataEntries;
    }



    [SerializeField]
    public List<RobotDataEntry> robotDataEntries;

    

    public void SaveRobotDataBinaryToFile(string filePath)
    {
        byte[] bytes = SerializationUtility.SerializeValue(this.robotDataEntries, DataFormat.Binary);
        File.WriteAllBytes(filePath, bytes);
    }

    public void LoadRobotDataBinaryFromFile(string filePath)
    {
        if (!File.Exists(filePath)) return; // No state to load

        byte[] bytes = File.ReadAllBytes(filePath);
        this.robotDataEntries = SerializationUtility.DeserializeValue<List<RobotDataEntry>>(bytes, DataFormat.Binary);
    }

    public void SaveRobotDataJsonToPlayerPrefs(string key)
    {
        byte[] bytes = SerializationUtility.SerializeValue(this.robotDataEntries, DataFormat.JSON);
        string jsonString = System.Text.Encoding.UTF8.GetString(bytes);
        PlayerPrefs.SetString(key, jsonString);
    }

    public void LoadRobotDataJsonFromPlayerPrefs(string key)
    {
        string jsonString = PlayerPrefs.GetString(key); 
        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
        this.robotDataEntries = SerializationUtility.DeserializeValue<List<RobotDataEntry>>(bytes, DataFormat.JSON);
    }

}

[Serializable]
public class RobotDataEntry
{
    public int parentIndex;
    public string robotPartIdentifier;
    public Vector3 robotPartLocalPosition;
    public Quaternion robotPartLocalRotation;
    public RobotDataEntrySettings robotPartSettings;

    public RobotDataEntry(int parentIndex, string robotPartIdentifier, Vector3 robotPartLocalPosition, Quaternion robotPartLocalRotation, RobotDataEntrySettings robotPartSetting)
    {
        this.parentIndex = parentIndex;
        this.robotPartIdentifier = robotPartIdentifier;
        this.robotPartLocalPosition = robotPartLocalPosition;
        this.robotPartLocalRotation = robotPartLocalRotation;
        this.robotPartSettings = robotPartSetting;
    }

    public RobotDataEntry()
    {
        this.parentIndex = 0;
        this.robotPartIdentifier = "";
        this.robotPartLocalPosition = Vector3.zero;
        this.robotPartLocalRotation = Quaternion.identity;
        this.robotPartSettings = null;
    }
}

[Serializable]
public class RobotDataEntrySettings
{
    public List<float> floatSettings;
    public List<bool> boolSettings;
    public List<string> stringSettings;

    public RobotDataEntrySettings(List<float> floatSettings, List<bool> boolSettings, List<string> stringSettings)
    {
        this.floatSettings = floatSettings;
        this.boolSettings = boolSettings;
        this.stringSettings = stringSettings;
    }
}
