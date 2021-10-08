﻿using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have a bool argument.
/// Example: An event to toggle a UI interface
/// </summary>

[CreateAssetMenu(menuName = "Events/Bool Event Channel")]
public class BoolEventChannelSO : ScriptableObject
{
	public UnityAction<bool> OnEventRaised;
	public void RaiseEvent(bool value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
	public void UnsubscribeAll()
	{
		if (OnEventRaised != null)
		{
			if (OnEventRaised.GetInvocationList() != null)

				foreach (System.Delegate d in OnEventRaised.GetInvocationList())
				{
					OnEventRaised -= d as UnityAction<bool>;

				}
		}


	}
}
