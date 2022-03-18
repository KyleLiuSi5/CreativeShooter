using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class C8763 : NetworkBehaviour {

    public bool PushT;
    public bool CanPushT;
    public GameObject C8763Collider;
    private CharacterBehavior _characterBehavior;
    private CorgiController _controller;
    public float C8763CDTime = 50f;
    private float AttackTime = 0f;
    public Weapon C8763weapon;
    public Weapon InitialWeapon;
    public bool Shoot;
    CharacterShoot _Shoot;
    playerScript _playersc;
    C876isActive _active;
    public GameObject _Skill;

    // Use this for initialization
    void Start () {
        _characterBehavior = GetComponent<CharacterBehavior>();
        _controller = GetComponent<CorgiController>();
        C8763CDTime = 50f;
        AttackTime = 0f;
        CanPushT = true;
        _Shoot = GetComponent<CharacterShoot>();
        _playersc = GetComponent<playerScript>();
        Shoot = false;
        C8763Collider.GetComponent<Collider2D>().enabled = false;
        _active = GetComponent<C876isActive>();
    }


    void Update()
    {
        if(Input.GetKey(KeyCode.T))
        {
            if (CanPushT == true && gameObject.tag != "Die")
            {
                PushT = true;
                C8763CDTime = 0;
                CmdChangeWeapon();
                _active.CmdSetC8763active(true);
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
                
            else
                return;
        }

        if(C8763CDTime < 50)
        {
            C8763CDTime += Time.deltaTime;
            CanPushT = false;
        }
        else
        {
            CanPushT = true;
        }

        if (PushT == true)
        {
            if (_characterBehavior.BehaviorState.IsDead != true)
            {
                _characterBehavior.BehaviorState.C8763Attack = true;
                _characterBehavior.Permissions.MeleeAttackEnabled = false;
                _characterBehavior.Permissions.JumpEnabled = false;
                C8763Collider.GetComponent<Collider2D>().enabled = true;
                _characterBehavior.canMove = false;
                _characterBehavior.BehaviorState.CanShoot = false;
                Shoot = true;
                PushT = false;               
            }
            else
                return;            
        }
        else if(PushT == false)
        {
            C8763Collider.GetComponent<Collider2D>().enabled = false;
            _characterBehavior.BehaviorState.C8763Attack = false;
            _active.CmdSetC8763active(false);
        }
        if(Shoot == true)
        {
            AttackTime += Time.deltaTime;
        }
        if (AttackTime >= 1.5)
        {
            _characterBehavior.canMove = true;
            _characterBehavior.BehaviorState.C8763Attack = false;
            _characterBehavior.Permissions.MeleeAttackEnabled = true;
            _characterBehavior.Permissions.JumpEnabled = true;
            Shoot = false;
            AttackTime = 0;
            _characterBehavior.BehaviorState.CanShoot = true;
            _Shoot.ShootOnce();
            CmdChangeWeaponBack();
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
        gameObject.GetComponent<CharacterShoot>().ChangeWeapon(C8763weapon);
        _playersc.SwichC876 = 1;
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
        _playersc.SwichC876 = 0;
    }    

}
