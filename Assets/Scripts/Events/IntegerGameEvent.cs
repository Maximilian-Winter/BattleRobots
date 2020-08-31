using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Integer Game Event", menuName = "Battle Robots/Events/IntegerGameEvent", order = 51)]
public class IntegerGameEvent : ScriptableObject
{
	private List<IntegerGameEventListener> listeners =
		new List<IntegerGameEventListener>();

	public void Raise(int integerData)
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised(integerData);
	}

	public void RegisterListener(IntegerGameEventListener listener)
	{ listeners.Add(listener); }

	public void UnregisterListener(IntegerGameEventListener listener)
	{ listeners.Remove(listener); }
}