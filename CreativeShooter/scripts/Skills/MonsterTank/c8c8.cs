using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class c8c8 : NetworkBehaviour {

    public GameObject stone;
    private CharacterBehavior _player;
    private GameObject StoneClone;
    public bool PushT;
    public bool CanPushT;

    public float c8c8CDTime = 50f;
    private float stoneTime = 0f;
    public GameObject _Skill;

	void Start () {
        PushT = false;
        CanPushT = true;
        _player = GetComponent<CharacterBehavior>();
        c8c8CDTime = 50f;
        stoneTime = 0f;
	
	}

	void Update () {
        if(Input.GetKey(KeyCode.T))
        {
            if (CanPushT == true && _player.gameObject.tag != "Die")
            {
                PushT = true;
                if (_player._isFacingRight)
                {
                    CmdStone();
                }
                else
                {
                    CmdStoneLeft();

                }
                c8c8CDTime = 0f;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;
        }
        if(c8c8CDTime <= 50f)
        {
            c8c8CDTime += Time.deltaTime;
            CanPushT = false;
        }
        else
        {
            CanPushT = true;
        }
        if(PushT == true)
        {
            stoneTime += Time.deltaTime;
        }
        if(stoneTime>10)
        {
            PushT = false;
           
            
            
            
            stoneTime = 0f;
        }
	
	}

    [Command]
    public void CmdStone()
    {
        RpcStone();
    }
    [ClientRpc]
    public void RpcStone()
    {
        Vector3 pos = gameObject.transform.position + new Vector3(3f, 0, 50f);
        GameObject stone0clone = Instantiate(stone, pos, Quaternion.identity) as GameObject;
    }
    [Command]
    public void CmdStoneLeft()
    {
        RpcStoneLeft();
    }
    [ClientRpc]
    public void RpcStoneLeft()
    {
        Vector3 pos = gameObject.transform.position + new Vector3(-3f, 0, 50f);
        GameObject stone0clone = Instantiate(stone, pos, Quaternion.identity) as GameObject;
    }

}
