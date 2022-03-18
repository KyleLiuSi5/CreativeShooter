using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class dudu : NetworkBehaviour {

    public bool PushE;
    public bool CanPushE;
    public float duduCDTime = 20f;
    private float duduTime = 0f;
    private CharacterBehavior _CB;
    public GameObject _Skill;

    void Start()
    {
        _CB = gameObject.GetComponent<CharacterBehavior>();
        duduCDTime = 20f;
        CanPushE = true;
        PushE = false;

    }

    void Update()
    {

        if (Input.GetKey(KeyCode.E))
        {
            if (CanPushE == true && _CB.gameObject.tag != "Die")
            {
                PushE = true;
                duduCDTime = 0;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;
        }
        if (duduCDTime <= 20)
        {
            duduCDTime += Time.deltaTime;
            CanPushE = false;
        }
        else
        {
            CanPushE = true;
        }
        if (PushE == true)
        {
            duduTime += Time.deltaTime;
            if (_CB._isFacingRight == true)
            {
                gameObject.transform.Translate(Vector2.right * 25 * Time.deltaTime);
            }
            else if (_CB._isFacingRight == false)
            {
                gameObject.transform.Translate(-Vector2.left * 25 * Time.deltaTime);
            }
        }
        if (duduTime > 0.3f)
        {
            PushE = false;

            duduTime = 0f;
        }

    }
    void OnTriggerEnter2D(Collider2D coll)
    {

        if (PushE == true)
        {
            if (coll.gameObject.tag == "Player")
            {
                if (_CB._isFacingRight)
                {
                    CmdGoBack(coll.gameObject);
                }
                else if (_CB._isFacingRight == false)
                {
                    CmdGoBack(coll.gameObject);
                }

            }
        }
        else
            return;
    }

    [Command]
    public void CmdGoBack(GameObject collider)
    {
        RpcGoBack(collider);
    }
    [ClientRpc]
    public void RpcGoBack(GameObject collider)
    {
        collider.transform.Translate(Vector2.up * 5);
    }

}
