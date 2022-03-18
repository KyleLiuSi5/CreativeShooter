using UnityEngine;
using System.Collections;

public class AvatarAI : MonoBehaviour {

	public bool AgentFollowsPlayer { get; set; }
    public float RunDistance = 10f;
    public float WalkDistance = 5f;
    public float StopDistance = 1f;
    public float JetpackDistance = 0.2f;
    private float ExistTime = 0f;
    public Vector2 _target;
    private float x;
    private float y;
    private Transform _playerPosition;
    private CharacterBehavior _playerComponent;
    private CorgiController _controller;
    private CharacterJetpack _jetpack;
    private LevelLimits _bounds;
    private float _speed;
    private float _direction;

  
    void Start()
    {
        //尋找範圍內的隨機點,定位並且朝著目標前進
        ExistTime = 0f;
        _bounds = GameObject.FindGameObjectWithTag("LevelBounds").GetComponent<LevelLimits>();

                x = Random.Range(_bounds.LeftLimit, _bounds.RightLimit);
                y = Random.Range(_bounds.BottomLimit, _bounds.TopLimit);

            _target.x = x;
            _target.y = y;            
        _playerComponent = (CharacterBehavior)GetComponent<CharacterBehavior>();
        _controller = (CorgiController)GetComponent<CorgiController>();
        _jetpack = (CharacterJetpack)GetComponent<CharacterJetpack>();
        AgentFollowsPlayer = true;
    }

    void Update()
    {
        ExistTime += Time.deltaTime;
            if (!AgentFollowsPlayer)
                return;
            if ((_playerComponent == null) || (_controller == null))
                return;

            float distance = Mathf.Abs(_target.x - transform.position.x);

            _direction = _target.x > transform.position.x ? 1f : -1f;

            if (distance > RunDistance)
            {
                _speed = 1;
                _playerComponent.RunStart();
            }
            else
            {
                _playerComponent.RunStop();
            }
            if (distance < RunDistance && distance > WalkDistance)
            {
                // walk
                _speed = 1;
            }
            if (distance < WalkDistance && distance > StopDistance)
            {
                // walk slowly
                _speed = distance / WalkDistance;
            }
            if (distance < StopDistance)
            {
                // stop
                _speed = 0f;
            }

            _playerComponent.SetHorizontalMove(_speed * _direction);

            if (_controller.State.IsCollidingRight || _controller.State.IsCollidingLeft)
            {
                _playerComponent.JumpStart();
            }

            if (_jetpack != null)
            {
                if (_target.y > transform.position.y + JetpackDistance)
                {
                    _jetpack.CmdJetpackStart();
                }
                else
                {
                    _jetpack.CmdJetpackStop();
                }
            }
        if (ExistTime >= 3)
        {
            Destroy(gameObject);
        }
        
    }
}
