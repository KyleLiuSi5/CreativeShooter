using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class Wheel : NetworkBehaviour
{

    public bool PushR;
    public bool CanPushR;

    private CharacterBehavior _player;
    CharacterShoot _shoot;
    public GameObject _Skill;
    public Weapon Wheelweapon;
    public Weapon InitialWeapon;
    playerScript _playersc;
    public float WheelCDTime = 10f;
    public float WheelTime;

    void Start()
    {
        _player = GetComponent<CharacterBehavior>();
        PushR = false;
        CanPushR = true;
        WheelCDTime = 15f;
        _shoot = GetComponent<CharacterShoot>();
        _playersc = gameObject.GetComponent<playerScript>();

    }

    void Update()
    {


        if (Input.GetKey(KeyCode.R))
        {
            if (CanPushR == true && _player.gameObject.tag != "Die")
            {
                PushR = true;
                CmdChangeWeapon();
                WheelCDTime = 0f;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
        }

        if(PushR == true)
        {
            _shoot.ShootStart();
            CmdChangeWeaponBack();
            WheelTime += Time.deltaTime;
        }
        if(WheelTime >= 0.5f)
        {
            CmdChangeWeaponBack();
            PushR = false;
        }
        if(PushR == false)
        {
            WheelTime = 0;
        }
        
        if (WheelCDTime <= 10f)
        {
            WheelCDTime += Time.deltaTime;
            CanPushR = false;
        }
        else
        {
            CanPushR = true;
        }

    }

    [Command]
    public void CmdChangeWeapon()
    {
        RpcChangeWeapon();
    }
    [ClientRpc]
    public void RpcChangeWeapon()
    {
        gameObject.GetComponent<CharacterShoot>().ChangeWeapon(Wheelweapon);
        _playersc.SwitchW = 1;
    }
    [Command]
    public void CmdChangeWeaponBack()
    {
        RpcChangeWeaponBack();
    }
    [ClientRpc]
    public void RpcChangeWeaponBack()
    {
        gameObject.GetComponent<CharacterShoot>().ChangeWeapon(InitialWeapon);
        _playersc.SwitchW = 0;
    }

}
