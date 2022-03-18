using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SummonSmallMonster : NetworkBehaviour {

	public GameObject smallMonster;
	private Vector3 MonsterPoint;
    public float PushECDTime = 10f;
    public bool CanPushE = true;
    public bool PushE;
    public GameObject _Skill;

    void Start () {

        PushECDTime = 10f;
        CanPushE = true;
        PushE = false;

    }
	

	void Update () {


        if (Input.GetKey(KeyCode.E))
        {
            if (CanPushE == true && gameObject.tag != "Die")
            {
                PushE = true;
                PushECDTime = 0f;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
        }

        if (PushECDTime <= 10f)
        {
            CanPushE = false;
            PushECDTime += Time.deltaTime;
        }
        if(PushECDTime >= 10f)
        {
            CanPushE = true;
        }

        if(PushE == true)
        {
            CmdSmallMonster();
            CmdSmallMonster();
            CmdSmallMonster();
            PushE = false;
        }
		
	}
    [Command]
    public void CmdSmallMonster()
    {
        RpcSmallMonster();
    }
    [ClientRpc]
    public void RpcSmallMonster()
    {
        MonsterPoint.x = Random.Range(gameObject.transform.position.x - 5, gameObject.transform.position.x + 5);
        MonsterPoint.y = gameObject.transform.position.y;
        MonsterPoint.z = gameObject.transform.position.z;
        Instantiate(smallMonster, MonsterPoint, Quaternion.identity);
    }

}
