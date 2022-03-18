using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
/// <summary>
/// This class will pilot the CorgiController component of your character.
/// This is where you'll implement all of your character's game rules, like jump, dash, shoot, stuff like that.
/// </summary>
public class CharacterBehavior : NetworkBehaviour,CanTakeDamage
{

	public BoxCollider2D HeadCollider ;

    [SyncVar]
    public int Health;
    [SyncVar]
    public int TankHealth;
    public bool incastle = false;
    public bool inbigmonster = false;
    public bool inSmallMonster = false;
    public int IncreaseHP;
    public int CurrentHealth;
    public GameObject _UIcamera;

    /// 角色的各種狀態
    public CharacterBehaviorState BehaviorState { get; private set; }
	/// 角色的默認參數
	public CharacterBehaviorParameters DefaultBehaviorParameters;	
	public CharacterBehaviorParameters BehaviorParameters{get{return _overrideBehaviorParameters ?? DefaultBehaviorParameters;}}
	public CharacterBehaviorPermissions Permissions ;
	
	[Space(10)]	
	[Header("Particle Effects")]
	/// 當角色每次碰到地板都會觸發的效果
	public ParticleSystem TouchTheGroundEffect;
    /// 當角色受到傷害會觸發的效果
	public ParticleSystem HurtEffect;
	
	[Space(10)]	
	[Header("Sounds")]
	// 當角色跳躍時播放的聲音
	public AudioClip PlayerJumpSfx;
	// 當角色受到傷害時播放的聲音
	public AudioClip PlayerHitSfx;
	
	/// 當角色可以跳時=true
	public bool JumpAuthorized 
	{ 
		get 
		{ 
			if ( (BehaviorParameters.JumpRestrictions == CharacterBehaviorParameters.JumpBehavior.CanJumpAnywhere) ||  (BehaviorParameters.JumpRestrictions == CharacterBehaviorParameters.JumpBehavior.CanJumpAnywhereAnyNumberOfTimes) )
				return true;
			
			if (BehaviorParameters.JumpRestrictions == CharacterBehaviorParameters.JumpBehavior.CanJumpOnGround)
				return _controller.State.IsGrounded;
			
			return false; 
		}
	}
	
	// 與物件和位置相關
	private CameraController _Camera;
	private CorgiController _controller;
    private Thorns _thorns;
    private RRRRR _RRRRR;
    private HealthCure _Cure;
    private Tank _Tank;
    private SpeedUp _SpeedUp;
    private Buff _Buff;
    private BigSkill _BigSkill;
    private Animator _animator;
	private CharacterJetpack _jetpack;
	private CharacterShoot _shoot;
	private Color _initialColor;
    CheckPoint _check;
	
	// 儲存首要行為參數
	private CharacterBehaviorParameters _overrideBehaviorParameters;
	// 儲存原始重力與時間
	private float _originalGravity;
	
	// 目前標準化的水平速度
	public float _normalizedHorizontalSpeed;
	
	// 跳躍的時間與按壓次數
	private float _jumpButtonPressTime = 0;
	private bool _jumpButtonPressed=false;
	private bool _jumpButtonReleased=false;

    private string OriginTag;
	
	public bool _isFacingRight=true;
	

	public float _horizontalMove;
	public float _verticalMove;

    public float speedupdata = 5f;
    public Vector2 OriginalPos;//原先位置
    public Vector2 NextPos;//N秒後位置
    private float GameTime = 0;
    public bool isMoving = false;
    public bool isBuff = false;
    private bool Buffing = false;
    private float BuffTime = 0;
    private float OriginSpeed = 0;
    public bool Invincible = false;
    public bool Invincibling = false;
    public float InvincibleTime = 0f;
    public float healeffectTime = 0;
    private float time = 0;
    public bool canMove;
    public bool GameStart = false;
    public bool isHeal;
    public bool isHealing;
    public GameObject BuffShow;
    public GameObject HealShow;
    public GameObject UnrivalShow;
    playerScript _playersc;

    /// 初始化人物的事件
    void Awake()
	{		
		BehaviorState = new CharacterBehaviorState();
        _Camera = gameObject.GetComponent<CameraController>();
		_controller = GetComponent<CorgiController>();
		_jetpack = GetComponent<CharacterJetpack>();
		_shoot = GetComponent<CharacterShoot> ();
        _thorns = GetComponent<Thorns>();
        _RRRRR = GetComponent<RRRRR>();
        _Cure = GetComponent<HealthCure>();
        _Tank = GetComponent<Tank>();
        _Buff = GetComponent<Buff>();
        _SpeedUp = GetComponent<SpeedUp>();
        _BigSkill = GetComponent<BigSkill>();
		Health=BehaviorParameters.MaxHealth;
        TankHealth = BehaviorParameters.TankMaxHealth;
		if (GetComponent<Renderer>()!=null)
			_initialColor=GetComponent<Renderer>().material.color;
	}

