using UnityEngine;
using System.Collections;

public class Destroyitself : MonoBehaviour {

    public float _existTime = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        _existTime += Time.deltaTime;

        if(_existTime >= 10f)
        {
            Destroy(gameObject);
        }
	
	}
}
