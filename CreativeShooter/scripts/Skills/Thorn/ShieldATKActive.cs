using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShieldATKActive : NetworkBehaviour {

	[SyncVar]
	public bool ShieldATKactive;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[Command]
	public void CmdsetShieldATK(bool _active)
	{
		RpcsetShieldATK (_active);
	}
	[ClientRpc]
	public void RpcsetShieldATK(bool _active)
	{
		ShieldATKactive = _active;
	}
}
