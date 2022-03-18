using UnityEngine;
using System.Collections;
/// <summary>
/// Add this to an item to make it modify time when it gets picked up by a Character
/// </summary>
public class TimeModifier : MonoBehaviour, IPlayerRespawnListener
{

	public void onPlayerRespawnInThisCheckpoint(CheckPoint checkpoint, CharacterBehavior character)
	{
		gameObject.SetActive(true);
	}
}
