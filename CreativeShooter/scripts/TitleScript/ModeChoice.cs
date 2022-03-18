using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ModeChoice : NetworkBehaviour {

    public GameObject[] ModeIntro;
    public Dropdown _drop;

    [SyncVar]
    public int NowMode = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeValue()
    {
        if (_drop.value == 0)
        {
            ModeIntro[0].SetActive(false);
            ModeIntro[1].SetActive(false);
            ModeIntro[2].SetActive(false);
            NowMode = 0;
        }
        else if(_drop.value == 1)
        {
            ModeIntro[0].SetActive(true);
            ModeIntro[1].SetActive(false);
            ModeIntro[2].SetActive(false);
            NowMode = 1;
        }
        else if(_drop.value == 2)
        {
            ModeIntro[0].SetActive(false);
            ModeIntro[1].SetActive(true);
            ModeIntro[2].SetActive(false);
            NowMode = 2;
        }
        else if(_drop.value == 3)
        {
            ModeIntro[0].SetActive(false);
            ModeIntro[1].SetActive(false);
            ModeIntro[2].SetActive(true);
            NowMode = 3;
        }
    }

}
