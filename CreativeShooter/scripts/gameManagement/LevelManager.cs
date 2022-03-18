using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Networking;


public class LevelManager : NetworkBehaviour
{

	/*public static LevelManager Instance { get; private set; }		
	[Header("Prefabs")]
	public CharacterBehavior PlayerPrefab ;
	public CheckPoint DebugSpawn;
	public TimeSpan RunningTime { get { return DateTime.UtcNow - _started ;}}
	
	[Space(10)]
	[Header("Intro and Outro durations")]
	public float IntroFadeDuration=1f;
	public float OutroFadeDuration=1f;

	public CharacterBehavior _player;
    private GameObject _DiePlayer;
    private CharacterBehavior _Die;
	private CheckPoint _checkpoint;
	private DateTime _started;
	private int _savedPoints;	
	private CameraController _cameraController ;
    public AILerp[] _AI;

	public void Awake()
	{
		Instance=this;
        _player = GameObject.Find("ME").GetComponent<CharacterBehavior>();
        if(_player.GetComponent<playerScript>().isLocalPlayer == true)
        {
            Camera.main.gameObject.SetActive(false);
        }
        GameObject.Find("GameManagers").GetComponent<GameManager>().Player = _player;
        _AI = GameObject.FindObjectsOfType<AILerp>();
    }
	

	public void Update()
	{
		_savedPoints = GameObject.Find("GameManagers").GetComponent<GameManager>().Points;
		_started = DateTime.UtcNow;
        
    }



	public void KillPlayer()
	{
        _DiePlayer = GameObject.FindGameObjectWithTag("Die");
        _Die = _DiePlayer.GetComponent<CharacterBehavior>();
        _Die.CurrentHealth = 0;
        StartCoroutine(KillPlayerCo());
	}


	private IEnumerator KillPlayerCo()
	{
		_Die.Kill();
        
		yield return new WaitForSeconds(2f);
        if (_Die.gameObject.layer == 9)
            _checkpoint = GameObject.Find("HumanRespawnPos").GetComponent<CheckPoint>();
        else if (_Die.gameObject.layer == 13)
            _checkpoint = GameObject.Find("MonsterRespawnPos").GetComponent<CheckPoint>();
        _checkpoint.SpawnPlayer(_Die);
        _Die.CurrentHealth = _Die.BehaviorParameters.MaxHealth;

        _started = DateTime.UtcNow;
        GameObject.Find("GameManagers").GetComponent<GameManager>().SetPoints(_savedPoints);
	}*/
}

