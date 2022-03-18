using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour {

    public bool PushE;
    public bool CanPushE;
    public float BombCDTime = 40f;
    private Vector3 FirePos;
    public GameObject _Rocketpref;
    public Camera _camera;
    playerScript _player;
    CharacterBehavior _Character;
    public GameObject _Skill;

    // Use this for initialization
    void Start () {

        PushE = false;
        CanPushE = true;
        BombCDTime = 40f;
        _player = GetComponent<playerScript>();
        _Character = GetComponent<CharacterBehavior>();
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKey(KeyCode.E))
        {
            if(CanPushE == true && _Character.gameObject.tag != "Die")
            {
                PushE = true;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
                BombCDTime = 0f;
            }
            else
            {
                return;
            }
        }

        if(BombCDTime <= 40)
        {
            BombCDTime += Time.deltaTime;
            CanPushE = false;
        }
        else
        {
            CanPushE = true;
        }

        if(PushE == true)
        {
            Vector3 MousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            FirePos.x = MousePos.x;
            FirePos.y = MousePos.y + 10;
            FirePos.z = 0;
            CmdSpawnRocket(FirePos);
        }

	}

    [Command]
    public void CmdSpawnRocket(Vector3 _FirePos)
    {
        RpcSpawnRocket(_FirePos);
    }
    [ClientRpc]
    public void RpcSpawnRocket(Vector3 _FirePos)
    {
        
        Instantiate(_Rocketpref, _FirePos, Quaternion.identity);
        PushE = false;
    }

}