	public void Start()
	{
		// 獲取動畫
		_animator = GetComponent<Animator>();
		// 如果人物寬度是正的,朝向右
		_isFacingRight = transform.localScale.x > 0;
		
		_originalGravity = _controller.Parameters.Gravity;
        CurrentHealth = BehaviorParameters.MaxHealth;
		
		// 初始化控制器的所有狀態為默認值
		BehaviorState.Initialize();
		BehaviorState.NumberOfJumpsLeft=BehaviorParameters.NumberOfJumps;
        isMoving = false;//是否有移動
        OriginalPos = gameObject.transform.position;//先記錄原先位置

        BehaviorState.CanJump=true;
        BuffTime = 0;
        OriginTag = gameObject.tag;
        canMove = true;
        _playersc = gameObject.GetComponent<playerScript>();
        if (gameObject.layer == 9)
            _check = GameObject.Find("HumanRespawnPos").GetComponent<CheckPoint>();
        else if (gameObject.layer == 13)
            _check = GameObject.Find("MonsterRespawnPos").GetComponent<CheckPoint>();
    }

    
	

	void Update()
	{
        if (incastle == true)
        {
            Cmdincastle();
        }
        if (inbigmonster == true)
        {
            Cmdinbigmonster();
        }
        if(inSmallMonster == true)
        {
            CmdInSmallMonster();
        }

        if (gameObject.tag == "Daze")
        {
            time += Time.deltaTime;
            if(time >= 3)
            {
                gameObject.tag = OriginTag;
                time = 0;
            }
        }

		// 傳送不同狀態給動畫器		
		UpdateAnimator ();
        // 如果角色沒死        

        GameTime += Time.deltaTime;//遊戲時間
        NextPos = gameObject.transform.position;//記錄下一個位置
        if ((int)GameTime % 5 == 0)
        {
            if (Mathf.Abs(NextPos.x - OriginalPos.x) <= 3 && Mathf.Abs(NextPos.y - OriginalPos.y) <= 3)//如果原先位置與下一個位置X軸以及Y軸在3以內表示沒有移動
            {
                isMoving = false;
            }
            else//若有移動,將原先位置改成下一個位置,繼續跑迴圈
            {
                isMoving = true;
                OriginalPos = NextPos;
            }
        }

        if (!BehaviorState.IsDead)
		{

           if(isHeal)
            {             
                isHealing = true;
                CmdHeal();
            }
           if(isHealing == true)
            {
                healeffectTime += Time.deltaTime;
            }

           if(healeffectTime >= 2f)
            {
                isHealing = false;
                CmdDeHeal();
            }


            if(isBuff == true)
            {
                Buffing = true;
                CmdBuffState();
            }
            if(Buffing == true)
            {
                BuffTime += Time.deltaTime;
            }
            if(BuffTime >= 10f)
            {
                Buffing = false;
                CmdDeBuff();
            }

            if (_Tank != null)
            {
                if (_Tank.PushR == true)
                {
                    gameObject.tag = "Tank";
                    CmdChangeTag("Tank");
                    BehaviorState.NumberOfJumpsLeft -= 2;
                    
                }
                else
                {
                    gameObject.tag = "Player";
                    CmdChangeTag("Player");
                }
                    
            }


            if (_thorns != null)
            {
                if (_thorns.PushE == true)
                {
                    Permissions.ShootEnabled = false;
                    Permissions.MeleeAttackEnabled = false;
                }

                else
                {
                    Permissions.ShootEnabled = true;
                    Permissions.MeleeAttackEnabled = true;
                }
            }

            if(_BigSkill != null)//當醫務兵施放大招
            {
                if(_BigSkill.PushT == true)
                {
                    Invincible = true;
                }
            }
            if(Invincible == true)
            {
                Invincibling = true;
                CmdBigSkillState();
            }
            if(Invincibling == true)
            {
                InvincibleTime += Time.deltaTime;
            }

            if(InvincibleTime >= 3)
            {
                Invincibling = false;
                CmdDeInvincible();
            }
            
            GravityActive(true);
			//處理水平以及垂直運動				
			HorizontalMovement();
			VerticalMovement();			
			
			BehaviorState.CanShoot=true;
			
			// 爬樓梯與牆壁滑行
			ClimbLadder();

			// 如果人物沒開火,重置firestop狀態
			if (!BehaviorState.Firing)
			{
				BehaviorState.FiringStop=false;
			}
			//如果角色跳躍,依照按壓的時間來處理跳躍
			if (JumpAuthorized)
			{				
				//如果使用者放開跳躍鍵並且最初跳躍的時間已經過了,給予一個下降的力讓他停止繼續向上跳躍
				if ( (_jumpButtonPressTime!=0) 
				    && (Time.time - _jumpButtonPressTime >= BehaviorParameters.JumpMinimumAirTime) 
				    && (_controller.Speed.y > Mathf.Sqrt(Mathf.Abs(_controller.Parameters.Gravity))) 
				    && (_jumpButtonReleased)
				    && (!_jumpButtonPressed||BehaviorState.Jetpacking))
				{
					_jumpButtonReleased=false;	
					if (BehaviorParameters.JumpIsProportionalToThePressTime)					
						_controller.AddForce(new Vector2(0,12 * -Mathf.Abs(_controller.Parameters.Gravity) * Time.deltaTime ));			
				}
			}			
		}
		else
		{	
			// if the character is dead, we prevent it from moving horizontally		
			_controller.SetHorizontalForce(0);
		}
	}
	
