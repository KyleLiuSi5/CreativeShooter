using UnityEngine;
using System.Collections;

public class ChargeAI : MonoBehaviour {
    //宣告場上會出現的4種human方角色以及3種Monster方角色
    public GameObject _Tank;
    public GameObject _Summoner;
    public GameObject _Shooter;
    public GameObject _DiamondProtecter;
    public GameObject _artilleryman;
    public GameObject _Commander;
    public GameObject _Curer;
    //我方基地
    public GameObject TeamCastle;
    //敵方基地
    public GameObject EnemyCastle;
    //角色攻擊目標
    public GameObject _target;
    //角色陣列
    public GameObject[] Team;
    //敵人陣列
    public GameObject[] Enemy;
    //最近的敵人
    public GameObject NearestEnemy;

    //我方以及敵方出現在可視距離內的角色
    public bool hasTank = true;
    public bool hasSummoner = true;
    public bool hasShooter = true;
    public bool hasDiamondProtecter = false;
    public bool hasArtilleryman = false;
    public bool hasCommander = false;
    public bool hasCurer = false;
    public bool hasOwn = false;
    //是否看到敵方角色
    public bool SeeEnemy = false;
    //角色有危險
    public bool Dangerous = false;
    public bool DangerousAgain = false;
    //角色使用了技能
    public bool useShield = false;
    public bool useShieldAttack = false;
    public bool useC8763 = false;
    //角色跟隨隊伍前進
    public bool FollowTeam;
    //決定要進攻還是防守
    public bool Attack;
    public bool Defense;
    public bool FireWithEnemy;

    //定義自己可視距離
    public float SightDistance = 10f;
    public float TeamDistance = 12f;
    //定義與敵人的安全距離
    public float SafeDistance = 5f;
    //在可視範圍內獲取敵方血量
    public float _TankBlood;
    public float _ShooterBlood;
    public float _SummonerBlood;
    public float _DiamondProtecterBlood;
    //敵方最後出現時的血量
    public float _TankLastBlood;
    public float _ShooterLastBlood;
    public float _SummonerLastBlood;
    public float _DiamondProtecterLastBlood;
    //我方血量
    public float _artillerymanBlood;
    public float _CurerBlood;
    public float _CommanderBlood;
    //定義權重
    public float Weight = 0f;
    //定義遊戲時間
    public float GameTime = 0f;
    //定義敵方最後看到的時間
    public float _TankLastSeeTime;
    public float _ShooterLastSeeTime;
    public float _SummonerLastSeeTime;
    public float _DiamondProtecterLastSeeTime;
    //施放技能後的脫逃時間
    public float RunAwayTime = 0f;
    public float DangerousAgainTime = 0f;
    //主堡的方向
    public float _Direction;
    //角色的速度
    public float _Speed;
    //角色對目的地距離決定採取什麼動作
    public float RunDistance = 10f;
    public float WalkDistance = 5f;
    public float StopDistance = 1f;
    public float JetpackDistance = 0.2f;
    //攻擊與防守的權重
    public float AttackWeight = 0;
    public float DefenseWeight = 0;
    //我方以及敵方主堡的血量
    public float TeamCastleBlood = 0;
    public float EnemyCastleBlood = 0;
    //與我方主堡的距離
    public float TeamCastleDistance = 0;
    //與敵方主堡的距離
    public float EnemyCastleDistance = 0;
    //前往的位置的權重
    public float NextPositionWeight = 0;
    //最近的敵人的距離
    public float NearestDistance = 0;

    //敵方角色數量
    public int EnemyCount = 0;
    //我方角色數量
    public int TeamCount = 0;
    //自己的生命值
    public int CurrentHealth;
    //某一範圍內我方角色數量
    public int TeammateCount = 0;

