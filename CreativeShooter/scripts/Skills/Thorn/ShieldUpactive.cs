using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShieldUpactive : NetworkBehaviour {


	[SyncVar]
	public bool shieldactive;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	[Command]
	public void CmdshieldupActive(bool _isactive)
	{
		RpcshieldupActive (_isactive);
	}
	[ClientRpc]
	public void RpcshieldupActive(bool _isactive)
	{
		shieldactive = _isactive;
	}

}
