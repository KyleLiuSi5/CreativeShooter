using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class Unrivalactive : NetworkBehaviour {

	[SyncVar]
	public bool unrival ;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[Command]
	public void CmdUnirivalActive(bool _isactive)
	{
		RpcUnrivalActive (_isactive);
	}
	[ClientRpc]
	public void RpcUnrivalActive(bool _isactive)
	{
		unrival = _isactive;
	}
}
