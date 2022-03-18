using UnityEngine;
using System.Collections;

public class TankBehavior : MonoBehaviour {
    Animator anim;
    private Tank _tank;
    public CharacterBehavior _characterBehavior { get;private set; }
    public CorgiController _controller { get; private set; }
    private bool Walking;
    private bool Jump;
    private bool idle;
    private Vector3 ProjectilePosition;
    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();
        _tank = gameObject.GetComponentInParent<Tank>();
        _characterBehavior = gameObject.GetComponentInParent<CharacterBehavior>();
        _controller = gameObject.GetComponentInParent<CorgiController>();
        Walking = false;
        Jump = false;
        idle = true;
        

	}
	
	// Update is called once per frame
	void Update () {

        

        if (_tank.PushR == true)
        {
            gameObject.SetActive(true);
           
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                anim.SetFloat("Speed", 8);
                Walking = true;
            }
            else
            {
                anim.SetFloat("Speed", 0);
                Walking = false;

            }
            if (Walking == false && _controller.State.IsCollidingBelow)
            {
                idle = true;
            }

        }


	
	}

  
}
