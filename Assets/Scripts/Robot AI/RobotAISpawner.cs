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

    public List<RobotPartRuntimeObject> RobotParts { get => robotParts; set => robotParts = value; }

    // Start is called before the first frame update
    void Start()
    {
        //RobotParts = saveLoadManager.LoadFile(robotPath, robotBodyGameObject);

        //saveLoadManager.SaveAsScriptableObject(robotDataSO, RobotParts);

        RobotParts = saveLoadManager.LoadFromScriptableObject(robotDataSO, robotBodyGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoIntoTestModeAI()
    {
        if (RobotParts != null)
        {
            foreach (RobotPartRuntimeObject robotPartRuntimeObject in RobotParts)
            {
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
}
