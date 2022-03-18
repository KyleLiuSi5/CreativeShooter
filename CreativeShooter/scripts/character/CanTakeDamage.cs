using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// Public interface for objects that can take damage
/// </summary>

public interface CanTakeDamage
{
    [Command]
	void CmdTakeDamage(int damage,GameObject instigator);
}
