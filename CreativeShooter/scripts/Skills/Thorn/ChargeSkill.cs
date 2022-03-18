using UnityEngine;
using System.Collections;

public class ChargeSkill : MonoBehaviour {

    private Thorns _Thorns;
    private C8763 _C8763;
    private CharacterBehavior _player;

	void Start () {

        _player = gameObject.GetComponentInParent<CharacterBehavior>();
        _Thorns = gameObject.GetComponentInParent<Thorns>();
        _C8763 = gameObject.GetComponentInParent<C8763>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void PushShieldButton()
    {
        if (_Thorns.CanPushE == true)
        {
            _Thorns.ThornCDTime = 0;
            _Thorns.PushE = true;
        }
        else
            return;
    }

    public void PushShieldAttackButton()
    {
        if (_Thorns.CanPushR == true)
        {
            _Thorns.ShieldAttackCDTime = 0;
            _Thorns.PushR = true;
        }
        else
            return;
    }

    public void PushC8763Button()
    {
        if (_C8763.CanPushT == true)
        {
            _C8763.C8763CDTime = 0;
            _C8763.PushT = true;
        }
        else
            return;
    }


}