	/// <summary>
	/// This is called once per frame, after Update();
	/// </summary>
	void LateUpdate()
	{
		// 如果角色在地面,重置二段跳使其可以再次二段跳
		if (_controller.State.JustGotGrounded)
		{
			BehaviorState.NumberOfJumpsLeft=BehaviorParameters.NumberOfJumps;		
		}
		
	}

    [Command]
    public void CmdHeal()
    {
        RpcHeal();
    }
    [ClientRpc]
    public void RpcHeal()
    {
        CmdHealShow(true);
        if(Health < BehaviorParameters.MaxHealth)
        {
            Health += (BehaviorParameters.MaxHealth / 4);
            if (Health > BehaviorParameters.MaxHealth)
            {
                Health = BehaviorParameters.MaxHealth;
            }
        }
        else if(Health == BehaviorParameters.MaxHealth)
        {
            Health = BehaviorParameters.MaxHealth;
        }

        isHeal = false;
    }


    [Command]
    public void CmdBuffShow(bool _active)
    {
        RpcBuffShow(_active);
    }
    [ClientRpc]
    public void RpcBuffShow(bool _active)
    {
        BuffShow.GetComponent<SpriteRenderer>().enabled = _active;
    }


    [Command]
    public void CmdHealShow(bool _active)
    {
        RpcHealShow(_active);
    }
    [ClientRpc]
    public void RpcHealShow(bool _active)
    {
        HealShow.GetComponent<SpriteRenderer>().enabled = _active;
    }


    [Command]
    public void CmdUnrivalShow(bool _active)
    {
        RpcUnrivalShow(_active);
    }
    [ClientRpc]
    public void RpcUnrivalShow(bool _active)
    {
        UnrivalShow.GetComponent<SpriteRenderer>().enabled = _active;
    }


    [Command]
    public void CmdChangeTag(string tag)
    {
        RpcChangeTag(tag);
    }
    [ClientRpc]
    public void RpcChangeTag(string tag)
    {
        gameObject.tag = tag;
    }

	
    [Command]
    private void CmdBuffState()
    {
        RpcBuffState();
    }
    [ClientRpc]
    private void RpcBuffState()
    {
        CmdBuffShow(true);
        OriginSpeed = BehaviorParameters.MovementSpeed;
        BehaviorParameters.MovementSpeed = BehaviorParameters.MovementSpeed + BehaviorParameters.MovementSpeed / 4;
        BehaviorParameters.MaxHealth = BehaviorParameters.MaxHealth + BehaviorParameters.MaxHealth / 3;
        Health += Health / 3;
        isBuff = false;
    }


    [Command]
    public void CmdDeInvincible()
    {
        RpcDeInvincible();
    }
    [ClientRpc]
    public void RpcDeInvincible()
    {
        InvincibleTime = 0;
        CmdUnrivalShow(false);
    }


    [Command]
    private void CmdBigSkillState()
    {
        RpcBigSkillState();
    }
    [ClientRpc]
    private void RpcBigSkillState()
    {
        CmdUnrivalShow(true);
        if (Health < BehaviorParameters.MaxHealth)
        {
            Health += BehaviorParameters.MaxHealth / 3;
            if (Health > BehaviorParameters.MaxHealth)
            {
                Health = BehaviorParameters.MaxHealth;
            }
        }
        if (Health == BehaviorParameters.MaxHealth)
        {
            Health = BehaviorParameters.MaxHealth;
        }
        Invincible = false;
    }


