using UnityEngine;
using System.Collections;

public class SummonerSkill : MonoBehaviour {

    private SummonSmallMonster _SmallMonster;
    private SummonZombie _SummonZombie;
    private SummonExplosionMonster _SummonExplosionMonster;
    private CharacterBehavior _player;

    // Use this for initialization
    void Start()
    {

        _player = gameObject.GetComponentInParent<CharacterBehavior>();
        _SmallMonster = gameObject.GetComponentInParent<SummonSmallMonster>();
        _SummonZombie = gameObject.GetComponentInParent<SummonZombie>();
        _SummonExplosionMonster = gameObject.GetComponentInParent<SummonExplosionMonster>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PushSummonSmallMonsterButton()
    {
        if (_SmallMonster.CanPushE == true)
        {
            _SmallMonster.PushECDTime = 0;
            _SmallMonster.PushE = true;
        }
        else
            return;
    }

    public void PushSummonZombieButton()
    {
        if (_SummonZombie.CanPushR == true)
        {
            _SummonZombie.PushRCDTime = 0;
            _SummonZombie.PushR = true;
        }
        else
            return;
    }

    public void PushSummonExplosionMonsterButton()
    {
        if (_SummonExplosionMonster.CanPushT == true)
        {
            _SummonExplosionMonster.PushTCDTime = 0;
            _SummonExplosionMonster.PushT = true;
        }
        else
            return;
    }

}
