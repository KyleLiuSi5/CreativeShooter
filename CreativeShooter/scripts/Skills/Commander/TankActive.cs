using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TankActive : NetworkBehaviour {

    [SyncVar]
    public bool isactive;

    void Update()
    {

    }

    [Command]
    public void CmdactiveTrue(bool a)
    {
        RpcactiveTrue(a);
    }
    [ClientRpc]
    public void RpcactiveTrue(bool a)
    {
        isactive = a;
    }
}
