using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// Handles all GUI effects and changes
/// </summary>
public class GUIManager : MonoBehaviour 
{

	public GameObject HUD;
	public GameObject JetPackBar;


	public void Start()
	{
				
	}
	public void SetHUDActive(bool state)
	{
		HUD.SetActive(state);
	}

	public void SetJetpackBar(bool state)
	{
		JetPackBar.SetActive(state);
	}


}
