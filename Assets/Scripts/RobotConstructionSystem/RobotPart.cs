using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RobotPartType
{
    CorePart,
    WheelPart,
    HingePart
}

[CreateAssetMenu(fileName = "new Robot Part", menuName = "Battle Robots/Robot Part", order = 51)]
public class RobotPart : ScriptableObject
{
    public string robotPartIdentifier;
    public string robotPartName;
    public RobotPartType robotPartType;
    public Vector3 robotPartOffset;
    public GameObject robotPartPrefab;
}
