using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BuffRegion : NetworkBehaviour {


	Buff _buff;



	// Use this for initialization
	void Start () {
		_buff = GetComponentInParent<Buff> ();

    }
	
	// Update is called once per frame
	void Update () {
		

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        
		if (collider.gameObject.tag == "Player") {
			_buff.CmdBuffconfirm (collider.gameObject);


		} else 
		{
			
		}
    }
}
