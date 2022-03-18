using UnityEngine;
using System.Collections;

public class CommanderSkill : MonoBehaviour {

    private Bomb _Bomb;
    private Tank _Tank;
    private CharacterBehavior _player;

    // Use this for initialization
    void Start()
    {
            _player = gameObject.GetComponentInParent<CharacterBehavior>();
            _Bomb = gameObject.GetComponentInParent<Bomb>();
            _Tank = gameObject.GetComponentInParent<Tank>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PushBombButton()
    {
        if (_Bomb.CanPushE == true)
        {
            _Bomb.BombCDTime = 0;
            _Bomb.PushE = true;
        }
        else
            return;
    }

    public void PushTankButton()
    {
        if (_Tank.CanPushR == true)
        {
            _Tank.TankCDTime = 0;
            _Tank.PushR = true;
        }
        else
            return;
    }
}
