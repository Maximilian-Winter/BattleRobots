using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Robot", menuName = "Battle Robots/Robot", order = 51)]
public class RobotDataSO : ScriptableObject
{
    public RobotData robotData;

    public void SetRobotData(RobotData robotData)
    {
        this.robotData = robotData;
    }

    public RobotData GetRobotData()
    {
        return this.robotData;
    }
}
