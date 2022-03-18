using UnityEngine;
using System.Collections;

public class CurerSkill : MonoBehaviour {


    private HealthCure _Cure;
    private Buff _Buff;
    private BigSkill _BigSkill;
    private CharacterBehavior _player;

    // Use this for initialization
    void Start()
    {
        _player = gameObject.GetComponentInParent<CharacterBehavior>();
        _Cure = gameObject.GetComponentInParent<HealthCure>();
        _Buff = gameObject.GetComponentInParent<Buff>();
        _BigSkill = gameObject.GetComponentInParent<BigSkill>();
    }


    public void PushHealthCureButton()
    {
        if (_Cure.CanPushE == true)
        {
            _Cure.CureCDTime = 0;
            _Cure.PushE = true;
        }
        else
            return;
    }

    public void PushBuffButton()
    {
        if (_Buff.CanPushR == true)
        {
            _Buff.buffcool = 0;
            _Buff.PushR = true;
        }
        else
            return;
    }

    public void PushBigSkillButton()
    {
        if (_BigSkill.CanPushT == true)
        {
            _BigSkill.PushTCDTime = 0;
            _BigSkill.PushT = true;
        }
        else
            return;
    }
}
