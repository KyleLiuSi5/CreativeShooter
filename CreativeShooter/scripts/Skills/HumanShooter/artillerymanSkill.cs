using UnityEngine;
using System.Collections;

public class artillerymanSkill : MonoBehaviour {

    private SpeedUp _SpeedUp;
    private YTT _SwitchWeapon;
    private uuu _penetratebullet;
    private CharacterBehavior _player;

    // Use this for initialization
    void Start()
    {
        _player = gameObject.GetComponentInParent<CharacterBehavior>();
        _SpeedUp = gameObject.GetComponentInParent<SpeedUp>();
        _SwitchWeapon = gameObject.GetComponentInParent<YTT>();
        _penetratebullet = gameObject.GetComponentInParent<uuu>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PushSpeedUpButton()
    {
        if (_SpeedUp.CanPushE == true)
        {
            _SpeedUp.speedupCDtime = 0;
            _SpeedUp.PushE = true;
        }
        else
            return;
    }

    public void PushSwitchWeaponButton()
    {
        if (_SwitchWeapon.CanPushR == true)
        {
            _SwitchWeapon.YTTCDTime = 0;
            _SwitchWeapon.PushR = true;
        }
        else
            return;
    }

    public void PushpenetratebulletButton()
    {
        if (_penetratebullet.CanPushT == true)
        {
            _penetratebullet.UUUCDTime = 0;
            _penetratebullet.PushT = true;
        }
        else
            return;
    }
}
