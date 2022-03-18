using UnityEngine;
using System.Collections;

public class ExplosionBlob : MonoBehaviour {

    public float Speed = 1f;
    private CorgiController _controller;
    private Animator _animator;
    public GameObject[] _player;
    public GameObject _target;
    public int i = 0;
    public int h = 0;
    public bool Repeat;
    public float Distance = 0f;
    public float temp = 0f;
    public float RepeatTime = 2f;
    public float existTime = 0f;
    public ParticleSystem ExplosionAni;
    public AudioClip BlobExplosionSfx;


    void Start()
    {
        _controller = GetComponent<CorgiController>();
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectsWithTag("Player");
        i = _player.Length;
        Repeat = true;
        RepeatTime = 2f;
        existTime = 0f;

    }

    void Update()
    {
        existTime += Time.deltaTime;
        if (Repeat == true)
        {
            HowMany();
        }
        if (RepeatTime <= 2f)
        {
            Repeat = false;
            RepeatTime += Time.deltaTime;
        }
        else
        {
            Repeat = true;
        }

        if (_animator != null)
            _animator.SetFloat("Speed", Mathf.Abs(_controller.Speed.x));
        if (_target == null)
        {
            if (existTime >= 3)
            {
                Destroy(gameObject);
                Instantiate(ExplosionAni, gameObject.transform.position, Quaternion.identity);
                existTime = 0;
            }
        }
        else
        {
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, _target.transform.position, Speed * Time.deltaTime);
            if (existTime >= 5)
            {
                Destroy(gameObject);
                Instantiate(ExplosionAni, gameObject.transform.position, Quaternion.identity);
                existTime = 0;
            }
        }


    }

    public void HowMany()
    {
        for (int j = 0; j < i; j++)
        {
            if (_player[j].transform.position.x < gameObject.transform.position.x + 20 && _player[j].transform.position.x > gameObject.transform.position.x - 20 && _player[j].transform.position.y < gameObject.transform.position.y + 20 && _player[j].transform.position.y > gameObject.transform.position.y - 20)
            {
                Distance = Mathf.Sqrt(Mathf.Pow(_player[j].transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(_player[j].transform.position.y - gameObject.transform.position.y, 2));
                if (temp == 0)
                {
                    temp = Distance;
                    _target = _player[j];
                    Distance = 0f;
                }
                else if (temp != 0)
                {
                    if (temp > Distance)
                    {
                        temp = Distance;
                        _target = _player[j];
                        Distance = 0f;
                    }
                }
            }
            else
            {
                return;
            }
        }
        RepeatTime = 0f;


    }
}
