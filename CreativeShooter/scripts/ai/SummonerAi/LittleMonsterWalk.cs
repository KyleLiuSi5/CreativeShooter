using UnityEngine;
using System.Collections;

public class LittleMonsterWalk : MonoBehaviour {

    public bool SMdirection = true;
    private CorgiController _controller;
    public float Speed = 3f;
    public float SeeDistance = 10f;
    private bool CharacterFacingRight = true;
    public float StopDistance = 1f;
    private Vector2 SeeDirection;
    private float _distance;
    private Animator _animator;
    private int _facingModifier;

    void Start()
    {


        _controller = GetComponent<CorgiController>();

        _animator = GetComponent<Animator>();

        // SeeDirection = Vector2.right;

        if (CharacterFacingRight)
            _facingModifier = -1;
        else
            _facingModifier = 1;


        SeeDirection = SMdirection ? Vector2.right : -Vector2.right;

    }

    void Update()
    {
        bool hit = false;

        _distance = 0;


        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y + (transform.localScale.y / 2));
        RaycastHit2D raycast = CorgiTools.CorgiRayCast(raycastOrigin, -Vector2.right, SeeDistance, 1 << LayerMask.NameToLayer("Player"), true, Color.gray);


        if (raycast)
        {
            hit = true;
            SeeDirection = -Vector2.right;

            _distance = raycast.distance;

        }

        raycastOrigin = new Vector2(transform.position.x, transform.position.y + (transform.localScale.y / 2));
        raycast = CorgiTools.CorgiRayCast(raycastOrigin, Vector2.right, SeeDistance, 1 << LayerMask.NameToLayer("Player"), true, Color.gray);
        if (raycast)
        {
            hit = true;
            SeeDirection = Vector2.right;
            _distance = raycast.distance;
        }

        if ((hit) && (_distance > StopDistance))
            _controller.SetHorizontalForce(SeeDirection.x * Speed);
        else
        {
            //_controller.SetHorizontalForce(0);

            _controller.SetHorizontalForce(SeeDirection.x * Speed);
            if ((SeeDirection.x < 0 && _controller.State.IsCollidingLeft) || (SeeDirection.x > 0 && _controller.State.IsCollidingRight))
            {
                SeeDirection = -SeeDirection;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

        }

        if (SeeDirection == Vector2.right)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * _facingModifier, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * _facingModifier, transform.localScale.y, transform.localScale.z);



    }
}
