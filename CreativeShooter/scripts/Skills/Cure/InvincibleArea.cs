using UnityEngine;
using System.Collections;

public class InvincibleArea : MonoBehaviour {


	BigSkill _bigskill;
	// Use this for initialization
	void Start () {
		_bigskill = GetComponentInParent<BigSkill> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{

		if (collider.gameObject.tag == "Player") {
			_bigskill.CmdBigSkill(collider.gameObject);


		}
	}
}
