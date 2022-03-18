using UnityEngine;
using System.Collections;

public class SpeedUp : MonoBehaviour {

    public bool PushE;
    public bool CanPushE;
    public float speedupCDtime = 20f;
	public SPupactive _Spactive;
	public GameObject SpeedupEffect;
    public GameObject _Skill;
    CharacterBehavior _character;



    private GameObject _Player;

    void Start()
    {
		_Spactive = GetComponent<SPupactive>();
        speedupCDtime = 20f;
        CanPushE = true;
        _character = GetComponent<CharacterBehavior>();
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.E))
        {
            if (CanPushE == true && _character.gameObject.tag != "Die")
            {
                PushE = true;
				_Spactive.CmdSpactive (true);
				SpeedupEffect.GetComponent<SpriteRenderer> ().enabled=true;
				speedupCDtime = 0;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;

        }
        if (speedupCDtime <= 20)
        {
            speedupCDtime += Time.deltaTime;
            CanPushE = false;
        }
        else
        {
			CanPushE = true;
        }
        if(_character.gameObject.tag == "Die")
        {
            PushE = false;
            SpeedupEffect.GetComponent<SpriteRenderer>().enabled = false;
            _Spactive.CmdSpactive(false);
        }

    }
}
