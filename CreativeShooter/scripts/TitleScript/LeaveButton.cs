using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LeaveButton : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LeaveGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
