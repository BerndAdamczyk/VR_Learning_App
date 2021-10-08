using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one transform argument.
/// Example: Spawn system initializes player and fire event, where the transform is the position of player.
/// </summary>

[CreateAssetMenu(menuName = "Events/GameObject Event Channel")]
public class GameObjectEventChannelSO : EventChannelBaseSO
{
	public UnityAction<GameObject> OnEventRaised;

	public void RaiseEvent(GameObject value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
