using UnityEngine;
using System.Collections;

public class TankSkill : MonoBehaviour {

    private dudu _HasaKi;
    private RRRRR _RRRRR;
    private c8c8 _TerrainUp;
    private CharacterBehavior _player;

    // Use this for initialization
    void Start()
    {

        _player = gameObject.GetComponentInParent<CharacterBehavior>();
        _HasaKi = gameObject.GetComponentInParent<dudu>();
        _RRRRR = gameObject.GetComponentInParent<RRRRR>();
        _TerrainUp = gameObject.GetComponentInParent<c8c8>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PushduduButton()
    {
        if (_HasaKi.CanPushE == true)
        {
            _HasaKi.duduCDTime = 0;
            _HasaKi.PushE = true;
        }
        else
            return;
    }

    public void PushRRRRRButton()
    {
        if (_RRRRR.CanPushR == true)
        {
            _RRRRR.RRRRRCDTime = 0;
            _RRRRR.PushR = true;
        }
        else
            return;
    }

    public void Pushc8c8Button()
    {
        if (_TerrainUp.CanPushT == true)
        {
            _TerrainUp.c8c8CDTime = 0;
            _TerrainUp.PushT = true;
        }
        else
            return;
    }
}
