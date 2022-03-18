using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class C87666 : NetworkBehaviour
{

    public bool PushT;
    public bool CanPushT;

    private CharacterBehavior _player;

    CharacterShoot _characterShoot;



    public Weapon C8766;
    public Weapon InitialWeapon;
    public GameObject _Skill;
    playerScript _playersc;
    public float C876666Time = 0;
    public float C876CDTime = 20f;

    void Start()
    {
        _player = GetComponent<CharacterBehavior>();
        PushT = false;
        CanPushT = true;
        _playersc = gameObject.GetComponent<playerScript>();
        _characterShoot = gameObject.GetComponent<CharacterShoot>();

    }

    void Update()
    {


        if (Input.GetKey(KeyCode.T))
        {
            if (CanPushT == true && _player.gameObject.tag != "Die")
            {
                PushT = true;
                C876CDTime = 0f;
                CmdChangeWeapon();
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
        }
        

        if(PushT == true)
        {
            _characterShoot.ShootStart();
            C876666Time += Time.deltaTime;
        }
        if(C876666Time >= 0.5f)
        {
            CmdChangeWeaponBack();
            PushT = false;
        }
        if(PushT == false)
        {
            C876666Time = 0;
        }

        if (C876CDTime <= 20f)
        {
            C876CDTime += Time.deltaTime;
            CanPushT = false;

        }
        else
        {
            CanPushT = true;
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
        gameObject.GetComponent<CharacterShoot>().ChangeWeapon(C8766);
        _playersc.SwitchC = 1;
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
        _playersc.SwitchC = 0;
    }
    

}

