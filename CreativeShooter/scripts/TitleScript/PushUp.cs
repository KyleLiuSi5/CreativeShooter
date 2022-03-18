using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PushUp : MonoBehaviour {

    private bool Up;
    public GameObject[] _Button;
    private int i;
    private int j;

	// Use this for initialization
	void Start () {
        i = _Button.Length;
        j = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (j == 0)
            {
                _Button[j].SetActive(false);
                _Button[i - 1].SetActive(true);
                j = i - 1;
            }
            else if (j > 0)
            {
                _Button[j].SetActive(false);
                _Button[j - 1].SetActive(true);
                j--;
            }
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (j == i - 1)
            {
                _Button[j].SetActive(false);
                _Button[0].SetActive(true);
                j = 0;
            }
            else if (j < i)
            {
                _Button[j].SetActive(false);
                _Button[j + 1].SetActive(true);
                j++;
            }
        }
	}

    public void PushButtonUp()
    {
        if (j == 0)
        {
            _Button[j].SetActive(false);
            _Button[i-1].SetActive(true);
            j = i-1;
        }
        else if (j > 0)
        {
            _Button[j].SetActive(false);
            _Button[j - 1].SetActive(true);
            j--;
        }
    }

    public void PushButtonDown()
    {
        if (j == i-1)
        {
            _Button[j].SetActive(false);
            _Button[0].SetActive(true);
            j = 0;
        }
        else if (j < i)
        {
            _Button[j].SetActive(false);
            _Button[j + 1].SetActive(true);
            j++;
        }
    }
}
