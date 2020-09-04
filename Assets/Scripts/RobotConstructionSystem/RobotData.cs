using OdinSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class RobotData
{
    [SerializeField]
    public List<RobotDataEntry> robotDataEntries;

    public void SaveRobotData(string filePath)
    {
        byte[] bytes = SerializationUtility.SerializeValue(this.robotDataEntries, DataFormat.Binary);
        File.WriteAllBytes(filePath, bytes);
    }

    public void LoadRobotData(string filePath)
    {
        if (!File.Exists(filePath)) return; // No state to load

        byte[] bytes = File.ReadAllBytes(filePath);
        this.robotDataEntries = SerializationUtility.DeserializeValue<List<RobotDataEntry>>(bytes, DataFormat.Binary);
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
