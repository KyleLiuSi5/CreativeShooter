using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RRRRactive : NetworkBehaviour {


	[SyncVar]
	public bool rrrrAcite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[Command]
	public void CmdSetRRRactive(bool _isactive)
	{
		RpcSetRRRactive (_isactive);
	}
	[ClientRpc]
	public void RpcSetRRRactive(bool _isactive)
	{
		rrrrAcite = _isactive;
	}
}
