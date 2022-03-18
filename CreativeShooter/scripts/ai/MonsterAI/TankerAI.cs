using UnityEngine;
using System.Collections;

public class TankerAI : MonoBehaviour {

    //宣告場上會出現的4種human方角色以及3種Monster方角色
    public GameObject _Summoner;
    public GameObject _DiamondProtecter;
    public GameObject _Shooter;
    public GameObject _Charge;
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
    public bool hasSummoner = true;
    public bool hasDiamondProtecter = true;
    public bool hasShooter = true;
    public bool hasCharge = false;
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
    public bool useDudu = false;
    public bool useRRRR = false;
    public bool useC8C8 = false;
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
    public float _ChargeBlood;
    public float _artillerymanBlood;
    public float _CommanderBlood;
    public float _CurerBlood;
    //敵方最後出現時的血量
    public float _ChargeLastBlood;
    public float _artillerymanLastBlood;
    public float _CommanderLastBlood;
    public float _CurerLastBlood;
    //我方血量
    public float _SummonerBlood;
    public float _DiamondProtecterBlood;
    public float _ShooterBlood;
    //定義權重
    public float Weight = 0f;
    //定義遊戲時間
    public float GameTime = 0f;
    //定義敵方最後看到的時間
    public float _ChargeLastSeeTime;
    public float _artillerymanLastSeeTime;
    public float _CommanderLastSeeTime;
    public float _CurerLastSeeTime;
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
    public Transform _ChargePosition;
    public Transform _artillerymanPosition;
    public Transform _CommanderPosition;
    public Transform _CurerPosition;
    //我方角色位置
    public Transform _SummonerPosition;
    public Transform _DiamondProtecterPosition;
    public Transform _ShooterPosition;
    public Transform _OwnPosition;
    //敵方角色最後位置
    public Transform _ChargeLastPosition;
    public Transform _artillerymanLastPosition;
    public Transform _CommanderLastPosition;
    public Transform _CurerLastPosition;
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

    //可獲取Monster方角色狀態
    public CharacterBehavior _SummonerBehavior;
    public CharacterBehavior _DiamondProtecterBehavior;
    public CharacterBehavior _ShooterBehavior;
    public CharacterBehavior _OwnBehavior;
    private CorgiController _Controller;
    private CharacterShoot _AIShoot;
    private CharacterJetpack _JetPack;
    private Camera _MainCamera;
    private Unit _Astar;
    //Summoner的技能
    private dudu _Dudu;//兄弟們硬起來
    private RRRRR _RRRR;//RRRR
    private c8c8 _C8C8;//C8763

    //判斷A*的下一步是不是可以行走的位置
    private TargetEnter _CantWalk;