    //敵方角色位置
    public Transform _TankPosition;
    public Transform _ShooterPosition;
    public Transform _SummonerPosition;
    public Transform _DiamondProtecterPosition;
    //我方角色位置
    public Transform _artillerymanPosition;
    public Transform _CurerPosition;
    public Transform _CommanderPosition;
    public Transform _OwnPosition;
    //敵方角色最後位置
    public Transform _TankLastPosition;
    public Transform _ShooterLastPosition;
    public Transform _SummonerLastPosition;
    public Transform _DiamondProtecterLastPosition;
    //我方基地的位置
    public Transform TeamCastlePosition;
    //敵方基地的位置
    public Transform EnemyCastlePosition;
    //下一個路徑位置
    public Transform NextPosition;

    //選擇下一步要到的位置
    public Vector3 NextPosition0;
    public Vector3 NextPosition1;
    public Vector3 NextPosition2;
    public Vector3 NextPosition3;
    public Vector3 NextPosition4;
    public Vector3 NextPosition5;
    public Vector3 NextPosition6;
    public Vector3 NextPosition7;
    public Transform RespawnPos;

    //可獲取Human方角色狀態
    public CharacterBehavior _artillerymanBehavior;
    public CharacterBehavior _CurerBehavior;
    public CharacterBehavior _CommanderBehavior;
    public CharacterBehavior _OwnBehavior;
    private CorgiController _Controller;
    private CharacterShoot _AIShoot;
    private CharacterJetpack _JetPack;
    private Camera _MainCamera;
    private Transform _Target;
    //Diamond的技能
    private Thorns _Shield;//分身
    private Thorns _ShieldAttack;//瞬間移動
    private C8763 _C8763;

    //判斷A*的下一步是不是可以行走的位置
    private TargetEnter _CantWalk;


