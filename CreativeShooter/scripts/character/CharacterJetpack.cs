using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class CharacterJetpack : NetworkBehaviour 
{
	
	public ParticleSystem Jetpack;
	
	public float JetpackForce = 2.5f;	
	public bool JetpackUnlimited = false;
	public float JetpackFuelDuration = 5f;
	public float JetpackRefuelCooldown=1f;
    public GameObject _UICamera;
	private CharacterBehavior _characterBehavior;
	private CorgiController _controller;
    private Tank _tank;
    private Stealth _stealth;
	
	void Start () 
	{
		_characterBehavior = GetComponent<CharacterBehavior>();
		_controller = GetComponent<CorgiController>();
        _tank = GetComponent<Tank>();
        _stealth = GetComponent<Stealth>();
	
		if (Jetpack!=null)
		{
			Jetpack.enableEmission=false;
			_UICamera.GetComponent<GUIManager>().SetJetpackBar (!JetpackUnlimited);
			_characterBehavior.BehaviorState.JetpackFuelDurationLeft = JetpackFuelDuration;		
		}
	}
	

    [Command]
	public void CmdJetpackStart()
	{
        RpcJetpackStart();	
	}
    [ClientRpc]
    public void RpcJetpackStart()
    {
        if (_tank != null && _tank.PushR == true)
            return;
        if (_stealth != null && _stealth.existTime < 5f && _stealth.existTime > 0f)
            return;

        if ((!_characterBehavior.Permissions.JetpackEnabled) || (!_characterBehavior.BehaviorState.CanJetpack) || (_characterBehavior.BehaviorState.IsDead))
            return;

        if (!_characterBehavior.BehaviorState.CanMoveFreely)
            return;

        if ((!JetpackUnlimited) && (_characterBehavior.BehaviorState.JetpackFuelDurationLeft <= 0f))
        {
            CmdJetpackStop();
            _characterBehavior.BehaviorState.CanJetpack = false;
            return;
        }


        _controller.SetVerticalForce(JetpackForce);
        _characterBehavior.BehaviorState.Jetpacking = true;
        _characterBehavior.BehaviorState.CanMelee = false;
        _characterBehavior.BehaviorState.CanJump = false;
        Jetpack.enableEmission = true;
        if (!JetpackUnlimited)
        {
            StartCoroutine(JetpackFuelBurn());

        }
    }
	
    [Command]
	public void CmdJetpackStop()
	{
        RpcJetpackStop();
	}
    [ClientRpc]
    public void RpcJetpackStop()
    {
        if (Jetpack == null)
            return;
        _characterBehavior.BehaviorState.Jetpacking = false;
        _characterBehavior.BehaviorState.CanMelee = true;
        Jetpack.enableEmission = false;
        _characterBehavior.BehaviorState.CanJump = true;
        if (!JetpackUnlimited)
            StartCoroutine(JetpackRefuel());
    }
	
	
	private IEnumerator JetpackFuelBurn()
	{
		float timer=_characterBehavior.BehaviorState.JetpackFuelDurationLeft;
		while ((timer > 0) && (_characterBehavior.BehaviorState.Jetpacking))
		{
			timer -= Time.deltaTime;
			_characterBehavior.BehaviorState.JetpackFuelDurationLeft=timer;
			yield return 0;
		}
	}
	

	private IEnumerator JetpackRefuel()
	{
		yield return new WaitForSeconds (JetpackRefuelCooldown);
		float timer=_characterBehavior.BehaviorState.JetpackFuelDurationLeft;
		while ((timer < JetpackFuelDuration) && (!_characterBehavior.BehaviorState.Jetpacking))
		{
			timer += Time.deltaTime/2;
			_characterBehavior.BehaviorState.JetpackFuelDurationLeft=timer;
			if ((!_characterBehavior.BehaviorState.CanJetpack) && (timer > 1f))
				_characterBehavior.BehaviorState.CanJetpack=true;
			yield return 0;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
