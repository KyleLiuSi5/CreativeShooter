using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HealthCure : NetworkBehaviour
{
    public GameObject _CureArea;
    public GameObject _CureEffect;
    public float CureCDTime = 10f;
    public bool PushE;
    public bool CanPushE;
	CureAreaActive _CureAreaActive;
	CharacterBehavior _character;
    public GameObject _Skill;

    // Use this for initialization
    void Start()
    {
		_CureAreaActive = GetComponent<CureAreaActive>();
        CanPushE = true;
		CureCDTime = 10;
		PushE = false;
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
                CanPushE = false;
                CureCDTime = 0;
                _CureEffect.GetComponent<SpriteRenderer>().enabled = true;
                _CureAreaActive.CmdCureAreaactiveTrue(true);
                _CureArea.GetComponent<CircleCollider2D>().enabled = true;
                _character.isHeal = true;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
                PushE = false;
            }
        }


        if (CureCDTime < 10)
        {
			CanPushE = false;
			CureCDTime += Time.deltaTime;
		}
        else 
		{
			CanPushE = true;
		}

		if (CureCDTime > 2) 
		{
            _CureEffect.GetComponent<SpriteRenderer>().enabled = false;
        }
			
		if (CureCDTime > 0.2f) 
		{
			_CureArea.GetComponent<CircleCollider2D> ().enabled = false;
			_CureAreaActive.CmdCureAreaactiveTrue (false);
		}

    }

	[Command]
	public void Cmdhealconfirm(GameObject _colliderH)
	{
		Rpchealconfirm (_colliderH);
	}
	[ClientRpc]
	public void Rpchealconfirm(GameObject _colliderH)
	{
		_colliderH.GetComponent<CharacterBehavior> ().isHeal = true;
	}
}