    // Use this for initialization
    void Start()
    {

        if (GameObject.Find("artilleryman") != null)
        {
            _artilleryman = GameObject.Find("artilleryman");
            _artillerymanBehavior = _artilleryman.GetComponent<CharacterBehavior>();
            _artillerymanPosition = _artilleryman.transform;
            _artillerymanBlood = _artilleryman.GetComponent<CharacterBehavior>().CurrentHealth;
        }
        else if (GameObject.Find("artilleryman(Clone)") != null)
        {
            _artilleryman = GameObject.Find("artilleryman(Clone)");
            _artillerymanBehavior = _artilleryman.GetComponent<CharacterBehavior>();
            _artillerymanPosition = _artilleryman.transform;
            _artillerymanBlood = _artilleryman.GetComponent<CharacterBehavior>().CurrentHealth;
        }

        if (GameObject.Find("Curer") != null)
        {
            _Curer = GameObject.Find("Curer");
            _CurerBehavior = _Curer.GetComponent<CharacterBehavior>();
            _CurerPosition = _Curer.transform;
            _CurerBlood = _Curer.GetComponent<CharacterBehavior>().CurrentHealth;
        }
        else if (GameObject.Find("Curer(Clone)") != null)
        {
            _Curer = GameObject.Find("Curer(Clone)");
            _CurerBehavior = _Curer.GetComponent<CharacterBehavior>();
            _CurerPosition = _Curer.transform;
            _CurerBlood = _Curer.GetComponent<CharacterBehavior>().CurrentHealth;
        }

        if (GameObject.Find("Commander") != null)
        {
            _Commander = GameObject.Find("Commander");
            _CommanderBehavior = _Commander.GetComponent<CharacterBehavior>();
            _CommanderPosition = _Commander.transform;
            _CommanderBlood = _Commander.GetComponent<CharacterBehavior>().CurrentHealth;
        }
        else if (GameObject.Find("Commander(Clone)") != null)
        {
            _Commander = GameObject.Find("Commander(Clone)");
            _CommanderBehavior = _Commander.GetComponent<CharacterBehavior>();
            _CommanderPosition = _Commander.transform;
            _CommanderBlood = _Commander.GetComponent<CharacterBehavior>().CurrentHealth;
        }

        if (GameObject.Find("Tanker") != null)
        {
            _Tank = GameObject.Find("Tanker");
            _TankBlood = _Tank.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }
        else if (GameObject.Find("Tanker(Clone)") != null)
        {
            _Tank = GameObject.Find("Tanker(Clone)");
            _TankBlood = _Tank.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }

        if (GameObject.Find("Shooter") != null)
        {
            _Shooter = GameObject.Find("Shooter");
            _ShooterBlood = _Shooter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }
        else if (GameObject.Find("Shooter(Clone)") != null)
        {
            _Shooter = GameObject.Find("Shooter(Clone)");
            _ShooterBlood = _Shooter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }

        if (GameObject.Find("Summoner") != null)
        {
            _Summoner = GameObject.Find("Summoner");
            _SummonerBlood = _Summoner.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }
        else if (GameObject.Find("Summoner(Clone)") != null)
        {
            _Summoner = GameObject.Find("Summoner(Clone)");
            _SummonerBlood = _Summoner.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }

        if (GameObject.Find("DiamondProtecter") != null)
        {
            _DiamondProtecter = GameObject.Find("DiamondProtecter");
            _DiamondProtecterBlood = _DiamondProtecter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }
        else if (GameObject.Find("DiamondProtecter(Clone)") != null)
        {
            _DiamondProtecter = GameObject.Find("DiamondProtecter(Clone)");
            _DiamondProtecterBlood = _DiamondProtecter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }

        if (GameObject.Find("HumanCastle") != null)
        {
            TeamCastle = GameObject.Find("HumanCastle");
            TeamCastleBlood = TeamCastle.GetComponent<Health>().OriginalHealth;
            TeamCastlePosition = TeamCastle.transform;
        }
        if (GameObject.Find("MonsterCastle") != null)
        {
            EnemyCastle = GameObject.Find("MonsterCastle");
            EnemyCastleBlood = EnemyCastle.GetComponent<Health>().OriginalHealth;
            EnemyCastlePosition = EnemyCastle.transform;
        }

        _OwnBehavior = gameObject.GetComponent<CharacterBehavior>();
        _OwnPosition = gameObject.transform;
        _Controller = GetComponent<CorgiController>();
        _Shield = GetComponent<Thorns>();
        _ShieldAttack = GetComponent<Thorns>();
        _C8763 = GetComponent<C8763>();
        _AIShoot = GetComponent<CharacterShoot>();
        _JetPack = GetComponent<CharacterJetpack>();
        CurrentHealth = _OwnBehavior.BehaviorParameters.MaxHealth;
        RunAwayTime = 5f;
        FollowTeam = true;
        Team = GameObject.FindGameObjectsWithTag("Player");
        NearestEnemy = null;
        FireWithEnemy = false;
        NearestDistance = 0;
        _CantWalk = gameObject.GetComponentInChildren<TargetEnter>();
    }

    // Update is called once per frame
    void Update()
    {

        GameTime += Time.deltaTime;

        //每秒更新自身血量
        CurrentHealth = _OwnBehavior.CurrentHealth;

        //計算附近的隊員數量與敵人數量以及角色種類,敵方血量等數值
        GetStatus();

        //角色行為
        AIActivity();

        if (RunAwayTime >= 1 && RunAwayTime <= 5)
        {
            RunAwayTime += Time.deltaTime;
        }
        else if (RunAwayTime >= 5)
        {
            RunAwayTime = 0;
        }

        if (Attack == true && FireWithEnemy == false)//如果權重為進攻並且沒有與他人交火中,則向敵方主堡位置前進
        {
            _Target = EnemyCastlePosition;
            
        }
        if (Defense == true)//如果權重為防守,則向我方主堡位置逃跑
        {
            FireWithEnemy = false;
            _Target = TeamCastlePosition;
            
        }
        if (_OwnBehavior.isMoving == false && FireWithEnemy == false)//如果沒有在移動並且沒有在與敵方交火,則重新計算攻防權重
        {
            
            AttackOrDefense();
        }
        if (NearestEnemy != null && Attack == true)//如果附近有敵人並且為進攻狀態,則與敵方交火為True
        {
            FireWithEnemy = true;
        }
        else if (NearestEnemy == null)//反之,附近沒有敵方則交火狀態為false
        {
            FireWithEnemy = false;
        }
        if (FireWithEnemy == true)//如果交火則呼叫attackEnemy函式
        {
            AttackEnemy();
        }


    }


