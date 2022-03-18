using UnityEngine;
using System.Collections;

public class CureArea : MonoBehaviour {

	HealthCure _HealthCure;

	// Use this for initialization
	void Start () {
		_HealthCure = GetComponentInParent<HealthCure> ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player") 
		{
			_HealthCure.Cmdhealconfirm (collider.gameObject);
		}
        else
        {

        }

	}
}
