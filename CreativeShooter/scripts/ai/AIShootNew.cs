using UnityEngine;
using System.Collections;

/// <summary>
/// Add this component to a CorgiController2D and it will try to kill your player on sight.
/// </summary>
public class AIShootNew : MonoBehaviour 
{

	/// The fire rate (in seconds)
	public float FireRate = 5;
	/// The kind of projectile shot by the agent
	public Projectile Projectile;
	/// The maximum distance at which the AI can shoot at the player
	public float ShootDistance = 50f;

	// private stuff
	private float _canFireIn;
	private Vector2 _direction;
	private Vector2 _directionLeft;
	private CorgiController _controller;
    private CharacterBehavior _player;

	/// initialization
	void Start () 
	{
		_directionLeft = new Vector2(-1,0);
		// we get the CorgiController2D component
		_controller = GetComponent<CorgiController>();
        _player = GetComponent<CharacterBehavior>();
	}

	/// Every frame, check for the player and try and kill it
	void Update () 
	{
		if (_player._isFacingRight == false) 
		{
			_direction=_directionLeft;
		}
		else
		{
			_direction=-_directionLeft;
		}

		// we cast a ray in front of the agent to check for a Player
		Vector2 raycastOrigin = new Vector2(transform.position.x,transform.position.y + (transform.localScale.y *2) );
		RaycastHit2D raycast = Physics2D.Raycast(raycastOrigin,_direction,ShootDistance,1<<LayerMask.NameToLayer("Player"));
		if (!raycast)
			return;

		// if the ray has hit the player, we fire a projectile
		Projectile projectile = (Projectile)Instantiate(Projectile, transform.position,transform.rotation);
		projectile.Initialize(gameObject,_direction,_controller.Speed);
		_canFireIn=FireRate/5;
	}
}
