using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class YTT : NetworkBehaviour {

    public bool PushR;
    public bool CanPushR;


    private CharacterBehavior _player;
    public float YTTCDTime = 5f;

    public int i ;
    public Weapon ShotGunWeapon;
    public Weapon InitialWeapon;
    public GameObject _Skill;
    playerScript _playersc;

    void Start () {


        _player = GetComponent<CharacterBehavior>();
        PushR = false;
        CanPushR = true;
        YTTCDTime = 5f;
        i = 0;
        _playersc = GetComponent<playerScript>();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.R))
        {
            if (CanPushR == true && _player.gameObject.tag != "Die")
            {
                PushR = true;
                YTTCDTime = 0f;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;
        }
            if (YTTCDTime <= 5f)
            {
                YTTCDTime += Time.deltaTime;
                CanPushR = false;
            }
            else
            {
                CanPushR = true;
            }
            if(PushR == true)
            {
            if (i == 0)
            {
                i++;
                _playersc.Switchi = 1;
                CmdWeaponChange();
            }
            else
            {
                i--;
                _playersc.Switchi = 0;
                CmdChangeBack();
            }                  
            PushR = false;
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
        gameObject.GetComponent<CharacterShoot>().ChangeWeapon(ShotGunWeapon);
    }
    [Command]
    public void CmdChangeBack()
    {
        RpcChangeBack();
    }
    [ClientRpc]
    public void RpcChangeBack()
    {
        gameObject.GetComponent<CharacterShoot>().ChangeWeapon(InitialWeapon);
    }

}
