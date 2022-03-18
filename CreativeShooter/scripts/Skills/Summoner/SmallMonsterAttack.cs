using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SmallMonsterAttack : NetworkBehaviour {

    public void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            CmdState(collider.gameObject, true);
        }

    }
    public void OnTriggerExit2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            CmdState(collider.gameObject, false);
        }

    }

    [Command]
    public void CmdState(GameObject _player, bool _true)
    {
        RpcState(_player, _true);
    }
    [ClientRpc]
    public void RpcState(GameObject _player, bool _true)
    {
        _player.GetComponent<CharacterBehavior>().inbigmonster = _true;
    }
}