    [Command]
    private void CmdDeHeal()
    {
        RpcDeHeal();
    }
    [ClientRpc]
    private void RpcDeHeal()
    {
        healeffectTime = 0;
        CmdHealShow(false);
    }


    [Command]
    private void CmdDeBuff()
    {
        RpcDeBuff();
    }
    [ClientRpc]
    private void RpcDeBuff()
    {
        CmdBuffShow(false);
        BehaviorParameters.MovementSpeed = OriginSpeed;
        Health = Health * 3 / 4;
        BehaviorParameters.MaxHealth = BehaviorParameters.MaxHealth * 3 / 4;
        BuffTime = 0f;
    }
	
	private void UpdateAnimator()
	{	
		
		UpdateAnimatorBool("Grounded",_controller.State.IsGrounded);
		UpdateAnimatorFloat("Speed",Mathf.Abs(_controller.Speed.x));
		UpdateAnimatorFloat("vSpeed",_controller.Speed.y);
		UpdateAnimatorBool("Running",BehaviorState.Running);
		UpdateAnimatorBool("Crouching",BehaviorState.Crouching);
		UpdateAnimatorBool("LookingUp",BehaviorState.LookingUp);
		UpdateAnimatorBool("Jetpacking",BehaviorState.Jetpacking);
		UpdateAnimatorBool("LadderClimbing",BehaviorState.LadderClimbing);
		UpdateAnimatorFloat("LadderClimbingSpeed",BehaviorState.LadderClimbingSpeed);
		UpdateAnimatorBool("FiringStop",BehaviorState.FiringStop);
		UpdateAnimatorBool("Firing",BehaviorState.Firing);
		UpdateAnimatorInteger("FiringDirection",BehaviorState.FiringDirection);
		UpdateAnimatorBool("MeleeAttacking",BehaviorState.MeleeAttacking);
        UpdateAnimatorBool("C8763Attack", BehaviorState.C8763Attack);
	}
	
	private void UpdateAnimatorBool(string parameterName,bool value)
	{
		if (_animator.HasParameterOfType (parameterName, AnimatorControllerParameterType.Bool))
			_animator.SetBool(parameterName,value);
		}
		private void UpdateAnimatorFloat(string parameterName,float value)
		{
			if (_animator.HasParameterOfType (parameterName, AnimatorControllerParameterType.Float))
				_animator.SetFloat(parameterName,value);
		}
		private void UpdateAnimatorInteger(string parameterName,int value)
		{
			if (_animator.HasParameterOfType (parameterName, AnimatorControllerParameterType.Int))
				_animator.SetInteger(parameterName,value);
		}
	
	
	public void SetHorizontalMove(float value)
	{
        if (canMove == true)
            _horizontalMove = value;
        else
            _horizontalMove = 0;
    }


    public void SetVerticalMove(float value)
    {
        _verticalMove = value;
    }


    private void HorizontalMovement()
    {
        if (!BehaviorState.CanMoveFreely)
            return;

        if (_SpeedUp != null && _SpeedUp.PushE == true)
        {
            StartCoroutine(HorizontalSpeedup());
        }
        else if ((_SpeedUp != null && _SpeedUp.PushE == false) || _SpeedUp == null)
        {

            if (_horizontalMove > 0)
            {
                _normalizedHorizontalSpeed = _horizontalMove * 1;   //速度修改這邊*1 正常
                if (!_isFacingRight)
                    CmdFlip();
            }
            // If it's negative, then we're facing left
            else if (_horizontalMove < 0)
            {
                _normalizedHorizontalSpeed = _horizontalMove * 1;
                if (_isFacingRight)
                    CmdFlip();
            }
            else
            {
                _normalizedHorizontalSpeed = 0;
            }
        }

        // 將水平力給予需要的控制器.

        var movementFactor = _controller.State.IsGrounded ? _controller.Parameters.SpeedAccelerationOnGround : _controller.Parameters.SpeedAccelerationInAir;
        _controller.SetHorizontalForce(Mathf.Lerp(_controller.Speed.x, _normalizedHorizontalSpeed * BehaviorParameters.MovementSpeed, Time.deltaTime * movementFactor));
    }


