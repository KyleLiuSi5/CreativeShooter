using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class PushStart : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        Time.timeScale = 1f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PushStartButton()
    {
        SceneManager.LoadScene(1);
    }

}