    public void AttackEnemy()
    {
        //將路線歸零
        _target.transform.position = Vector3.zero;//歸零綁定在AI身上的target位置
        if (Mathf.Abs(NearestEnemy.transform.position.y - _OwnPosition.position.y) >= 1)
        {
            if (NearestEnemy.transform.position.x > _OwnPosition.position.x)//追著敵方開火
            {
                var GoodPosX = NearestEnemy.transform.position.x - SafeDistance;
                var GoodPosY = NearestEnemy.transform.position.y;
                var GoodPosZ = gameObject.transform.position.z;
                var GoodPos = new Vector3(GoodPosX, GoodPosY, GoodPosZ);
                _target.transform.position = GoodPos;
                _Target = _target.transform;
                


            }
            else if (NearestEnemy.transform.position.x < _OwnPosition.position.x)
            {
                var GoodPosX = NearestEnemy.transform.position.x + SafeDistance;
                var GoodPosY = NearestEnemy.transform.position.y;
                var GoodPosZ = gameObject.transform.position.z;
                var GoodPos = new Vector3(GoodPosX, GoodPosY, GoodPosZ);
                _target.transform.position = GoodPos;
                _Target = _target.transform;
            }
        }

    }

    //如果一個範圍內聚集的同伴較多,跟隨那個範圍內的同伴
    public void FollowTeammate()
    {
        int Temp = 0;
        TeammateCount = 0;
        for (int j = 0; j < Team.Length; j++)
        {
            for (int i = 0; i < Team.Length; i++)
            {
                if (i == j)
                    continue;
                if (Team[i].transform.position.x < Team[j].transform.position.x + TeamDistance && Team[i].transform.position.x > Team[j].transform.position.x - TeamDistance
                    && Team[i].transform.position.y < Team[j].transform.position.y + TeamDistance && Team[i].transform.position.y > Team[j].transform.position.y - TeamDistance)
                {
                    TeammateCount++;
                }
            }
            if (TeammateCount >= Temp)
            {
                Temp = TeammateCount;
                TeammateCount = 0;
                _Target = Team[j].transform;
            }

        }
        
        FollowTeam = false;
    }

