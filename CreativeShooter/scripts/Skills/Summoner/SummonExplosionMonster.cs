using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SummonExplosionMonster : NetworkBehaviour {

	public GameObject explosiveMonster;
	private Vector3 MonsterPoint;
    public bool PushT;
    public float PushTCDTime = 30f;
    public bool CanPushT = true;
    public CharacterBehavior _player;
    public GameObject _Skill;

    void Start () {

        CanPushT = true;
        PushTCDTime = 30f;
        PushT = false;
        _player = gameObject.GetComponent<CharacterBehavior>();

    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKey(KeyCode.T))
        {
            if (CanPushT == true && _player.gameObject.tag != "Die")
            {
                PushTCDTime = 0f;
                PushT = true;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
        }

        if (PushTCDTime <= 30f)
        {
            PushTCDTime += Time.deltaTime;
            CanPushT = false;
        }

        if(PushTCDTime >= 30f)
        {
            CanPushT = true;
        }

        if(PushT == true)
        {
            CmdSpawnExplosion();
            PushT = false;          
        }

	}

    [Command]
    public void CmdSpawnExplosion()
    {
        RpcSpawnExplosion();
    }
    [ClientRpc]
    public void RpcSpawnExplosion()
    {
        if (_player._isFacingRight == true)
        {
            MonsterPoint.x = gameObject.transform.position.x + 2;
            MonsterPoint.y = gameObject.transform.position.y;
            MonsterPoint.z = gameObject.transform.position.z;
            Instantiate(explosiveMonster, MonsterPoint, gameObject.transform.rotation);
        }
        else if (_player._isFacingRight == false)
        {
            MonsterPoint.x = gameObject.transform.position.x - 2;
            MonsterPoint.y = gameObject.transform.position.y;
            MonsterPoint.z = gameObject.transform.position.z;
            Instantiate(explosiveMonster, MonsterPoint, gameObject.transform.rotation);
        }
    }
    
}
