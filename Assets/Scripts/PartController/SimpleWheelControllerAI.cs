using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWheelControllerAI : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    [SerializeField] private bool activateMotor;
    [SerializeField] private bool reverseSpinDirection;
    [SerializeField] private bool activateSteering;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider wheelCollider;

    [SerializeField] private Transform wheelTransform;

    public bool GetActivateSteering()
    {
        return activateSteering;
    }

    public void SetActivateSteering(bool value)
    {
        activateSteering = value;
    }

    public bool GetActivateMotor()
    {
        return activateMotor;
    }

    public void SetActivateMotor(bool value)
    {
        activateMotor = value;
    }

    public bool GetReverseSpinDirection()
    {
        return reverseSpinDirection;
    }

    public void SetReverseSpinDirection(bool value)
    {
        reverseSpinDirection = value;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }


    private void GetInput()
    {
        horizontalInput = transform.parent.gameObject.transform.GetChild(0).gameObject.GetComponent<RobotAIMotionController>().InputSteer;
        verticalInput = transform.parent.gameObject.transform.GetChild(0).gameObject.GetComponent<RobotAIMotionController>().InputTorque;
        if(verticalInput == 0.0f)
        {
            isBreaking = true;
        }
        else
        {
            isBreaking = false;
        }
    }

    private void HandleMotor()
    {
        if (GetActivateMotor())
        {
            wheelCollider.motorTorque = verticalInput * motorForce;
        }
        if(GetReverseSpinDirection())
        {
            wheelCollider.motorTorque *= -1;
        }
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        wheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        if (GetActivateSteering())
        {
            currentSteerAngle = maxSteerAngle * horizontalInput;
            wheelCollider.steerAngle = currentSteerAngle;
        }
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(wheelCollider, wheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