    //獲取各角色狀態
    private void GetStatus()
    {
        TeamCount = 0;
        EnemyCount = 0;
        NearestDistance = 0;
        NearestEnemy = null;
        //自身血量不低於10%可列為戰鬥人員
        if (CurrentHealth >= CurrentHealth * 0.2)
        {
            TeamCount++;
            hasOwn = true;
        }
        else
        {
            TeamCount--;
            hasOwn = false;
        }


        //角色的位置在Diamond的可視範圍內並且血量不低於自身的10%,可將之視為可戰鬥人員並且記錄角色
        if (_artilleryman != null)
        {
            if (_artilleryman.transform.position.x < (gameObject.transform.position.x + TeamDistance) && _artilleryman.transform.position.x > (gameObject.transform.position.x - TeamDistance)
            && _artilleryman.transform.position.y < (gameObject.transform.position.y + TeamDistance) && _artilleryman.transform.position.y > (gameObject.transform.position.y - TeamDistance) && _artillerymanBehavior.CurrentHealth >= _artillerymanBehavior.BehaviorParameters.MaxHealth * 0.1)
            {
                TeamCount++;
                hasArtilleryman = true;
                _artillerymanBlood = _artilleryman.GetComponent<CharacterBehavior>().CurrentHealth;
            }

            else
            {
                hasArtilleryman = false;
                _artillerymanBlood = _artilleryman.GetComponent<CharacterBehavior>().CurrentHealth;
            }
        }
        else
            hasArtilleryman = false;


        if (_Curer != null)
        {
            if (_Curer.transform.position.x < (gameObject.transform.position.x + TeamDistance) && _Curer.transform.position.x > (gameObject.transform.position.x - TeamDistance)
            && _Curer.transform.position.y < (gameObject.transform.position.y + TeamDistance) && _Curer.transform.position.y > (gameObject.transform.position.y - TeamDistance) && _CurerBehavior.CurrentHealth > _CurerBehavior.BehaviorParameters.MaxHealth * 0.1)
            {
                TeamCount++;
                hasCurer = true;
                _CurerBlood = _Curer.GetComponent<CharacterBehavior>().CurrentHealth;
            }

            else
            {
                hasCurer = false;
                _CurerBlood = _Curer.GetComponent<CharacterBehavior>().CurrentHealth;
            }
        }
        else
            hasCurer = false;

        if (_Commander != null)
        {

            if (_Commander.transform.position.x < (gameObject.transform.position.x + TeamDistance) && _Commander.transform.position.x > (gameObject.transform.position.x - TeamDistance)
            && _Commander.transform.position.y < (gameObject.transform.position.y + TeamDistance) && _Commander.transform.position.y > (gameObject.transform.position.y - TeamDistance) && _CommanderBehavior.CurrentHealth > _CommanderBehavior.BehaviorParameters.MaxHealth * 0.1)
            {
                TeamCount++;
                hasCommander = true;
                _CommanderBlood = _Commander.GetComponent<CharacterBehavior>().CurrentHealth;

            }

            else
            {
                hasCommander = false;
                _CommanderBlood = _Commander.GetComponent<CharacterBehavior>().CurrentHealth;

            }
        }
        else
            hasCommander = false;


        //若敵方出現在可視距離內則敵方角色數量+1並且記錄出現的角色,獲取對方血量以及歸零最後看到時間
        if (_Tank != null)
        {
            if (_Tank.transform.position.x < (gameObject.transform.position.x + SightDistance) && _Tank.transform.position.x > (gameObject.transform.position.x - SightDistance)
           && _Tank.transform.position.y < (gameObject.transform.position.y + SightDistance) && _Tank.transform.position.y > (gameObject.transform.position.y - SightDistance))
            {
                EnemyCount++;
                hasTank = true;
                _TankBlood = _Tank.GetComponent<CharacterBehavior>().CurrentHealth;
                _TankPosition = _Tank.transform;
                _TankLastPosition = _TankPosition;
                _TankLastSeeTime = 0;
                if (NearestDistance <= Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _TankPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _TankPosition.position.y, 2)))
                {
                    NearestDistance = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _TankPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _TankPosition.position.y, 2));
                    NearestEnemy = _Tank;
                }
            }

            else
            {
                _TankLastSeeTime += Time.deltaTime;
                hasTank = false;
                _TankLastBlood = _TankBlood;
                _TankPosition = null;
                if (_TankLastSeeTime > 5)
                {
                    _TankLastBlood = _Tank.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
                    _TankLastPosition = null;
                }
            }
        }

        if (_Shooter != null)
        {
            if (_Shooter.transform.position.x < (gameObject.transform.position.x + SightDistance) && _Shooter.transform.position.x > (gameObject.transform.position.x - SightDistance)
          && _Shooter.transform.position.y < (gameObject.transform.position.y + SightDistance) && _Shooter.transform.position.y > (gameObject.transform.position.y - SightDistance))
            {
                EnemyCount++;
                hasShooter = true;
                _ShooterBlood = _Shooter.GetComponent<CharacterBehavior>().CurrentHealth;
                _ShooterPosition = _Shooter.transform;
                _ShooterLastPosition = _ShooterPosition;
                _ShooterLastSeeTime = 0;
                if (NearestDistance <= Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _ShooterPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _ShooterPosition.position.y, 2)))
                {
                    NearestDistance = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _ShooterPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _ShooterPosition.position.y, 2));
                    NearestEnemy = _Shooter;
                }
            }

            else
            {
                _ShooterLastSeeTime += Time.deltaTime;
                hasShooter = false;
                _ShooterLastBlood = _ShooterBlood;
                _ShooterPosition = null;
                if (_ShooterLastSeeTime > 5)
                {
                    _ShooterLastBlood = _Shooter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
                    _ShooterLastPosition = null;
                }
            }
        }

        if (_Summoner != null)
        {
            if (_Summoner.transform.position.x < (gameObject.transform.position.x + SightDistance) && _Summoner.transform.position.x > (gameObject.transform.position.x - SightDistance)
           && _Summoner.transform.position.y < (gameObject.transform.position.y + SightDistance) && _Summoner.transform.position.y > (gameObject.transform.position.y - SightDistance))
            {
                EnemyCount++;
                hasSummoner = true;
                _SummonerBlood = _Summoner.GetComponent<CharacterBehavior>().CurrentHealth;
                _SummonerPosition = _Summoner.transform;
                _SummonerLastPosition = _SummonerPosition;
                _SummonerLastSeeTime = 0;
                if (NearestDistance <= Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _SummonerPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _SummonerPosition.position.y, 2)))
                {
                    NearestDistance = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _SummonerPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _SummonerPosition.position.y, 2));
                    NearestEnemy = _Summoner;
                }
            }

            else
            {
                _SummonerLastSeeTime += Time.deltaTime;
                hasSummoner = false;
                _SummonerLastBlood = _SummonerBlood;
                _SummonerPosition = null;
                if (_SummonerLastSeeTime > 5)
                {
                    _SummonerLastBlood = _Summoner.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
                    _SummonerLastPosition = null;
                }
            }
        }

        if (_DiamondProtecter != null)
        {
            if (_DiamondProtecter.transform.position.x < (gameObject.transform.position.x + SightDistance) && _DiamondProtecter.transform.position.x > (gameObject.transform.position.x - SightDistance)
          && _DiamondProtecter.transform.position.y < (gameObject.transform.position.y + SightDistance) && _DiamondProtecter.transform.position.y > (gameObject.transform.position.y - SightDistance))
            {
                EnemyCount++;
                hasDiamondProtecter = true;
                _DiamondProtecterBlood = _DiamondProtecter.GetComponent<CharacterBehavior>().CurrentHealth;
                _DiamondProtecterPosition = _DiamondProtecter.transform;
                _DiamondProtecterLastPosition = _DiamondProtecterPosition;
                _DiamondProtecterLastSeeTime = 0;
                if (NearestDistance <= Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _DiamondProtecterPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _DiamondProtecterPosition.position.y, 2)))
                {
                    NearestDistance = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _DiamondProtecterPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _DiamondProtecterPosition.position.y, 2));
                    NearestEnemy = _DiamondProtecter;
                }
            }

            else
            {
                _DiamondProtecterLastSeeTime += Time.deltaTime;
                hasDiamondProtecter = false;
                _DiamondProtecterLastBlood = _DiamondProtecterBlood;
                _DiamondProtecterPosition = null;
                if (_DiamondProtecterLastSeeTime > 5)
                {
                    _DiamondProtecterLastBlood = _DiamondProtecter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
                    _DiamondProtecterLastPosition = null;
                }
            }
        }
        //更新我方及敵方主堡血量
        if (TeamCastle != null)
        {
            TeamCastleBlood = TeamCastle.GetComponent<Health>().CurrentHealth;
            TeamCastleDistance = Mathf.Sqrt(Mathf.Pow(_OwnPosition.position.x - TeamCastlePosition.position.x, 2) + Mathf.Pow(_OwnPosition.position.y - TeamCastlePosition.position.y, 2));
        }

        if (EnemyCastle != null)
        {
            EnemyCastleBlood = EnemyCastle.GetComponent<Health>().CurrentHealth;
            EnemyCastleDistance = Mathf.Sqrt(Mathf.Pow(_OwnPosition.position.x - EnemyCastlePosition.position.x, 2) + Mathf.Pow(_OwnPosition.position.y - EnemyCastlePosition.position.y, 2));
        }


    }

    //AI的行為
    private void AIActivity()
    {
        if(FireWithEnemy == true)
        {
            UseSkill();
        }
        if(_OwnBehavior.Health <= 0)
        {
            gameObject.tag = "Die";
            gameObject.GetComponent<AILerp>().enabled = false;
        }
        else
        {
            gameObject.tag = "Player";
            gameObject.GetComponent<AILerp>().enabled = true;
        }
        if(_C8763.PushT == true)
        {
            
        }
    }


    //AI施放技能
    private void UseSkill()
    {
        if (NearestEnemy != null && NearestDistance <= 3)
        {
            if (NearestEnemy.transform.position.x < gameObject.transform.position.x)
            {
                if (_ShieldAttack.CanPushR == true && _C8763.CanPushT == true)
                {
                    _ShieldAttack.PushR = true;
                }
                else if (_ShieldAttack == true && _C8763.CanPushT == false)
                {
                    _ShieldAttack.PushR = true;
                }
                else if (_ShieldAttack == false && _C8763.CanPushT == true)
                {
                    _C8763.PushT = true;
                }
                else
                    return;
            }
        }
        else
            return;
        if(EnemyCount >= 2)
        {
            if (_Shield.CanPushE == true)
            {
                _Shield.PushE = true;
            }
            else
                return;
        }
        if (EnemyCount != 0 && _OwnBehavior.CurrentHealth <= _OwnBehavior.BehaviorParameters.MaxHealth *0.3)
        {
            if(_Shield.CanPushE == true)
            {
                _Shield.PushE = true;
            }
        }

    }

    //往主堡方向逃跑
    private void RunAway()
    {
        _Target = TeamCastlePosition;
        
        Dangerous = false;
    }

    //計算攻防權重,如果進攻權重較大則以敵方主堡為目標進攻,若防守權重較大,則以我方主堡為目標撤退
    private void AttackOrDefense()
    {
        AttackWeight = 0;
        DefenseWeight = 0;
        NextPositionWeight = 0;
        float EnemyCastleDistanceWeight = 0;
        float TeamCastleDistanceWeight = 0;

        //有以下角色,分別算出各自的數值後用防禦權重去加,血量*基礎數值 + 補數(我方為60 敵方為50)
        if (hasOwn)
            AttackWeight += (_OwnBehavior.CurrentHealth / _OwnBehavior.BehaviorParameters.MaxHealth) * 30 + 60;
        if (hasArtilleryman)
            AttackWeight += (_artillerymanBehavior.CurrentHealth / _artillerymanBehavior.BehaviorParameters.MaxHealth) * 30 + 60;
        if (hasCurer)
            AttackWeight += (_CurerBehavior.CurrentHealth / _CurerBehavior.BehaviorParameters.MaxHealth) * 15 + 60;
        if (hasCommander)
            AttackWeight += (_CommanderBehavior.CurrentHealth / _CommanderBehavior.BehaviorParameters.MaxHealth) * 25 + 60;
        if (hasTank)
            AttackWeight -= (_TankBlood / _Tank.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 30 + 50;
        if (hasShooter)
            AttackWeight -= (_ShooterBlood / _Shooter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 30 + 50;
        if (hasSummoner)
            AttackWeight -= (_SummonerBlood / _Summoner.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 25 + 50;
        if (hasDiamondProtecter)
            AttackWeight -= (_DiamondProtecterBlood / _DiamondProtecter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 15 + 50;

        //攻擊權重加上雙方主堡距離除以(自身與敵方主堡間距除以10),值的範圍為0~200之間
        //攻擊權重加上敵方主堡消耗血量百分比乘以100
        if (EnemyCastle != null)
        {
            EnemyCastleDistanceWeight = Mathf.Sqrt(Mathf.Pow(TeamCastlePosition.position.x - EnemyCastlePosition.position.x, 2) + Mathf.Pow(TeamCastlePosition.position.y - EnemyCastlePosition.position.y, 2)) / (EnemyCastleDistance / 10);
            if (EnemyCastleDistanceWeight >= 200)
                EnemyCastleDistanceWeight = 200;
            AttackWeight += EnemyCastleDistanceWeight;
            AttackWeight += (1 - (EnemyCastleBlood / EnemyCastle.GetComponent<Health>().OriginalHealth)) * 100;
        }

        //如果指揮官接近敵方主堡範圍15內,攻擊權重+50
        if (hasCommander && EnemyCastleDistance <= 15)
            AttackWeight += 70;

        //加總後的值乘上係數0.6
        AttackWeight *= 0.6f;

        //有以下角色,分別算出各自的數值後用防禦權重去加,血量*基礎數值 + 補數(我方為60 敵方為50)
        if (!hasOwn)
            DefenseWeight += (_OwnBehavior.CurrentHealth / _OwnBehavior.BehaviorParameters.MaxHealth) * 30 + 60;
        if (!hasArtilleryman && _artillerymanBehavior != null)
            DefenseWeight += (_artillerymanBehavior.CurrentHealth / _artillerymanBehavior.BehaviorParameters.MaxHealth) * 30 + 60;
        if (!hasCommander && _Commander != null)
            DefenseWeight += (_CommanderBehavior.CurrentHealth / _CommanderBehavior.BehaviorParameters.MaxHealth) * 25 + 60;
        if (!hasCurer && _Curer != null)
            DefenseWeight += (_CurerBehavior.CurrentHealth / _CurerBehavior.BehaviorParameters.MaxHealth) * 15 + 60;
        if (hasTank)
            DefenseWeight += (_TankBlood / _Tank.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 30 + 50;
        if (hasShooter)
            DefenseWeight += (_ShooterBlood / _Shooter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 30 + 50;
        if (hasSummoner)
            DefenseWeight += (_SummonerBlood / _Summoner.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 25 + 50;
        if (hasDiamondProtecter)
            DefenseWeight += (_DiamondProtecterBlood / _DiamondProtecter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 15 + 50;

        //防禦權重加上雙方主堡距離除以(自身與我方主堡間距除以10),值的範圍為0~200之間
        //防禦權重加上主堡消耗血量百分比乘以100
        if (TeamCastle != null)
        {
            TeamCastleDistanceWeight = Mathf.Sqrt(Mathf.Pow(TeamCastlePosition.position.x - EnemyCastlePosition.position.x, 2) + Mathf.Pow(TeamCastlePosition.position.y - EnemyCastlePosition.position.y, 2)) / (TeamCastleDistance / 10);
            if (TeamCastleDistanceWeight >= 200)
                TeamCastleDistanceWeight = 200;
            DefenseWeight += TeamCastleDistanceWeight;
            DefenseWeight += (1 - (TeamCastleBlood / TeamCastle.GetComponent<Health>().OriginalHealth)) * 100;
        }

        //如果鑽石擁護者接近主堡範圍15內,防禦權重+70
        if (_DiamondProtecter != null)
        {
            if ((Mathf.Sqrt(Mathf.Pow(_DiamondProtecter.transform.position.x - TeamCastlePosition.position.x, 2) + Mathf.Pow(_DiamondProtecter.transform.position.y - TeamCastlePosition.position.y, 2))) <= 15)
                DefenseWeight += 70;
        }

        //加總後的值乘上係數0.4
        DefenseWeight *= 0.4f;



        if (AttackWeight >= DefenseWeight)
        {
            Attack = true;
            Defense = false;
        }
        else if (AttackWeight < DefenseWeight)
        {
            Attack = false;
            Defense = true;
        }

    }
}
