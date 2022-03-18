using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Tank : NetworkBehaviour {

    public bool PushR;
    public bool CanPushR;
    public float TankCDTime = 70f;
    private float TankTime = 0f;
    public GameObject _Tankidle;
    public Weapon TankWeapon;
    public Weapon InitialWeapon;
    playerScript _playersc;
    TankActive _active;
    CharacterBehavior _character;
    public GameObject _Skill;

    // Use this for initialization
    void Start() {
        PushR = false;
        CanPushR = true;
        TankCDTime = 70f;
        TankTime = 0f;
        _playersc = GetComponent<playerScript>();
        _Tankidle.GetComponent<TankBehavior>().enabled = false;
        _active = GetComponent<TankActive>();
        _character = GetComponent<CharacterBehavior>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.R))
        {
            if (CanPushR == true && _character.gameObject.tag != "Die")
            {
                PushR = true;
                CmdWeaponChange();
                SetTankActive();
                _active.CmdactiveTrue(true);
                TankCDTime = 0f;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;
        }

        if (TankCDTime < 70f)
        {
            TankCDTime += Time.deltaTime;
            CanPushR = false;
        }
        else
        {
            CanPushR = true;
        }

        if (PushR == true)
        {
            TankTime += Time.deltaTime;
        }
        if (TankTime > 20)
        {
            PushR = false;
            CmdWeaponBack();
            _active.CmdactiveTrue(false);
            SetTankBack();
            TankTime = 0f;
        }
        if(_character.gameObject.tag == "Die" && PushR == true)
        {
            PushR = false;
            CmdWeaponBack();
            _active.CmdactiveTrue(false);
            SetTankBack();
            TankTime = 0f;
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
        gameObject.GetComponent<CharacterShoot>().ChangeWeapon(TankWeapon);
        _playersc.SwitchT = 1;

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
        _playersc.SwitchT = 0;
    }


    public void SetTankActive()
    {
        _Tankidle.GetComponent<SpriteRenderer>().enabled = true;
        _Tankidle.GetComponent<PolygonCollider2D>().enabled = true;
        _Tankidle.GetComponent<TankBehavior>().enabled = true;
    }

    public void SetTankBack()
    {
        _Tankidle.GetComponent<SpriteRenderer>().enabled = false;
        _Tankidle.GetComponent<PolygonCollider2D>().enabled = false;
        _Tankidle.GetComponent<TankBehavior>().enabled = false;
    }

}
