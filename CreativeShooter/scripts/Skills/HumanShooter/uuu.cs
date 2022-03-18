using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class uuu : NetworkBehaviour {

    public bool PushT;
    public bool CanPushT;

    private CharacterBehavior _player;


    public float KeyTime = 0f;
    private bool addTime = false;
    public GameObject _Skill;


    public GameObject U01right;

    public GameObject U01left;
    public int _time = 1;
    public float UUUCDTime=20f;

    void Start () {
        _player = GetComponent<CharacterBehavior>();
        PushT = false;
        CanPushT = true;
        UUUCDTime = 20f;

    }
	
	void Update () {


        if (Input.GetKeyDown(KeyCode.T))
        {
            if (CanPushT == true && _player.gameObject.tag != "Die")
            {
                PushT = true;
                KeyTime += Time.deltaTime;
            }    
        }
        else if(Input.GetKeyUp(KeyCode.T))
        {
            if (PushT == true)
            {
                addTime = false;
                Choose(KeyTime);
                UUUCDTime = 0f;
                KeyTime = 0f;
                PushT = false;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;      
        }
        if (UUUCDTime <= 20f)
        {
            UUUCDTime += Time.deltaTime;
            CanPushT = false;
        }
        else
        {
            CanPushT = true;
        }

    }
    public void Choose(float time)
    {
        if (time <= 1f)
        {
            _time = 1;
            CmdSpawnUUU(_time);
        }
        else if (time <= 3f)
        {
            _time = 2;
            CmdSpawnUUU(_time);

        }
        else if (time > 3f)
        {
            _time = 3;
            CmdSpawnUUU(_time);
        }

    }

    [Command]
    public void CmdSpawnUUU(int i)
    {
        RpcSpawnUUU(i);
    }
    [ClientRpc]
    public void RpcSpawnUUU(int i)
    {
        if (_player._isFacingRight)
        {
            Vector3 pos = gameObject.transform.position + new Vector3(0, 0, 0);
            GameObject UO1right = Instantiate(U01right, pos, Quaternion.identity) as GameObject;
            U01right.GetComponent<rightfly>().Damage *= i;
        }
        else
        {
            Vector3 pos = gameObject.transform.position + new Vector3(0, 0, 0);
            GameObject UO1left = Instantiate(U01left, pos, Quaternion.identity) as GameObject;
            U01left.GetComponent<rightfly>().Damage *= i;
        }
    }

}
