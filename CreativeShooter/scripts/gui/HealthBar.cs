using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class HealthBar : NetworkBehaviour 
{
	/// the healthbar's foreground sprite
	public Transform ForegroundSprite;
    public Transform TankgroundSprite;
	/// the color when at max health
	public Color MaxHealthColor = new Color(255/255f, 63/255f, 63/255f);
	/// the color for min health
	public Color MinHealthColor = new Color(64/255f, 137/255f, 255/255f);
    public Color TankHealthColor = new Color(154 / 255f, 153 / 255f, 152 / 255f);
	
	private CharacterBehavior _character;
    private Tank _tank;

	/// <summary>
	/// Initialization, gets the player
	/// </summary>
	void Start()
	{
        _character = gameObject.GetComponentInParent<CharacterBehavior>();
        if(gameObject.GetComponentInParent<Tank>() != null)
        {
            _tank = _character.GetComponent<Tank>();
        }
        
    }

	/// <summary>
	/// Every frame, sets the foreground sprite's width to match the character's health.
	/// </summary>
	public void Update()
	{
		if (_character==null)
			return;
		var healthPercent = _character.Health / (float) _character.BehaviorParameters.MaxHealth;
        var tankHealthPercent = _character.TankHealth / (float)_character.BehaviorParameters.TankMaxHealth;
        ForegroundSprite.localScale = new Vector3(healthPercent,1,1);
        TankgroundSprite.localScale = new Vector3(tankHealthPercent, 0, 0);

        if (_tank != null)
        {
            if (_tank.PushR == true)
            {
                
                TankgroundSprite.localScale = new Vector3(tankHealthPercent, 1, 1);
                ForegroundSprite.localScale = new Vector3(healthPercent, 0, 0);
            }
            else
            {
                
                ForegroundSprite.localScale = new Vector3(healthPercent, 1, 1);
                TankgroundSprite.localScale = new Vector3(tankHealthPercent, 0, 0);
            }
        }
        else
            return;
        
		
	}
	
}
