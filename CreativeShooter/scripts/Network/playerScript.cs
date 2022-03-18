using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class playerScript : NetworkBehaviour {

    [SerializeField]
    Behaviour[] ComponentsToClose;
    public Camera _camera;
    public Camera _UICamera;

    Camera SceneCamera;
    [SyncVar]
    public float calltime = 0.5f;
    [SyncVar]
    public int Switchi;
    [SyncVar]
    public bool CanCallAgain = true;
    [SyncVar]
    public int Switchj;
    [SyncVar]
    public Vector3 FirePos;
    [SyncVar]
    public int SwitchT;
    [SyncVar]
    public float RRRRRTime;
    [SyncVar]
    public int SwitchC;
    [SyncVar]
    public int SwitchW;
    [SyncVar]
    public float TankTime;
    [SyncVar]
    public int SwichC876;

    public bool TankisActive;

    void Start()
    {
        if (isLocalPlayer)
        {
            gameObject.name = "ME";
            SceneCamera = Camera.main;
            
            if (SceneCamera != null)
                SceneCamera.gameObject.SetActive(false);
        }
       
        if(!isLocalPlayer)
        {
            for(int i = 0; i < ComponentsToClose.Length;i++)
            {
                ComponentsToClose[i].enabled = false;
            }
            _camera.gameObject.SetActive(false);
            _UICamera.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (calltime < 0.2f)
        {
            calltime += Time.deltaTime;
            CanCallAgain = false;
        }
        else
            CanCallAgain = true;

        if(Switchj == 1)
        {
            RRRRRTime += Time.deltaTime;
        }
        if(RRRRRTime > 5f)
        {
            Switchj = 0;
            RRRRRTime = 0;
        }
        if(SwitchT == 1)
        {
            TankTime += Time.deltaTime;
        }
        if(TankTime > 20f)
        {
            SwitchT = 0;
            TankTime = 0;
        }
       


        if (GameObject.Find("Commander(Clone)") != null)
        {
            if (GameObject.Find("Commander(Clone)").GetComponent<TankActive>().isactive == true)
            {
                GameObject.Find("TankIdle").GetComponent<SpriteRenderer>().enabled = true;
                GameObject.Find("TankIdle").GetComponent<PolygonCollider2D>().enabled = true;
            }
            else if (GameObject.Find("Commander(Clone)").GetComponent<TankActive>().isactive == false)
            {
                GameObject.Find("TankIdle").GetComponent<SpriteRenderer>().enabled = false;
                GameObject.Find("TankIdle").GetComponent<PolygonCollider2D>().enabled = false;
            }
        }
        if(GameObject.Find("Charge(Clone)") != null)
        {
            if(GameObject.Find("Charge(Clone)").GetComponent<C876isActive>().C8763isActive == true)
            {
                GameObject.Find("C8763").GetComponent<Collider2D>().enabled = true;
            }
            else if(GameObject.Find("Charge(Clone)").GetComponent<C876isActive>().C8763isActive == false)
            {
                GameObject.Find("C8763").GetComponent<Collider2D>().enabled = false;
            }
            if (GameObject.Find("Charge(Clone)").GetComponent<C876isActive>().ShieldisActive == true)
            {
                GameObject.Find("ShieldAttack").GetComponent<Collider2D>().enabled = true;
            }
            else if (GameObject.Find("Charge(Clone)").GetComponent<C876isActive>().ShieldisActive == false)
            {
                GameObject.Find("ShieldAttack").GetComponent<Collider2D>().enabled = false;
            }
            if (GameObject.Find("Charge(Clone)").GetComponent<ShieldUpactive>().shieldactive == true)
            {
                GameObject.Find("shield_effect").GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (GameObject.Find("Charge(Clone)").GetComponent<ShieldUpactive>().shieldactive == false)
            {
                GameObject.Find("shield_effect").GetComponent<SpriteRenderer>().enabled = false;
            }

            if (GameObject.Find("Charge(Clone)").GetComponent<ShieldATKActive>().ShieldATKactive == true)
            {
                GameObject.Find("impactEffect").GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (GameObject.Find("Charge(Clone)").GetComponent<ShieldATKActive>().ShieldATKactive == false)
            {
                GameObject.Find("impactEffect").GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        if (GameObject.Find("Curer(Clone)") != null)
        {
            if (GameObject.Find("Curer(Clone)").GetComponent<Buffactive>().isactive == true)
            {
                GameObject.Find("BuffRegion").GetComponent<CircleCollider2D>().enabled = true;
            }
            else if (GameObject.Find("Curer(Clone)").GetComponent<Buffactive>().isactive == false)
            {
                GameObject.Find("BuffRegion").GetComponent<CircleCollider2D>().enabled = false;
            }

            if (GameObject.Find("Curer(Clone)").GetComponent<CureAreaActive>().isCureactive == true)
            {
                GameObject.Find("CureArea").GetComponent<CircleCollider2D>().enabled = true;
            }
            else if (GameObject.Find("Curer(Clone)").GetComponent<CureAreaActive>().isCureactive == false)
            {
                GameObject.Find("CureArea").GetComponent<CircleCollider2D>().enabled = false;
            }

            if (GameObject.Find("Curer(Clone)").GetComponent<Unrivalactive>().unrival == true)
            {
                GameObject.Find("InvincibleBlock").GetComponent<CircleCollider2D>().enabled = true;
            }
            else if (GameObject.Find("Curer(Clone)").GetComponent<Unrivalactive>().unrival == false)
            {
                GameObject.Find("InvincibleBlock").GetComponent<CircleCollider2D>().enabled = false;
            }


        }

        if(GameObject.Find("artilleryman(Clone)") != null)
        {
            if(GameObject.Find("artilleryman(Clone)").GetComponent<SPupactive>().Spactive == true)
            {
                GameObject.Find("Spupeffect").GetComponent<SpriteRenderer>().enabled = true;
            }
            else if(GameObject.Find("artilleryman(Clone)").GetComponent<SPupactive>().Spactive == false)
            {
                GameObject.Find("Spupeffect").GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        if (GameObject.Find("Tanker(Clone)") != null)
        {
            if (GameObject.Find("Tanker(Clone)").GetComponent<RRRRactive>().rrrrAcite == true)
            {
                GameObject.Find("atkspeedupeffect").GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (GameObject.Find("Tanker(Clone)").GetComponent<RRRRactive>().rrrrAcite == false)
            {
                GameObject.Find("atkspeedupeffect").GetComponent<SpriteRenderer>().enabled = false;
            }
        }

    }
       
    void OnDisable()
    {
        if(SceneCamera != null)
        {
            SceneCamera.gameObject.SetActive(true);
        }
    }

}
