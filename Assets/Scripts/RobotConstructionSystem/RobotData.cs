using OdinSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RobotData
{
    [SerializeField]
    public List<RobotDataEntry> robotDataEntries;

    public void SaveState(string filePath)
    {
        byte[] bytes = SerializationUtility.SerializeValue(this.robotDataEntries, DataFormat.Binary);
        File.WriteAllBytes(filePath, bytes);
    }

    public void LoadState(string filePath)
    {
        if (!File.Exists(filePath)) return; // No state to load

        byte[] bytes = File.ReadAllBytes(filePath);
        this.robotDataEntries = SerializationUtility.DeserializeValue<List<RobotDataEntry>>(bytes, DataFormat.Binary);
    }
}

[Serializable]
public class RobotDataEntry
{
    public string robotPartIdentifier;
    public Vector3 robotPartLocalPosition;
    public Quaternion robotPartLocalRotation;
    public RobotDataEntrySettings robotPartSettings;

    public RobotDataEntry(string robotPartIdentifier, Vector3 robotPartLocalPosition, Quaternion robotPartLocalRotation, RobotDataEntrySettings robotPartSetting)
    {
        this.robotPartIdentifier = robotPartIdentifier;
        this.robotPartLocalPosition = robotPartLocalPosition;
        this.robotPartLocalRotation = robotPartLocalRotation;
        this.robotPartSettings = robotPartSetting;
    }
}

[Serializable]
public class RobotDataEntrySettings
{
    public List<bool> boolSettings;

    public RobotDataEntrySettings(List<bool> boolSettings)
    {
        this.boolSettings = boolSettings;
    }
}
