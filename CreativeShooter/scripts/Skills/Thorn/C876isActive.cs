using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class C876isActive : NetworkBehaviour {

    [SyncVar]
    public bool C8763isActive = false;
    [SyncVar]
    public bool ShieldisActive = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        
	}

    [Command]
    public void CmdSetC8763active(bool a)
    {
        RpcSetC8763active(a);
    }
    [ClientRpc]
    public void RpcSetC8763active(bool a)
    {
        C8763isActive = a;
    }
    [Command]    
    public void CmdSetShieldactive(bool b)
    {
        RpcSetShieldactive(b);
    }
    [ClientRpc]
    public void RpcSetShieldactive(bool b)
    {
        ShieldisActive = b;
    }

}
