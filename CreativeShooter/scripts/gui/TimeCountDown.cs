using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TimeCountDown : NetworkBehaviour {


    public int totaltime;
    // Use this for initialization
    void Start () {

        if(GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>().playScene == "MAP1")
        {
            gameObject.SetActive(false);
        }
        else if(GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>().playScene == "MAP2")
        {
            totaltime = 300;
            StartCoroutine(CountDownTime());
        }
        else if(GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>().playScene == "MAP3")
        {
            totaltime = 330;
            StartCoroutine(CountDownTime());
        }
        
    }
	
	// Update is called once per frame
	void Update () {

            int min = totaltime / 60;
            int sec = totaltime % 60;
            GetComponent<Text>().text = (min < 10 ? "0" + min : min + "") + ":" + (sec < 10 ? "0" + sec : sec + "");
        
    }
    IEnumerator CountDownTime()
    {
        while (true)
        {

            yield return new WaitForSeconds(1);
            totaltime--;
            if (totaltime == 0)
                break;
        }
    }

}
