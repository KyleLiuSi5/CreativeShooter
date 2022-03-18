using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

    public bool PushE;
    public bool CanPushE;
    public float TeleportCDTime = 60f;
    private DiamondAI _useSkill;
    public Camera _Camera;
    public GameObject _Skill;
    CharacterBehavior _Character;

    // Use this for initialization
    void Start () {

        TeleportCDTime = 60f;
        CanPushE = true;
        PushE = false;
        _useSkill = gameObject.GetComponent<DiamondAI>();
        _Character = GetComponent<CharacterBehavior>();

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKey(KeyCode.E))
        {
            if (CanPushE == true && _Character.gameObject.tag != "Die")
            {
                PushE = true;
                TeleportCDTime = 0f;
                _Skill.GetComponent<SkillCoolDown>().currentCoolDown = 0;
            }
            else
                return;
        }
        if(PushE == true)
        {
            Vector3 MousePos = _Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            transform.position = MousePos;
            PushE = false;
        }

        if (TeleportCDTime < 60f)
        {
            TeleportCDTime += Time.deltaTime;
            CanPushE = false;
        }
        else
        {
            CanPushE = true;
        }
        if(_useSkill != null)
        {
            if (_useSkill.useTeleport == true)
            {
                TeleportCDTime = 0f;
            }
        }       
	}
}
