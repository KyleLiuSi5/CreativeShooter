using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BigSkill : NetworkBehaviour {

    public bool PushT;
    public bool CanPushT;
    public float PushTCDTime = 60f;
	public GameObject _bigskilleffect;
	public GameObject _bigskillArea;
	Unrivalactive _unriavalactive;
	CharacterBehavior _characterbehavior;
    public GameObject _Skill;

    void Start()
    {
        PushT = false;
        CanPushT = true;
		PushTCDTime = 60;
		_unriavalactive = GetComponent<Unrivalactive> ();
		_characterbehavior = GetComponent<CharacterBehavior> ();
    }

    void Update()
    {
        if(PushTCDTime < 60)
        {
            PushTCDTime += Time.deltaTime;
            CanPushT = false;
        }
        else
        {
            CanPushT = true;
        }
		if (PushTCDTime > 3) 
		{
			_bigskilleffect.GetComponent<SpriteRenderer> ().enabled = false;
			_unriavalactive.CmdUnirivalActive (false);
			_bigskillArea.GetComponent<CircleCollider2D> ().enabled = false;
		}

        if (Input.GetKey(KeyCode.T))
        {
            if (CanPushT == true && _characterbehavior.gameObject.tag != "Die")
            {
                PushT = true;
                _characterbehavior.Invincible = true;
                _unriavalactive.CmdUnirivalActive(true);
                _bigskilleffect.GetComponent<SpriteRenderer>().enabled = true;
                _bigskillArea.GetComponent<CircleCollider2D>().enabled = true;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;
        }

        if (PushT == true)
        {
            StopPushT();
        }
    }

    private void StopPushT()
    {
        CanPushT = false;
        PushTCDTime = 0;
        PushT = false;
    }



	[Command]
	public void CmdBigSkill(GameObject _colliderB)
	{
		RpcBigSkill (_colliderB);
	}
	[ClientRpc]
	public void RpcBigSkill(GameObject _colliderB)
	{
		_colliderB.GetComponent<CharacterBehavior> ().Invincible = true;
	}


}
