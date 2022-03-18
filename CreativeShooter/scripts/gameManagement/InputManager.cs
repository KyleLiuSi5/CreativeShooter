using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


public class InputManager : MonoBehaviour
{
	private CharacterBehavior _player;

    void Start()
    {
        _player = gameObject.GetComponent<CharacterBehavior>();
    }
	
	void Update()
	{		


		if (!_player.canMove)
			return;
			
		_player.SetHorizontalMove(CrossPlatformInputManager.GetAxis ("Horizontal"));       
		_player.SetVerticalMove(CrossPlatformInputManager.GetAxis ("Vertical"));
		
		if ((CrossPlatformInputManager.GetButtonDown("Run")||CrossPlatformInputManager.GetButton("Run")) )
			_player.RunStart();		
		
		if (CrossPlatformInputManager.GetButtonUp("Run"))
			_player.RunStop();		
				
		if (CrossPlatformInputManager.GetButtonDown ("Jump")) 
		{
			_player.JumpStart ();
		}
						
		if (CrossPlatformInputManager.GetButtonUp("Jump"))
		{
			_player.JumpStop();
		}
					
		if (_player.GetComponent<CharacterMelee>() != null) 
		{		
			if ( CrossPlatformInputManager.GetButtonDown("Melee")  )
				_player.GetComponent<CharacterMelee>().Melee();
		}
        
		
		if (_player.GetComponent<CharacterShoot>() != null) 
		{
			_player.GetComponent<CharacterShoot>().SetHorizontalMove(CrossPlatformInputManager.GetAxis ("Horizontal"));
			_player.GetComponent<CharacterShoot>().SetVerticalMove(CrossPlatformInputManager.GetAxis ("Vertical"));

			if (CrossPlatformInputManager.GetButtonDown("Fire"))
                _player.GetComponent<CharacterShoot>().ShootOnce();
            if (CrossPlatformInputManager.GetButton("Fire"))
                _player.GetComponent<CharacterShoot>().ShootStart();
            if (CrossPlatformInputManager.GetButtonUp("Fire"))
				_player.GetComponent<CharacterShoot>().CmdShootStop();

		}

		if (_player.GetComponent<CharacterJetpack>()!=null)
		{
			if ((CrossPlatformInputManager.GetButtonDown("Jetpack")||CrossPlatformInputManager.GetButton("Jetpack")) )
				_player.GetComponent<CharacterJetpack>().CmdJetpackStart();
			
			if (CrossPlatformInputManager.GetButtonUp("Jetpack"))
				_player.GetComponent<CharacterJetpack>().CmdJetpackStop();
		}
	}	
}
