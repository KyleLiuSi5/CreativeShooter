using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Buffactive : NetworkBehaviour {

	[SyncVar]
	public bool isactive;
	
	// Update is called once per frame

	[Command]
	public void CmdactiveTrue(bool active)
	{
		RpcactiveTrue (active);
	}
	[ClientRpc] 
	public void RpcactiveTrue(bool active)
	{
		isactive = active;
	}
}
