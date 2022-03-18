using UnityEngine;
using System.Collections;
/// <summary>
/// Manages the jetpack bar
/// </summary>
public class JetpackBar : MonoBehaviour 
{
	public Transform ForegroundBar;
	public Color MaxFuelColor = new Color(36/255f, 199/255f, 238/255f);
	public Color MinFuelColor = new Color(24/255f, 164/255f, 198/255f);
	
	private CharacterBehavior _character;
	private CharacterJetpack _jetpack;

	void Start()
	{
        _character = gameObject.GetComponentInParent<CharacterBehavior>() ;
		if (_character!=null)
			_jetpack=_character.GetComponent<CharacterJetpack>();
	}

	public void Update()
	{
		if (_jetpack==null)
			return;
		if (_character==null)
			return;
		
		float jetpackPercent = _character.BehaviorState.JetpackFuelDurationLeft / (float) _jetpack.JetpackFuelDuration;
		ForegroundBar.localScale = new Vector3(jetpackPercent,1,1);		
	}	
}
