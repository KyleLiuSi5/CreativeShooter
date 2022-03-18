using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Avatar : NetworkBehaviour {

    public bool PushT;
    public bool CanPushT;
    public GameObject Monster;
    private Vector3 Monster1;
    public float AvatarCDTime = 50f;
    public GameObject _Skill;
    CharacterBehavior _Character;

    // Use this for initialization
    void Start () {

        PushT = false;
        CanPushT = true;
        AvatarCDTime = 50f;
        _Character = GetComponent<CharacterBehavior>();

    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKey(KeyCode.T))
        {
            if (CanPushT == true && _Character.gameObject.tag != "Die")
            {
                PushT = true;
                AvatarCDTime = 0f;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;
        }

        if(AvatarCDTime < 50)
        {
            CanPushT = false;
            AvatarCDTime += Time.deltaTime;
        }
        else
        {
            CanPushT = true;
        }

        if (PushT == true)
        {
           CmdAvatarMonster();
           CmdAvatarMonster();
           CmdAvatarMonster();
           CmdAvatarMonster();
           CmdAvatarMonster();

            PushT = false;
        }
	
	}

    [Command]
    public void CmdAvatarMonster()
    {
        RpcAvatarMonster();
    }
    [ClientRpc]
    public void RpcAvatarMonster()
    {
        Monster1.x = Random.Range(gameObject.transform.position.x - 5, gameObject.transform.position.x + 5);
        Monster1.y = gameObject.transform.position.y;
        Monster1.z = gameObject.transform.position.z;
        Instantiate(Monster, Monster1, Quaternion.identity);
    }

}
