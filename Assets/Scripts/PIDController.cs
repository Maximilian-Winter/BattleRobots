using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PIDController
{

	[Tooltip("Proportional constant (counters current error)")]
	public float Kp = 0.2f;

	[Tooltip("Integral constant (counters cumulated error)")]
	public float Ki = 0.05f;

	[Tooltip("Derivative constant (fights oscillation)")]
	public float Kd = 1f;

	[Tooltip("Current control value")]
	public float value = 0;

	private float lastError;
	private float integral;

	/// 
	/// Update our value, based on the given error.  We assume here that the
	/// last update was Time.deltaTime seconds ago.
	/// 
	/// <param name="error" />Difference between current and desired outcome.
	/// Updated control value.
	public float Update(float error)
	{
		return Update(error, Time.deltaTime);
	}

	/// 
	/// Update our value, based on the given error, which was last updated
	/// dt seconds ago.
	/// 
	/// <param name="error" />Difference between current and desired outcome.
	/// <param name="dt" />Time step.
	/// Updated control value.
	public float Update(float error, float dt)
	{
		float derivative = (error - lastError) / dt;
		integral += error * dt;
		lastError = error;

		value = Mathf.Clamp01(Kp * error + Ki * integral + Kd * derivative);
		return value;
	}
}
