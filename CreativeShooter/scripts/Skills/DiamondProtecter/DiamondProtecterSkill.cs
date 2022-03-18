using UnityEngine;
using System.Collections;

public class DiamondProtecterSkill : MonoBehaviour {

    private Teleport _Teleport;
    private Stealth _Stealth;
    private Avatar _Avatar;
    private CharacterBehavior _player;

    // Use this for initialization
    void Start()
    {

        _player = gameObject.GetComponentInParent<CharacterBehavior>();
        _Teleport = gameObject.GetComponentInParent<Teleport>();
        _Stealth = gameObject.GetComponentInParent<Stealth>();
        _Avatar = gameObject.GetComponentInParent<Avatar>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PushTeleportButton()
    {
        if (_Teleport.CanPushE == true)
        {
            _Teleport.TeleportCDTime = 0;
            _Teleport.PushE = true;
        }
        else
            return;
    }

    public void PushStealthButton()
    {
        if (_Stealth.CanPushR == true)
        {
            _Stealth.StealthCDTime = 0;
            _Stealth.PushR = true;
        }
        else
            return;
    }

    public void PushAvatarButton()
    {
        if (_Avatar.CanPushT == true)
        {
            _Avatar.AvatarCDTime = 0;
            _Avatar.PushT = true;
        }
        else
            return;
    }
}
