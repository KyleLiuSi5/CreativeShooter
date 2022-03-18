using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Buff : NetworkBehaviour
{

    public float buffcool = 40f;
    //public bool buff = false;
  
    private CharacterBehavior _characterBehavior;
    private float speednobuff;
    public bool PushR;
    public bool CanPushR;
    public GameObject _BuffRegion;
	Buffactive _buffact;
    public GameObject _Skill;
    // Use this for initialization
    void Start()
    {
       
		_buffact = GetComponent<Buffactive> ();
		CanPushR = true;
		buffcool = 40;
		PushR = false;
        _characterBehavior = GetComponent<CharacterBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.R))
        {
            if (CanPushR == true && _characterBehavior.gameObject.tag != "Die")
            {
                PushR = true;
                _BuffRegion.GetComponent<CircleCollider2D>().enabled = true;
                _buffact.CmdactiveTrue(true);
                _characterBehavior.isBuff = true;
                GameObject.Find("buffeffect").GetComponent<SpriteRenderer>().enabled = true;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;
        }

        if (buffcool < 40)
        {
            buffcool += Time.deltaTime;
            CanPushR = false;
        }
        else
        {
            CanPushR = true;
        }
        if (buffcool > 10)
        {
            GameObject.Find("buffeffect").GetComponent<SpriteRenderer>().enabled = false;
        }

        if (buffcool >= 0.2f)
        {
            _BuffRegion.GetComponent<CircleCollider2D>().enabled = false;
            _buffact.CmdactiveTrue(false);
        }


        if (PushR == true)
        {
            CanPushR = false;
            buffcool = 0;
            PushR = false;
        }


    }
	[Command]
	public void CmdBuffconfirm(GameObject _collider)
	{
		RpcBuffconfirm (_collider);
	}
	[ClientRpc] 
	public void RpcBuffconfirm(GameObject _collider)
	{
		_collider.gameObject.GetComponent<CharacterBehavior> ().isBuff = true;
	}



}