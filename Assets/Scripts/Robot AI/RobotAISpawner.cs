using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAISpawner : MonoBehaviour
{
    [SerializeField]
    private RobotDataSO robotDataSO;

    [SerializeField]
    private SaveLoadManager saveLoadManager;

    [SerializeField]
    private GameObject robotBodyGameObject;

    [SerializeField]
    private string robotPath = "C:/Users/Maximilian/AppData/LocalLow/DefaultCompany/Battle Robots/Saves/Robots/TestEnemy.robot";

    private List<RobotPartRuntimeObject> robotParts;

    private bool isInTestMode = false;

    public List<RobotPartRuntimeObject> GetRobotParts()
    {
        return robotParts;
    }

    public void SetRobotParts(List<RobotPartRuntimeObject> value)
    {
        robotParts = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        //RobotParts = saveLoadManager.LoadFile(robotPath, robotBodyGameObject);

        //saveLoadManager.SaveAsScriptableObject(robotDataSO, RobotParts);

        SetRobotParts(saveLoadManager.LoadFromScriptableObject(robotDataSO, robotBodyGameObject));
        foreach (RobotPartRuntimeObject robotPartRuntimeObject in GetRobotParts())
        {
            robotPartRuntimeObject.robotPartGameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DeleteRobot()
    {
        if (GetRobotParts() != null)
        {
            foreach (RobotPartRuntimeObject robotPartRuntimeObject in GetRobotParts())
            {
                DestroyImmediate(robotPartRuntimeObject.robotPartGameObject);
            }
        }
        SetRobotParts(null);
    }

    public void ToggleTestMode()
    {
        if (!isInTestMode)
        {
            GoIntoTestModeAI();
        }
        else
        {
            LeaveTestModeAI();
        }
    }

    public void GoIntoTestModeAI()
    {
        isInTestMode = true;
        if (GetRobotParts() != null)
        {
            foreach (RobotPartRuntimeObject robotPartRuntimeObject in GetRobotParts())
            {
                robotPartRuntimeObject.robotPartGameObject.SetActive(true);
                if (robotPartRuntimeObject.robotPart.robotPartType == RobotPartType.CorePart)
                {
                    robotPartRuntimeObject.robotPartGameObject.GetComponent<CorePart>().OnEnterTestingModeAI();
                }
                else if (robotPartRuntimeObject.robotPart.robotPartType == RobotPartType.WheelPart)
                {
                    robotPartRuntimeObject.robotPartGameObject.GetComponent<WheelPart>().OnEnterTestingModeAI();
                }
                else if (robotPartRuntimeObject.robotPart.robotPartType == RobotPartType.HingePart)
                {
                    robotPartRuntimeObject.robotPartGameObject.GetComponent<HingePart>().OnEnterTestingModeAI();
                }
            }
        }

    }

    public void LeaveTestModeAI()
    {
        isInTestMode = false;
        DeleteRobot();
        SetRobotParts(saveLoadManager.LoadFromScriptableObject(robotDataSO, robotBodyGameObject));
        foreach (RobotPartRuntimeObject robotPartRuntimeObject in GetRobotParts())
        {
            robotPartRuntimeObject.robotPartGameObject.SetActive(false);
        }
    }
}
