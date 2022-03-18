using UnityEngine;
using System.Collections;

public class stonestate : MonoBehaviour {

    private GameObject _player;
    private Vector2 FinalTranform;
    private Vector2 _playerTransform;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, 10);
    }

}
