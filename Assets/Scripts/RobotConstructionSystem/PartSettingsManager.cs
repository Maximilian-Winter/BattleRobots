using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartSettingsManager : MonoBehaviour
{
    [SerializeField]
    private RobotConstructionController robotConstructionController;

    [SerializeField]
    private GameObject partSettingsTab;

    [SerializeField]
    private Text partSettingsPartName;

    [SerializeField]
    private Text partSettingsPartType;

    [SerializeField]
    private GameObject partSettingsWheelSettings;

    [SerializeField]
    private Toggle partSettingsWheelDirectionToggle;

    [SerializeField]
    private Toggle partSettingsWheelActivateMotorToggle;

    [SerializeField]
    private Toggle partSettingsWheelActivateSteeringToggle;

    [SerializeField]
    private GameObject partSettingsButton;

    private bool isPartSettingsOpen = false;

    public bool IsPartSettingsOpen { get => isPartSettingsOpen; set => isPartSettingsOpen = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPartSettings()
    {
        IsPartSettingsOpen = true;
        if (robotConstructionController.SelectedRobotPart != null && robotConstructionController.SelectedRobotPartGameObject != null)
        {
            partSettingsPartName.text = "Part Name: " + robotConstructionController.SelectedRobotPart.robotPartName;

            if (robotConstructionController.SelectedRobotPart.robotPartType == RobotPartType.CorePart)
            {
                partSettingsPartType.text = "Part Type: Core Part";
                partSettingsWheelSettings.SetActive(false);
            }
            else if (robotConstructionController.SelectedRobotPart.robotPartType == RobotPartType.WheelPart)
            {
                partSettingsPartType.text = "Part Type: Wheel";
                partSettingsWheelSettings.SetActive(true);
                partSettingsWheelActivateMotorToggle.isOn = robotConstructionController.SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().GetActivateMotor();
                partSettingsWheelActivateSteeringToggle.isOn = robotConstructionController.SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().GetActivateSteering();
                partSettingsWheelDirectionToggle.isOn = robotConstructionController.SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().GetReverseSpinDirection();
            }
        }
    }

    public void ClosePartSettings()
    {
        IsPartSettingsOpen = false;
    }

    public void ShowPartSettingsButton()
    {
        partSettingsButton.SetActive(true);
    }

    public void HidePartSettingsButton()
    {
        partSettingsButton.SetActive(false);
    }


    public void PartSettingActivateMotorHasChanged(bool value)
    {
        if (robotConstructionController.SelectedRobotPart != null && robotConstructionController.SelectedRobotPartGameObject != null)
        {
            if (robotConstructionController.SelectedRobotPart.robotPartType == RobotPartType.WheelPart)
            {
                robotConstructionController.SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetActivateMotor(value);
            }
        }
    }

    public void PartSettingActivateSteeringHasChanged(bool value)
    {
        if (robotConstructionController.SelectedRobotPart != null && robotConstructionController.SelectedRobotPartGameObject != null)
        {
            if (robotConstructionController.SelectedRobotPart.robotPartType == RobotPartType.WheelPart)
            {
                robotConstructionController.SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetActivateSteering(value);
            }
        }
    }

    public void PartSettingReverseWheelDirectionHasChanged(bool value)
    {
        if (robotConstructionController.SelectedRobotPart != null && robotConstructionController.SelectedRobotPartGameObject != null)
        {
            if (robotConstructionController.SelectedRobotPart.robotPartType == RobotPartType.WheelPart)
            {
                robotConstructionController.SelectedRobotPartGameObject.GetComponent<SimpleWheelController>().SetReverseSpinDirection(value);
            }
        }
    }
}
