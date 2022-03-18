using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    public Transform target;
    public Vector3[] path;
    float Speed;
    public int targetIndex;
    private float _direction;
    private CharacterBehavior _player;
    private CharacterJetpack _jetpack;
    public float RunDistance = 10f;
    public float WalkDistance = 5f;
    public float StopDistance = 0.1f;
    public float JetpackDistance = 0.3f;
    private CorgiController _controller;

    void Start()
    {
        _player = GetComponentInChildren<CharacterBehavior>();
        _jetpack = GetComponentInChildren<CharacterJetpack>();
        _controller = GetComponentInChildren<CorgiController>();
    }

    void Update()
    {

    }

    public void FindRoad()
    {
        RequestPathManager.RequestPath(transform.position, target.position, OnPathFound);       
    }


    public void OnPathFound(Vector3[] newPath , bool pathSuccessful)
    {
            if (pathSuccessful)
            {
                path = newPath;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        
    }

    IEnumerator FollowPath()
    {
        
        Vector3 currentWayPoint = path[0];
        if (currentWayPoint == target.transform.position)
            yield break;
        while(true)
        {
            //Mathf.Abs(transform.position.x - currentWayPoint.x) <= 0.2 && Mathf.Abs(transform.position.y - currentWayPoint.y) <= 0.3
            if (transform.position == currentWayPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWayPoint = path[targetIndex];
            }
            float distance = Mathf.Abs(currentWayPoint.x - transform.position.x) + 2;
            _direction = currentWayPoint.x > transform.position.x ? 1f : -1f;
            if (distance > RunDistance)
            {
                Speed = 6;
                _player.RunStart();
            }
            else
            {
                _player.RunStop();
            }
            if (distance < RunDistance && distance > WalkDistance)
            {
                Speed = 3;
            }
            if (distance < WalkDistance && distance > StopDistance)
            {
                Speed = 3;
            }
            if (distance < StopDistance)
            {
                Speed = 0f;
            }

            if (_controller.State.IsCollidingLeft)
            {
                transform.Translate(Vector3.right*0.5f);

            }
            if (_controller.State.IsCollidingAbove)
            {
                transform.Translate(Vector3.down * 0.5f);
                transform.Translate(Vector3.right * 0.5f);
            }
            if (_controller.State.IsCollidingRight)
            {
                transform.Translate(Vector3.left * 0.5f);
            }

            if (_jetpack != null)
            {
                if (currentWayPoint.y > transform.position.y + JetpackDistance)
                {
                    _jetpack.CmdJetpackStart();
                    Speed = 2;
                }
                else
                {
                    _jetpack.CmdJetpackStop();
                }
            }

            if (currentWayPoint.x < transform.position.x)
            {
                if (_player._isFacingRight == true)
                    _player.CmdFlip();
            }
                
            else if (currentWayPoint.x > transform.position.x)
            {
                if (_player._isFacingRight == false)
                    _player.CmdFlip();
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, Speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

}
