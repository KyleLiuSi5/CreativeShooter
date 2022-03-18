using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RRRRR : NetworkBehaviour {

    public bool PushR;
    public bool CanPushR;
    public float RRRRRCDTime = 25f;
    private float RRRRRTime = 0f;
	public GameObject RRRREffect;
	RRRRactive _rrrractive;
    public Weapon RRRRRWeapon;
    public Weapon InitialWeapon;
    playerScript _playersc;
    public GameObject _Skill;
    CharacterBehavior _character;

    void Start()
    {
		_rrrractive = GetComponent<RRRRactive> ();
        PushR = false;
        CanPushR = true;
        RRRRRCDTime = 25f;
        RRRRRTime = 0f;
        _playersc = GetComponent<playerScript>();
        _character = GetComponent<CharacterBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (CanPushR == true && _character.gameObject.tag != "Die")
            {
                PushR = true;
				_rrrractive.CmdSetRRRactive (true);
				RRRREffect.GetComponent<SpriteRenderer> ().enabled = true;
				CmdWeaponChange();
                RRRRRCDTime = 0f;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;
        }
        if (RRRRRCDTime <= 25f)
        {
            RRRRRCDTime += Time.deltaTime;
            CanPushR = false;
        }
        else
        {
			
			CanPushR = true;
        }
		if (RRRRRCDTime > 5) 
		{
			_rrrractive.CmdSetRRRactive (false);
			RRRREffect.GetComponent<SpriteRenderer> ().enabled = false;
		}
        if (PushR == true)
        {
            RRRRRTime += Time.deltaTime;

        }
        if (_playersc.RRRRRTime > 5f)
        {
            PushR = false;
            CmdWeaponBack();
            RRRRRTime = 0f;
        }

        if(gameObject.tag == "Die" && PushR == true)
        {
            PushR = false;
            CmdWeaponBack();
            RRRRRTime = 0f;
        }

    }
    [Command]
    public void CmdWeaponChange()
    {
        RpcWeaponChange();
    }
    [ClientRpc]
    public void RpcWeaponChange()
    {
        gameObject.GetComponent<CharacterShoot>().ChangeWeapon(RRRRRWeapon);        
        _playersc.Switchj = 1;
    }
    [Command]
    public void CmdWeaponBack()
    {
        RpcWeaponBack();
    }
    [ClientRpc]
    public void RpcWeaponBack()
    {
        gameObject.GetComponent<CharacterShoot>().ChangeWeapon(InitialWeapon);
        _playersc.Switchj = 0;
    }
}
