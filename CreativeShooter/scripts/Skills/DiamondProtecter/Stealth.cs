using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Stealth : NetworkBehaviour {

    public bool PushR;
    public bool CanPushR;
    public float StealthCDTime = 30f;
    public float existTime = 0f;
    private CharacterBehavior _Behavior;
    private DiamondAI _useSkill;
    public float StealthTime = 0f;
    public CharacterJetpack _jetpack;
    public GameObject _Skill;
    public GameObject _Root;
    CharacterBehavior _Character;

    // Use this for initialization
    void Start()
    {

        PushR = false;
        CanPushR = true;
        StealthCDTime = 30f;
        StealthTime = 0f;
        _Behavior = gameObject.GetComponent<CharacterBehavior>();
        _useSkill = gameObject.GetComponent<DiamondAI>();
        _jetpack = GetComponent<CharacterJetpack>();
        _Character = GetComponent<CharacterBehavior>();
    }

    // Update is called once per frame
    void Update () {
        
        if (Input.GetKey(KeyCode.R))
        {
            if (CanPushR == true && _Character.gameObject.tag != "Die")
            {
                PushR = true;
                StealthCDTime = 0f;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;
        }

        if(StealthTime >= 1 && Input.GetMouseButton(0))
        {
            existTime = 5;
        }

        if(StealthCDTime < 30f)
        {
            CanPushR = false;
            StealthCDTime += Time.deltaTime;
        }
        else
        {
            CanPushR = true;
        }

        if (PushR == true)
        {
            CmdStealth();
            StealthTime = 1;
            PushR = false;
        }

        if(StealthTime >= 1)
        {
            existTime += Time.deltaTime;
            
        }
        if(existTime >= 5f)
        {
            RpcStealthBack();
            StealthTime = 0;
            existTime = 0;
        }    
        if(_Character.gameObject.tag == "Die" && existTime != 0)
        {
            RpcStealthBack();
            StealthTime = 0;
            existTime = 0;
        }
	}

    [Command]
    public void CmdStealth()
    {
        RpcStealth();
    }
    [ClientRpc]
    public void RpcStealth()
    {
        _Root.transform.localScale *= 0.1f;
        
    }
    [Command]
    public void CmdStealthBack()
    {
        RpcStealthBack();
    }
    [ClientRpc]
    public void RpcStealthBack()
    {
        _Root.transform.localScale *= 10f;
    }

}
