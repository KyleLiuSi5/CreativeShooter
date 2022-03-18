using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WhoWin : NetworkBehaviour {

    public GameObject WinPanel;
    public GameObject Win;
    public GameObject Lose;
    public int TeamValue = 0;
    public string Team;
    public int modeValue = 0;
    public Transform MonsterCarGoal;
    public Transform HumanCarGoal;

	// Use this for initialization
	void Start () {

        if(gameObject.transform.parent.tag == "Player")
        {
            Team = "Human";
            TeamValue = 1;
        }
        else if(gameObject.transform.parent.tag == "Enemy")
        {
            Team = "Monster";
            TeamValue = 2;
        }
        if(GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>().playScene == "MAP1")
        {
            modeValue = 1;
        }
        else if(GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>().playScene == "MAP2")
        {
            modeValue = 2;
            MonsterCarGoal = GameObject.Find("MonsterCarGoal").transform;
        }
        else if(GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>().playScene == "MAP3")
        {
            modeValue = 3;
            HumanCarGoal = GameObject.Find("HumanCarGoal").transform;
        }
	
	}

    // Update is called once per frame
    void Update() {

        if (modeValue == 1)
        {
            if (TeamValue == 1)
            {
                if (GameObject.Find("LevelBounds").GetComponent<LevelLimits>()._WhoWin == 1)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Lose.SetActive(true);
                }
                else if (GameObject.Find("LevelBounds").GetComponent<LevelLimits>()._WhoWin == 2)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Win.SetActive(true);
                }
                else
                    return;
            }
            else if (TeamValue == 2)
            {
                if (GameObject.Find("LevelBounds").GetComponent<LevelLimits>()._WhoWin == 1)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Win.SetActive(true);
                }
                else if (GameObject.Find("LevelBounds").GetComponent<LevelLimits>()._WhoWin == 2)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Lose.SetActive(true);
                }
                else
                    return;
            }
        }
        else if (modeValue == 2)
        {
            if(TeamValue == 1)
            {
                if(GameObject.Find("MonsterCar").transform == MonsterCarGoal)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Lose.SetActive(true);
                }
                else if(gameObject.GetComponentInChildren<TimeCountDown>().totaltime == 0)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Win.SetActive(true);
                }
            }
            else if(TeamValue == 2)
            {
                if (GameObject.Find("MonsterCar").transform == MonsterCarGoal)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Win.SetActive(true);
                }
                else if (gameObject.GetComponentInChildren<TimeCountDown>().totaltime == 0)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Lose.SetActive(true);
                }
            }
        }
        else if (modeValue == 3)
        {
            if (TeamValue == 1)
            {
                if (GameObject.Find("HumanCar").transform == HumanCarGoal)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Win.SetActive(true);
                }
                else if (gameObject.GetComponentInChildren<TimeCountDown>().totaltime == 0)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Lose.SetActive(true);
                }
            }
            else if (TeamValue == 2)
            {
                if (GameObject.Find("HumanCar").transform == HumanCarGoal)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Lose.SetActive(true);
                }
                else if (gameObject.GetComponentInChildren<TimeCountDown>().totaltime == 0)
                {
                    Time.timeScale = 0f;
                    WinPanel.SetActive(true);
                    Win.SetActive(true);
                }
            }
        }

    }
}
