using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CureAreaActive : NetworkBehaviour {

	[SyncVar]
	public bool	isCureactive;
	
	[Command]
	public void CmdCureAreaactiveTrue(bool active)
	{
		RpcCureAreaactiveTrue (active);
	}
	[ClientRpc] 
	public void RpcCureAreaactiveTrue(bool active)
	{
		isCureactive = active;
	}
}
