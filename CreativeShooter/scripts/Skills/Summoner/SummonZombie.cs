using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SummonZombie : NetworkBehaviour {


	public GameObject bigZombie;
	private Vector3 ZombiePoint;
    public float PushRCDTime = 40f;
    public bool CanPushR = true;
    public bool PushR;
    public CharacterBehavior _player;
    public GameObject _Skill;

    // Use this for initialization
    void Start () {

        CanPushR = true;
        PushRCDTime = 40f;
        PushR = false;
        _player = gameObject.GetComponent<CharacterBehavior>();

    }

	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.R))
        {
            if (CanPushR == true && gameObject.tag != "Die")
            {
                PushRCDTime = 0f;
                PushR = true;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
        }

        if (PushRCDTime <= 40f)
        {
            CanPushR = false;
            PushRCDTime += Time.deltaTime;
        }
        if(PushRCDTime >= 40f)
        {
            CanPushR = true;
        }

        if(PushR == true)
        {
            CmdSpawnZombie();
            PushR = false;
        }
	}

    [Command]
    public void CmdSpawnZombie()
    {
        RpcSpawnZombie();
    }
    [ClientRpc]
    public void RpcSpawnZombie()
    {
        if (_player._isFacingRight == true)
        {
            ZombiePoint.x = gameObject.transform.position.x + 2;
            ZombiePoint.y = gameObject.transform.position.y;
            ZombiePoint.z = gameObject.transform.position.z;
            Instantiate(bigZombie, ZombiePoint, gameObject.transform.rotation);
        }
        else if (_player._isFacingRight == false)
        {
            ZombiePoint.x = gameObject.transform.position.x - 2;
            ZombiePoint.y = gameObject.transform.position.y;
            ZombiePoint.z = gameObject.transform.position.z;
            Instantiate(bigZombie, ZombiePoint, gameObject.transform.rotation);
        }
    }

}

