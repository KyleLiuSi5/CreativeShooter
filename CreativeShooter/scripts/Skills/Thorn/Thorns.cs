using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Thorns : NetworkBehaviour {

    public bool PushE;
    public bool CanPushE;
    public bool PushR;
    public bool CanPushR;
    public float ThorntimeCount = 0;
    public float ShieldAttackCDTime = 0;
    public float ThornCDTime = 0;
    public float ShieldAttackArea = 0;
    private CharacterBehavior _characterBehavior;
    public GameObject ShieldAttackCollider;
    public GameObject _shieldAtk;
    public GameObject _shieldUp;
    private CorgiController _controller;
    C876isActive _active;
    ShieldATKActive _shieldAtkactive;
    ShieldUpactive _shieldUpactive;
    public GameObject _Skill1;
    public GameObject _Skill2;


    // Use this for initialization
    void Start () {
        CanPushE = true;
        CanPushR = true;
        ThornCDTime = 30f;
        ShieldAttackCDTime = 20f;
        _controller = GetComponent<CorgiController>();
        _characterBehavior = GetComponent<CharacterBehavior>();
        _active = GetComponent<C876isActive>();
        _shieldAtkactive = GetComponent<ShieldATKActive>();
        _shieldUpactive = GetComponent<ShieldUpactive>();
    }
	
	// Update is called once per frame
	void Update () {

        
        if (Input.GetKeyDown(KeyCode.E))
            if (CanPushE == true && gameObject.tag != "Die")
            {
                ThornCDTime = 0;
                PushE = true;
                _shieldUpactive.CmdshieldupActive(true);
                _shieldUp.GetComponent<SpriteRenderer>().enabled = true;
                _Skill1.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }              
            else
                return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CanPushR == true && gameObject.tag != "Die")
            {
                ShieldAttackCDTime = 0;
                PushR = true;
                _active.CmdSetShieldactive(true);
                _shieldAtkactive.CmdsetShieldATK(true);
                _shieldAtk.GetComponent<SpriteRenderer>().enabled = true;
                _Skill2.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;

        }
            
            


        if (ThornCDTime < 30)//荊棘之盾的CD時間
        {
            ThornCDTime += Time.deltaTime;
            CanPushE = false;
        }
        else
        {
            CanPushE = true;
        }

        

        if (ShieldAttackCDTime < 20)//盾擊的CD時間
        {
            ShieldAttackCDTime += Time.deltaTime;
            CanPushR = false;
        }
        else
        {
            CanPushR = true;
        }

        if (PushE == true)
        {
            ThorntimeCount += Time.deltaTime;
            if(ThorntimeCount < 10)
            {
                this.GetComponent<CharacterBehavior>().CmdChangeTag("ThornsShield");
            }
            else if(ThorntimeCount >= 10)
            {
                this.GetComponent<CharacterBehavior>().CmdChangeTag("Player");
                ThorntimeCount = 0;
                _shieldUpactive.CmdshieldupActive(false);
                _shieldUp.GetComponent<SpriteRenderer>().enabled = false;
                PushE = false;
            }
            else if(gameObject.GetComponent<CharacterBehavior>().Health <= 0)
            {
                this.GetComponent<CharacterBehavior>().CmdChangeTag("Die");
                ThorntimeCount = 0;
                PushE = false;
                _characterBehavior.CmdKillPlayer();
            }
        }

        if(PushR == true)
        {
            ShieldAttackCollider.GetComponent<Collider2D>().enabled = true;
            PushR = false;
        }
        else
        {
            ShieldAttackCollider.GetComponent<Collider2D>().enabled = false;
            _active.CmdSetShieldactive(false);
            _shieldAtkactive.CmdsetShieldATK(false);
            _shieldAtk.GetComponent<SpriteRenderer>().enabled = false;
        }

    }

    


}
