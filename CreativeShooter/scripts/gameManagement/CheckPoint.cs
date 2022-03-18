using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Checkpoint class. Will make the player respawn at this point if it dies.
/// </summary>
public class CheckPoint : MonoBehaviour 
{
	private List<IPlayerRespawnListener> _listeners;

	/// <summary>
	/// Initializes the list of listeners
	/// </summary>
	public void Awake () 
	{
		_listeners = new List<IPlayerRespawnListener>();
	}
	

	public void SpawnPlayer(CharacterBehavior player)
	{
		player.RespawnAt(transform);
		
		foreach(var listener in _listeners)
		{
			listener.onPlayerRespawnInThisCheckpoint(this,player);
		}
	}
	
	public void AssignObjectToCheckPoint (IPlayerRespawnListener listener) 
	{
		_listeners.Add(listener);
	}
}
