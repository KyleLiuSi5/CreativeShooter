using UnityEngine;
using System.Collections;
/// <summary>
/// Add this class to an object that should give damage to the player when colliding with it
/// </summary>
public class GiveDamageToPlayer : MonoBehaviour 
{
	/// The amount of health to remove from the player's health
	public int DamageToGive = 10;
	
	// storage		
	private Vector2
		_lastPosition,
		_velocity;
	
	/// <summary>
	/// During last update, we store the position and velocity of the object
	/// </summary>
	public void LateUpdate () 
	{
		_velocity = (_lastPosition - (Vector2)transform.position) /Time.deltaTime;
		_lastPosition = transform.position;
	}
	

}
