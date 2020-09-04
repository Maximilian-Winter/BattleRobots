using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDCarDriver : MonoBehaviour
{
	public PIDController torquePID;
	public PIDController steeringPID;
	public Transform target;
	private float inputSteer;
	private float inputTorque;

	
	public float InputSteer { get => inputSteer; set => inputSteer = value; }
	public float InputTorque { get => inputTorque; set => inputTorque = value; }

	void Start()
	{
	}

	void Update()
	{
		Vector3 relativeWaypointPosition = transform.InverseTransformPoint(target.position);

		float steer = relativeWaypointPosition.x / relativeWaypointPosition.magnitude;

		float steerErr = inputSteer - steer;
		inputSteer = steeringPID.Update(steerErr);

		float torque = relativeWaypointPosition.z / relativeWaypointPosition.magnitude;

		float torqueErr = inputTorque - torque;
		inputTorque = torquePID.Update(torqueErr);

		Debug.Log(inputSteer + " " + inputTorque);
	}
}