    private void VerticalMovement()
    {
        if (GameStart == true)
        {
            // Looking up
            if ((_verticalMove > 0) && (_controller.State.IsGrounded))
            {
                BehaviorState.LookingUp = true;
                _Camera.LookUp();
            }
            else
            {
                BehaviorState.LookingUp = false;
                _Camera.ResetLookUpDown();
            }

            // 管理處碰地板的效果
            if (_controller.State.JustGotGrounded)
            {
                Instantiate(TouchTheGroundEffect, new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2), transform.rotation);
            }

            // 如果角色不在一個他能夠自由移動的位置,什麼都不做
            if (!BehaviorState.CanMoveFreely)
                return;

            //蹲伏檢測:如果角色按壓方向鍵下,並且角色在地面,並且蹲伏動作完成
            if ((_verticalMove < -0.1) && (_controller.State.IsGrounded) && (Permissions.CrouchEnabled))
            {
                BehaviorState.Crouching = true;
                BehaviorParameters.MovementSpeed = BehaviorParameters.CrouchSpeed;
                BehaviorState.Running = false;
                _Camera.LookDown();
            }
            else
            {
                // 如果人物蹲著,檢測他是否在洞窟中(利用頭部碰撞檢測)
                if (BehaviorState.Crouching)
                {
                    if (HeadCollider == null)
                    {
                        BehaviorState.Crouching = false;
                        return;
                    }
                    bool headCheck = Physics2D.OverlapCircle(HeadCollider.transform.position, HeadCollider.size.x / 2, _controller.PlatformMask);
                    // 如果人物不再蹲著,設置 
                    if (!headCheck)
                    {
                        if (!BehaviorState.Running)
                            BehaviorParameters.MovementSpeed = BehaviorParameters.WalkSpeed;
                        BehaviorState.Crouching = false;
                        BehaviorState.CanJump = true;
                    }
                    else
                    {

                        BehaviorState.CanJump = false;
                    }
                }
            }

            if (BehaviorState.CrouchingPreviously != BehaviorState.Crouching)
            {
                Invoke("RecalculateRays", Time.deltaTime * 10);
            }

            BehaviorState.CrouchingPreviously = BehaviorState.Crouching;
        }
    }


    /// <summary>
    ///用這個方法來強制控制器重新計算光線,角色大小改變時特別有用
    /// </summary>
    public void RecalculateRays()
	{
		_controller.SetRaysParameters();
	}
	
	/// <summary>
	/// 使角色開始奔跑
	/// </summary>
	public void RunStart()
	{		
		// if the Run action is enabled in the permissions, we continue, if not we do nothing
		if (!Permissions.RunEnabled)
			return;
		// if the character is not in a position where it can move freely, we do nothing.
		if (!BehaviorState.CanMoveFreely)
			return;
		
		// if the player presses the run button and if we're on the ground and not crouching and we can move freely, 
		// then we change the movement speed in the controller's parameters.
		if (_controller.State.IsGrounded && !BehaviorState.Crouching)
		{
			BehaviorParameters.MovementSpeed = BehaviorParameters.RunSpeed;
			BehaviorState.Running=true;
		}
	}
	
	/// <summary>
	/// Causes the character to stop running.
	/// </summary>
	public void RunStop()
	{
		// if the run button is released, we revert back to the walking speed.
		BehaviorParameters.MovementSpeed = BehaviorParameters.WalkSpeed;
		BehaviorState.Running=false;
	}
	
	/// <summary>
	/// Causes the character to start jumping.
	/// </summary>
	public void JumpStart()
	{
		
		// if the Jump action is enabled in the permissions, we continue, if not we do nothing. If the player is dead, we do nothing.
		if (!Permissions.JumpEnabled  || !JumpAuthorized || BehaviorState.IsDead)
			return;
		
		// we check if the character can jump without conflicting with another action
		if (_controller.State.IsGrounded 
		    || BehaviorState.LadderClimbing 
		    || BehaviorState.NumberOfJumpsLeft>0) 	
			BehaviorState.CanJump=true;
		else
			BehaviorState.CanJump=false;
					
		// if the player can't jump, we do nothing. 
		if ( (!BehaviorState.CanJump) && !(BehaviorParameters.JumpRestrictions==CharacterBehaviorParameters.JumpBehavior.CanJumpAnywhereAnyNumberOfTimes) )
			return;
		
		// if the character is standing on a one way platform (layermask n°11) and is also pressing the down button,
		if (_verticalMove<0 && _controller.State.IsGrounded)
		{
			if (_controller.StandingOn.layer==11)
			{
				// we make it fall down below the platform by moving it just below the platform
				_controller.transform.position=new Vector2(transform.position.x,transform.position.y-0.1f);
				// we turn the boxcollider off for a few milliseconds, so the character doesn't get stuck mid platform
				StartCoroutine(_controller.DisableCollisions(0.3f));
				return;
			}
		}
		
		// we decrease the number of jumps left
		BehaviorState.NumberOfJumpsLeft=BehaviorState.NumberOfJumpsLeft-1;
		
		BehaviorState.LadderClimbing=false;
		BehaviorState.CanMoveFreely=true;
		GravityActive(true);
		
		_jumpButtonPressTime=Time.time;
		_jumpButtonPressed=true;
		_jumpButtonReleased=false;
				
		_controller.SetVerticalForce(Mathf.Sqrt( 2f * BehaviorParameters.JumpHeight * Mathf.Abs(_controller.Parameters.Gravity) ));
		
		// we play the jump sound
		/*if (PlayerJumpSfx!=null)
            GameObject.Find("GameManagers").GetComponent<SoundManager>().PlaySound(PlayerJumpSfx,transform.position);*/
		
		
	}
	
	/// <summary>
	/// Causes the character to stop jumping.
	/// </summary>
	public void JumpStop()
	{
		_jumpButtonPressed=false;
		_jumpButtonReleased=true;
	}
	
	


	void ClimbLadder()
	{
        if (_Tank != null && _Tank.PushR == true)
            return;

        if (BehaviorState.LadderColliding)
        {
            if (_verticalMove > 0.1 && !BehaviorState.LadderClimbing && !BehaviorState.LadderTopColliding && !BehaviorState.Jetpacking)
            {
                BehaviorState.LadderClimbing = true;
                _controller.CollisionsOn();

                BehaviorState.CanMoveFreely = false;
                if (_shoot != null)
                    _shoot.CmdShootStop();
                BehaviorState.LadderClimbingSpeed = 0;
                _controller.SetHorizontalForce(0);
                _controller.SetVerticalForce(0);
                GravityActive(false);
            }

            if (BehaviorState.LadderClimbing)
            {
                BehaviorState.CanShoot = false;
                GravityActive(false);

                if (!BehaviorState.LadderTopColliding)
                    _controller.CollisionsOn();

                _controller.SetVerticalForce(_verticalMove * BehaviorParameters.LadderSpeed);
                BehaviorState.LadderClimbingSpeed = Mathf.Abs(_verticalMove);
            }

            if (BehaviorState.LadderClimbing && _controller.State.IsGrounded && !BehaviorState.LadderTopColliding)
            {
                BehaviorState.LadderColliding = false;
                BehaviorState.LadderClimbing = false;
                BehaviorState.CanMoveFreely = true;
                BehaviorState.LadderClimbingSpeed = 0;
                GravityActive(true);
            }
        }

        if (BehaviorState.LadderTopColliding && _verticalMove < -0.1 && !BehaviorState.LadderClimbing && _controller.State.IsGrounded)
        {
            _controller.CollisionsOff();
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.1f);
            BehaviorState.LadderClimbing = true;
            BehaviorState.CanMoveFreely = false;
            BehaviorState.LadderClimbingSpeed = 0;
            _controller.SetHorizontalForce(0);
            _controller.SetVerticalForce(0);
            GravityActive(false);
        }



    }
	

	IEnumerator Boost(float boostDuration, float boostForceX, float boostForceY, string name) 
	{
		float time = 0f; 
		
		while(boostDuration > time) 
		{
			if (boostForceX!=0)
			{
				_controller.AddForce(new Vector2(boostForceX,0));
			}
			if (boostForceY!=0)
			{
				_controller.AddForce(new Vector2(0,boostForceY));
			}
			time+=Time.deltaTime;
			yield return 0; 
		}

	}

    IEnumerator HorizontalSpeedup()
    {
        _SpeedUp.PushE = false;
        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            if (_horizontalMove > 0.01)
            {

                _normalizedHorizontalSpeed = _horizontalMove * speedupdata;//速度調整這邊

                if (!_isFacingRight)
                    CmdFlip();
            }
            else if (_horizontalMove < -0.01)
            {

                _normalizedHorizontalSpeed = _horizontalMove * speedupdata;
                if (_isFacingRight)
                    CmdFlip();
            }
            else
            {
                _normalizedHorizontalSpeed = 0;
            }
            var movementFactor = _controller.State.IsGrounded ? _controller.Parameters.SpeedAccelerationOnGround : _controller.Parameters.SpeedAccelerationInAir;
            _controller.SetHorizontalForce(Mathf.Lerp(_controller.Speed.x, _normalizedHorizontalSpeed * BehaviorParameters.MovementSpeed, Time.deltaTime * movementFactor));

            yield return 0;
        }
        _SpeedUp.SpeedupEffect.GetComponent<SpriteRenderer>().enabled = false;
        _SpeedUp._Spactive.CmdSpactive(false);
    }

	private void GravityActive(bool state)
	{
		if (state==true)
		{
			if (_controller.Parameters.Gravity==0)
			{
				_controller.Parameters.Gravity = _originalGravity;
			}
		}
		else
		{
			if (_controller.Parameters.Gravity!=0)
				_originalGravity = _controller.Parameters.Gravity;
			_controller.Parameters.Gravity = 0;
		}
	}

	IEnumerator Flicker(Color initialColor, Color flickerColor, float flickerSpeed)
	{
		if (GetComponent<Renderer>()!=null)
		{			
			for(var n = 0; n < 10; n++)
			{
				GetComponent<Renderer>().material.color = initialColor;
				yield return new WaitForSeconds (flickerSpeed);
				GetComponent<Renderer>().material.color = flickerColor;
				yield return new WaitForSeconds (flickerSpeed);
			}
			GetComponent<Renderer>().material.color = initialColor;
		}				
	}
	

	IEnumerator ResetLayerCollision(float delay)
	{
		yield return new WaitForSeconds (delay);
		Physics2D.IgnoreLayerCollision(9,12,false);
		Physics2D.IgnoreLayerCollision(9,13,false);
	}
	
    [Command]
	public void CmdKill()
	{
        RpcKill();
	}
    [ClientRpc]
    public void RpcKill()
    {
        _controller.CollisionsOff();
        GetComponent<Collider2D>().enabled = false;
        BehaviorState.IsDead = true;
        if(Buffing)
        {
            CmdDeBuff();
        }
        if(isHealing)
        {
            CmdDeHeal();
        }
        if(Invincibling)
        {
            CmdDeInvincible();
        }       
      
        Health = 0;
        _controller.ResetParameters();
        ResetParameters();
        _controller.SetForce(new Vector2(0, 10));
    }
	
	public void Disable()
	{
		enabled=false;
		_controller.enabled=false;
		GetComponent<Collider2D>().enabled=false;		
	}

	public void RespawnAt(Transform spawnPoint)
	{
		if(!_isFacingRight)
		{
			CmdFlip ();
		}
		BehaviorState.IsDead=false;
		GetComponent<Collider2D>().enabled=true;
		_controller.CollisionsOn();
		transform.position=spawnPoint.position;
		Health=BehaviorParameters.MaxHealth;
        TankHealth = BehaviorParameters.TankMaxHealth;
	}
	
    [Command]
	public void CmdTakeDamage(int damage,GameObject instigator)
	{
       /* if (PlayerHitSfx != null)
            GameObject.Find("GameManagers").GetComponent<SoundManager>().PlaySound(PlayerHitSfx, transform.position);*/

        Instantiate(HurtEffect, transform.position, transform.rotation);
        Physics2D.IgnoreLayerCollision(9, 12, true);
        Physics2D.IgnoreLayerCollision(9, 13, true);
        StartCoroutine(ResetLayerCollision(0.5f));
        if (GetComponent<Renderer>() != null)
        {
            Color flickerColor = new Color32(255, 20, 20, 255);
            StartCoroutine(Flicker(_initialColor, flickerColor, 0.05f));
        }
        if (_BigSkill != null && _Tank != null)
        {
            if (_Tank.PushR == false && Invincible == false)
            {
                Health -= damage;
                CurrentHealth = Health;
                if(Health <= 0 && gameObject.GetComponent<AIFollower>() != null)
                {
                    Destroy(gameObject);
                }
                if (Health <= 0 && gameObject.GetComponent<AIFollower>() == null)
                {
                    CmdChangeTag("Die");
                    CmdKillPlayer();
                }
            }
            else if (_Tank.PushR == true && Invincible == false)
            {
                TankHealth -= damage;
                if (TankHealth <= 0)
                {
                    _Tank.PushR = false;
                }
            }
            else if (_Tank.PushR == false && Invincible == true)
            {

            }
            else if (_Tank.PushR == true && Invincible == true)
            {

            }
        }
        else if (_Tank != null && _BigSkill == null)
        {
            if (_Tank.PushR == false)
            {
                Health -= damage;
                CurrentHealth = Health;
                if (Health <= 0 && gameObject.GetComponent<AIFollower>() != null)
                {
                    Destroy(gameObject);
                }
                if (Health <= 0 && gameObject.GetComponent<AIFollower>() == null)
                {
                    CmdChangeTag("Die");
                    CmdKillPlayer();
                }
            }
            else if (_Tank.PushR == true)
            {
                TankHealth -= damage;
                if (TankHealth <= 0)
                {
                    _Tank.PushR = false;
                }
            }
        }
        else if (_Tank == null && _BigSkill != null)
        {
            if (Invincible == true)
            {

            }
            else if (Invincible == false)
            {
                Health -= damage;
                CurrentHealth = Health;
                if (Health <= 0 && gameObject.GetComponent<AIFollower>() != null)
                {
                    Destroy(gameObject);
                }
                if (Health <= 0 && gameObject.GetComponent<AIFollower>() == null)
                {
                    CmdChangeTag("Die");
                    CmdKillPlayer();
                }
            }
        }
        else if (_Tank == null && _BigSkill == null)
        {
            Health -= damage;
            CurrentHealth = Health;
            if (Health <= 0 && gameObject.GetComponent<AIFollower>() != null)
            {
                Destroy(gameObject);
            }
            if (Health <= 0 && gameObject.GetComponent<AIFollower>() == null)
            {
                CmdChangeTag("Die");
                CmdKillPlayer();
            }
        }
        if (_RRRRR != null && _RRRRR.PushR == true)
        {
            Health -= damage / 5;
        }
        else
        {
            Health -= damage;
            CurrentHealth = Health;
        }
        if (Health <= 0 && gameObject.GetComponent<AIFollower>() != null)
        {
            Destroy(gameObject);
        }
        if (Health <= 0 && gameObject.GetComponent<AIFollower>() == null)
        {
            CmdChangeTag("Die");
            CmdKillPlayer();
        }
    }


	

	public void GiveHealth(int health,GameObject instigator)
	{
		Health = Mathf.Min (Health + health,BehaviorParameters.MaxHealth);
	}
	

    [Command]
	public void CmdFlip()
	{
        RpcFlip();
    }

    [ClientRpc]
    public void RpcFlip()
    {
        Flip();
    }

    void Flip()
    {
        if (_playersc != null && _playersc.CanCallAgain == true)
        {

            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _UIcamera.transform.localScale = new Vector3(-_UIcamera.transform.localScale.x, _UIcamera.transform.localScale.y, _UIcamera.transform.localScale.z);
            if (transform.localScale.x > 0)
                _isFacingRight = true;
            else
                _isFacingRight = false;


            if (_jetpack != null)
            {
                if (_jetpack.Jetpack != null)
                    _jetpack.Jetpack.transform.eulerAngles = new Vector3(_jetpack.Jetpack.transform.eulerAngles.x, _jetpack.Jetpack.transform.eulerAngles.y + 180, _jetpack.Jetpack.transform.eulerAngles.z);
            }
            if (_shoot != null)
            {
                _shoot.CmdFlip();
            }
            _playersc.CanCallAgain = false;
            _playersc.calltime = 0f;
        }
        else if (_playersc != null && _playersc.CanCallAgain == false)
        {
            return;
        }
        else if (_playersc == null)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (transform.localScale.x > 0)
                _isFacingRight = true;
            else
                _isFacingRight = false;
        }
    }


    public void ResetParameters()
	{
		_overrideBehaviorParameters = DefaultBehaviorParameters;
        gameObject.tag = OriginTag;
        CmdChangeTag(OriginTag);
    }
    
    [Command]
    public void CmdKillPlayer()
    {
        RpcKillPlayer();
    }
    [ClientRpc]
    public void RpcKillPlayer()
    {
        StartCoroutine(KillPlayerCo());
    }

    private IEnumerator KillPlayerCo()
    {
        CmdKill();

        yield return new WaitForSeconds(2f);
        _check.SpawnPlayer(gameObject.GetComponent<CharacterBehavior>());
        Health = BehaviorParameters.MaxHealth;
    }

    public void Cmdincastle()
    {
        Health += 1;
        if (Health >= BehaviorParameters.MaxHealth)
        {
            Health = BehaviorParameters.MaxHealth;
        }
    }
    public void Cmdinbigmonster()
    {
        Health -= 1;
        if (Health <= 1)
        {
            Health = 1;
        }
    }

    public void CmdInSmallMonster()
    {
        Health -= 1;
        if (Health <= 1)
        {
            Health = 1;
        }
    }

}