    // Use this for initialization
    void Start()
    {

        if (GameObject.Find("Summoner") != null)
        {
            _Summoner = GameObject.Find("Summoner");
            _SummonerBehavior = _Summoner.GetComponent<CharacterBehavior>();
            _SummonerPosition = _Summoner.transform;
            _SummonerBlood = _Summoner.GetComponent<CharacterBehavior>().CurrentHealth;
        }
        else if (GameObject.Find("Summoner(Clone)") != null)
        {
            _Summoner = GameObject.Find("Summoner(Clone)");
            _SummonerBehavior = _Summoner.GetComponent<CharacterBehavior>();
            _SummonerPosition = _Summoner.transform;
            _SummonerBlood = _Summoner.GetComponent<CharacterBehavior>().CurrentHealth;
        }

        if (GameObject.Find("DiamondProtecter") != null)
        {
            _DiamondProtecter = GameObject.Find("DiamondProtecter");
            _DiamondProtecterBehavior = _DiamondProtecter.GetComponent<CharacterBehavior>();
            _DiamondProtecterPosition = _DiamondProtecter.transform;
            _DiamondProtecterBlood = _DiamondProtecter.GetComponent<CharacterBehavior>().CurrentHealth;
        }
        else if (GameObject.Find("DiamondProtecter(Clone)") != null)
        {
            _DiamondProtecter = GameObject.Find("DiamondProtecter(Clone)");
            _DiamondProtecterBehavior = _DiamondProtecter.GetComponent<CharacterBehavior>();
            _DiamondProtecterPosition = _DiamondProtecter.transform;
            _DiamondProtecterBlood = _DiamondProtecter.GetComponent<CharacterBehavior>().CurrentHealth;
        }

        if (GameObject.Find("Shooter") != null)
        {
            _Shooter = GameObject.Find("Shooter");
            _ShooterBehavior = _Shooter.GetComponent<CharacterBehavior>();
            _ShooterPosition = _Shooter.transform;
            _ShooterBlood = _Shooter.GetComponent<CharacterBehavior>().CurrentHealth;
        }
        else if (GameObject.Find("Shooter(Clone)") != null)
        {
            _Shooter = GameObject.Find("Shooter(Clone)");
            _ShooterBehavior = _Shooter.GetComponent<CharacterBehavior>();
            _ShooterPosition = _Shooter.transform;
            _ShooterBlood = _Shooter.GetComponent<CharacterBehavior>().CurrentHealth;
        }

        if (GameObject.Find("Charge") != null)
        {
            _Charge = GameObject.Find("Charge");
            _ChargeBlood = _Charge.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }
        else if (GameObject.Find("Charge(Clone)") != null)
        {
            _Charge = GameObject.Find("Charge(Clone)");
            _ChargeBlood = _Charge.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }

        if (GameObject.Find("artilleryman") != null)
        {
            _artilleryman = GameObject.Find("artilleryman");
            _artillerymanBlood = _artilleryman.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }
        else if (GameObject.Find("artilleryman(Clone)") != null)
        {
            _artilleryman = GameObject.Find("artilleryman(Clone)");
            _artillerymanBlood = _artilleryman.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }

        if (GameObject.Find("Curer") != null)
        {
            _Curer = GameObject.Find("Curer");
            _CurerBlood = _Curer.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }
        else if (GameObject.Find("Curer(Clone)") != null)
        {
            _Curer = GameObject.Find("Curer(Clone)");
            _CurerBlood = _Curer.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }

        if (GameObject.Find("Commander") != null)
        {
            _Commander = GameObject.Find("Commander");
            _CommanderBlood = _Commander.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }
        else if (GameObject.Find("Commander(Clone)") != null)
        {
            _Commander = GameObject.Find("Commander(Clone)");
            _CommanderBlood = _Commander.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
        }

        if (GameObject.Find("HumanCastle") != null)
        {
            EnemyCastle = GameObject.Find("HumanCastle");
            EnemyCastleBlood = EnemyCastle.GetComponent<Health>().OriginalHealth;
            EnemyCastlePosition = EnemyCastle.transform;
        }
        if (GameObject.Find("MonsterCastle") != null)
        {
            TeamCastle = GameObject.Find("MonsterCastle");
            TeamCastleBlood = TeamCastle.GetComponent<Health>().OriginalHealth;
            TeamCastlePosition = TeamCastle.transform;
        }

        _OwnBehavior = gameObject.GetComponent<CharacterBehavior>();
        _OwnPosition = gameObject.transform;
        _Controller = GetComponent<CorgiController>();
        _Dudu = GetComponent<dudu>();
        _RRRR = GetComponent<RRRRR>();
        _C8C8 = GetComponent<c8c8>();
        _AIShoot = GetComponent<CharacterShoot>();
        _JetPack = GetComponent<CharacterJetpack>();
        CurrentHealth = _OwnBehavior.BehaviorParameters.MaxHealth;
        _Astar = GetComponent<Unit>();
        RunAwayTime = 5f;
        FollowTeam = true;
        Team = GameObject.FindGameObjectsWithTag("Enemy");
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
            _Astar.target = EnemyCastlePosition;
            _Astar.FindRoad();
        }
        if (Defense == true)//如果權重為防守,則向我方主堡位置逃跑
        {
            FireWithEnemy = false;
            _Astar.target = TeamCastlePosition;
            _Astar.FindRoad();
        }
        if (_OwnBehavior.isMoving == false && FireWithEnemy == false)//如果沒有在移動並且沒有在與敵方交火,則重新計算攻防權重
        {
            _Astar.targetIndex = 0;
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
        _Astar.targetIndex = 0;//將路線歸零
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
                _Astar.target = _target.transform;
                _Astar.FindRoad();


            }
            else if (NearestEnemy.transform.position.x < _OwnPosition.position.x)
            {
                var GoodPosX = NearestEnemy.transform.position.x + SafeDistance;
                var GoodPosY = NearestEnemy.transform.position.y;
                var GoodPosZ = gameObject.transform.position.z;
                var GoodPos = new Vector3(GoodPosX, GoodPosY, GoodPosZ);
                _target.transform.position = GoodPos;
                _Astar.target = _target.transform;
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
                _Astar.target = Team[j].transform;
            }

        }
        _Astar.FindRoad();
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


        //角色的位置在Summoner的可視範圍內並且血量不低於自身的10%,可將之視為可戰鬥人員並且記錄角色
        if (_Summoner != null)
        {
            if (_Summoner.transform.position.x < (gameObject.transform.position.x + TeamDistance) && _Summoner.transform.position.x > (gameObject.transform.position.x - TeamDistance)
            && _Summoner.transform.position.y < (gameObject.transform.position.y + TeamDistance) && _Summoner.transform.position.y > (gameObject.transform.position.y - TeamDistance) && _SummonerBehavior.CurrentHealth >= _SummonerBehavior.BehaviorParameters.MaxHealth * 0.1)
            {
                TeamCount++;
                hasSummoner = true;
                _SummonerBlood = _Summoner.GetComponent<CharacterBehavior>().CurrentHealth;
            }

            else
            {
                hasSummoner = false;
                _SummonerBlood = _Summoner.GetComponent<CharacterBehavior>().CurrentHealth;
            }
        }
        else
            hasSummoner = false;


        if (_DiamondProtecter != null)
        {
            if (_DiamondProtecter.transform.position.x < (gameObject.transform.position.x + TeamDistance) && _DiamondProtecter.transform.position.x > (gameObject.transform.position.x - TeamDistance)
            && _DiamondProtecter.transform.position.y < (gameObject.transform.position.y + TeamDistance) && _DiamondProtecter.transform.position.y > (gameObject.transform.position.y - TeamDistance) && _DiamondProtecter.GetComponent<CharacterBehavior>().CurrentHealth > _DiamondProtecter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth * 0.1)
            {
                TeamCount++;
                hasDiamondProtecter = true;
                _DiamondProtecterBlood = _DiamondProtecter.GetComponent<CharacterBehavior>().CurrentHealth;
            }

            else
            {
                hasDiamondProtecter = false;
                _DiamondProtecterBlood = _DiamondProtecter.GetComponent<CharacterBehavior>().CurrentHealth;
            }
        }
        else
            hasDiamondProtecter = false;

        if (_Shooter != null)
        {

            if (_Shooter.transform.position.x < (gameObject.transform.position.x + TeamDistance) && _Shooter.transform.position.x > (gameObject.transform.position.x - TeamDistance)
            && _Shooter.transform.position.y < (gameObject.transform.position.y + TeamDistance) && _Shooter.transform.position.y > (gameObject.transform.position.y - TeamDistance) && _ShooterBehavior.CurrentHealth > _ShooterBehavior.BehaviorParameters.MaxHealth * 0.1)
            {
                TeamCount++;
                hasShooter = true;
                _ShooterBlood = _Shooter.GetComponent<CharacterBehavior>().CurrentHealth;

            }

            else
            {
                hasShooter = false;
                _ShooterBlood = _Shooter.GetComponent<CharacterBehavior>().CurrentHealth;

            }
        }
        else
            hasShooter = false;


        //若敵方出現在可視距離內則敵方角色數量+1並且記錄出現的角色,獲取對方血量以及歸零最後看到時間
        if (_Charge != null)
        {
            if (_Charge.transform.position.x < (gameObject.transform.position.x + SightDistance) && _Charge.transform.position.x > (gameObject.transform.position.x - SightDistance)
           && _Charge.transform.position.y < (gameObject.transform.position.y + SightDistance) && _Charge.transform.position.y > (gameObject.transform.position.y - SightDistance))
            {
                EnemyCount++;
                hasCharge = true;
                _ChargeBlood = _Charge.GetComponent<CharacterBehavior>().CurrentHealth;
                _ChargePosition = _Charge.transform;
                _ChargeLastPosition = _ChargePosition;
                _ChargeLastSeeTime = 0;
                if (NearestDistance <= Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _ChargePosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _ChargePosition.position.y, 2)))
                {
                    NearestDistance = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _ChargePosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _ChargePosition.position.y, 2));
                    NearestEnemy = _Charge;
                }
            }

            else
            {
                _ChargeLastSeeTime += Time.deltaTime;
                hasCharge = false;
                _ChargeLastBlood = _ChargeBlood;
                _ChargePosition = null;
                if (_ChargeLastSeeTime > 5)
                {
                    _ChargeLastBlood = _Charge.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
                    _ChargeLastPosition = null;
                }
            }
        }

        if (_artilleryman != null)
        {
            if (_artilleryman.transform.position.x < (gameObject.transform.position.x + SightDistance) && _artilleryman.transform.position.x > (gameObject.transform.position.x - SightDistance)
          && _artilleryman.transform.position.y < (gameObject.transform.position.y + SightDistance) && _artilleryman.transform.position.y > (gameObject.transform.position.y - SightDistance))
            {
                EnemyCount++;
                hasArtilleryman = true;
                _artillerymanBlood = _artilleryman.GetComponent<CharacterBehavior>().CurrentHealth;
                _artillerymanPosition = _artilleryman.transform;
                _artillerymanLastPosition = _artillerymanPosition;
                _artillerymanLastSeeTime = 0;
                if (NearestDistance <= Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _artillerymanPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _artillerymanPosition.position.y, 2)))
                {
                    NearestDistance = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _artillerymanPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _artillerymanPosition.position.y, 2));
                    NearestEnemy = _artilleryman;
                }
            }

            else
            {
                _artillerymanLastSeeTime += Time.deltaTime;
                hasArtilleryman = false;
                _artillerymanLastBlood = _artillerymanBlood;
                _artillerymanPosition = null;
                if (_artillerymanLastSeeTime > 5)
                {
                    _artillerymanLastBlood = _artilleryman.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
                    _artillerymanLastPosition = null;
                }
            }
        }

        if (_Curer != null)
        {
            if (_Curer.transform.position.x < (gameObject.transform.position.x + SightDistance) && _Curer.transform.position.x > (gameObject.transform.position.x - SightDistance)
           && _Curer.transform.position.y < (gameObject.transform.position.y + SightDistance) && _Curer.transform.position.y > (gameObject.transform.position.y - SightDistance))
            {
                EnemyCount++;
                hasCurer = true;
                _CurerBlood = _Curer.GetComponent<CharacterBehavior>().CurrentHealth;
                _CurerPosition = _Curer.transform;
                _CurerLastPosition = _CurerPosition;
                _CurerLastSeeTime = 0;
                if (NearestDistance <= Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _CurerPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _CurerPosition.position.y, 2)))
                {
                    NearestDistance = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _CurerPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _CurerPosition.position.y, 2));
                    NearestEnemy = _Curer;
                }
            }

            else
            {
                _CurerLastSeeTime += Time.deltaTime;
                hasCurer = false;
                _CurerLastBlood = _CurerBlood;
                _CurerPosition = null;
                if (_CurerLastSeeTime > 5)
                {
                    _CurerLastBlood = _Curer.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
                    _CurerLastPosition = null;
                }
            }
        }

        if (_Commander != null)
        {
            if (_Commander.transform.position.x < (gameObject.transform.position.x + SightDistance) && _Commander.transform.position.x > (gameObject.transform.position.x - SightDistance)
          && _Commander.transform.position.y < (gameObject.transform.position.y + SightDistance) && _Commander.transform.position.y > (gameObject.transform.position.y - SightDistance))
            {
                EnemyCount++;
                hasCommander = true;
                _CommanderBlood = _Commander.GetComponent<CharacterBehavior>().CurrentHealth;
                _CommanderPosition = _Commander.transform;
                _CommanderLastPosition = _CommanderPosition;
                _CommanderLastSeeTime = 0;
                if (NearestDistance <= Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _CommanderPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _CommanderPosition.position.y, 2)))
                {
                    NearestDistance = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - _CommanderPosition.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - _CommanderPosition.position.y, 2));
                    NearestEnemy = _Commander;
                }
            }

            else
            {
                _CommanderLastSeeTime += Time.deltaTime;
                hasCommander = false;
                _CommanderLastBlood = _CommanderBlood;
                _CommanderPosition = null;
                if (_CommanderLastSeeTime > 5)
                {
                    _CommanderLastBlood = _Commander.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth;
                    _CommanderLastPosition = null;
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
        //當自身血量小於自身最大血量的1/10,可以執行少數行動
        /*if (CurrentHealth < _OwnBehavior.BehaviorParameters.MaxHealth * 0.2 && RunAwayTime == 0)
        {
            UseSkill();
        }*/
        //if (Attack == true)
        //  UseSkill();
        if (FireWithEnemy == true)

            UseSkill();
    }

    //AI施放技能
    private void UseSkill()
    {
        if (CurrentHealth < _OwnBehavior.BehaviorParameters.MaxHealth * 0.2 && EnemyCount >= 1)
        {
            if (_C8C8.CanPushT == true)
            {
                _C8C8.PushT = true;
                _C8C8.c8c8CDTime = 0;
                _C8C8.CanPushT = false;
                RunAwayTime = 1;
            }
            else if (_C8C8.CanPushT == false && _RRRR.CanPushR == true)
            {
                gameObject.transform.position = TeamCastlePosition.position;
                _RRRR.CanPushR = false;
                _RRRR.RRRRRCDTime = 0;
                RunAwayTime = 1;
            }
            else if (_RRRR.CanPushR == false && _C8C8.CanPushT == false && _Dudu.CanPushE == true)
            {
                _Dudu.PushE = true;
                _Dudu.CanPushE = false;
                _Dudu.duduCDTime = 0;
                RunAwayTime = 1;
            }
            else if (_C8C8.CanPushT == false && _RRRR.CanPushR == false && _Dudu.CanPushE == false)
                return;
        }

    }

    //往主堡方向逃跑
    private void RunAway()
    {
        _Astar.target = TeamCastlePosition;
        _Astar.FindRoad();
        Dangerous = false;
        if (_Astar.targetIndex >= _Astar.path.Length)
        {
            DangerousAgain = false;
        }
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
        if (hasSummoner)
            AttackWeight += (_SummonerBehavior.CurrentHealth / _SummonerBehavior.BehaviorParameters.MaxHealth) * 30 + 60;
        if (hasShooter)
            AttackWeight += (_ShooterBehavior.CurrentHealth / _ShooterBehavior.BehaviorParameters.MaxHealth) * 30 + 60;
        if (hasDiamondProtecter)
            AttackWeight += (_DiamondProtecter.GetComponent<CharacterBehavior>().CurrentHealth / _DiamondProtecter.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 25 + 60;
        if (hasOwn)
            AttackWeight += (_OwnBehavior.CurrentHealth / _OwnBehavior.BehaviorParameters.MaxHealth) * 15 + 60;
        if (hasCharge)
            AttackWeight -= (_ChargeBlood / _Charge.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 30 + 50;
        if (hasArtilleryman)
            AttackWeight -= (_artillerymanBlood / _artilleryman.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 30 + 50;
        if (hasCommander)
            AttackWeight -= (_CommanderBlood / _Commander.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 25 + 50;
        if (hasCurer)
            AttackWeight -= (_CurerBlood / _Curer.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 15 + 50;

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

        //如果鑽石守護者接近敵方主堡範圍25內,攻擊權重+50
        if (hasOwn && EnemyCastleDistance <= 15)
            AttackWeight += 70;

        //加總後的值乘上係數0.6
        AttackWeight *= 0.6f;

        //有以下角色,分別算出各自的數值後用防禦權重去加,血量*基礎數值 + 補數(我方為60 敵方為50)
        if (!hasSummoner && _Summoner != null)
            DefenseWeight += (_SummonerBehavior.CurrentHealth / _SummonerBehavior.BehaviorParameters.MaxHealth) * 30 + 60;
        if (!hasShooter && _Shooter != null)
            DefenseWeight += (_ShooterBehavior.CurrentHealth / _ShooterBehavior.BehaviorParameters.MaxHealth) * 30 + 60;
        if (!hasDiamondProtecter && _DiamondProtecter != null)
            DefenseWeight += (_DiamondProtecterBehavior.CurrentHealth / _DiamondProtecterBehavior.BehaviorParameters.MaxHealth) * 25 + 60;
        if (!hasOwn)
            DefenseWeight += (_OwnBehavior.CurrentHealth / _OwnBehavior.BehaviorParameters.MaxHealth) * 15 + 60;
        if (hasCharge)
            DefenseWeight += (_ChargeBlood / _Charge.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 30 + 50;
        if (hasArtilleryman)
            DefenseWeight += (_artillerymanBlood / _artilleryman.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 30 + 50;
        if (hasCommander)
            DefenseWeight += (_CommanderBlood / _Commander.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 25 + 50;
        if (hasCurer)
            DefenseWeight += (_CurerBlood / _Curer.GetComponent<CharacterBehavior>().BehaviorParameters.MaxHealth) * 15 + 50;

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

        //如果指揮官接近主堡範圍25內,防禦權重+70
        if (_Commander != null)
        {
            if ((Mathf.Sqrt(Mathf.Pow(_Commander.transform.position.x - TeamCastlePosition.position.x, 2) + Mathf.Pow(_Commander.transform.position.y - TeamCastlePosition.position.y, 2))) <= 15)
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
