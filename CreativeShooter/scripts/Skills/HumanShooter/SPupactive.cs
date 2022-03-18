using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SPupactive : NetworkBehaviour {

	[SyncVar]
	public bool Spactive;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[Command]
	public void CmdSpactive(bool _isactive)
	{
		RpcSpactive (_isactive);
	}
	[ClientRpc]
	public void RpcSpactive(bool _isactive)
	{
		Spactive = _isactive;
	}
}
