using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Camera))]

public class CameraController : MonoBehaviour 
{
	
	public bool FollowsPlayer{get;set;}
	
	[Space(10)]	
	[Header("Distances")]
	public float HorizontalLookDistance = 3;
	public float VerticalLookDistance = 3;
	public float LookAheadTrigger = 0.1f;
	
	
	[Space(10)]	
	[Header("Movement Speed")]
	public float ResetSpeed = 0.5f;
	public float CameraSpeed = 0.3f;
	
	[Space(10)]	
	[Header("Camera Zoom")]
	[Range (1, 20)]
	public float MinimumZoom=5f;
	[Range (1, 20)]
	public float MaximumZoom=10f;	
	public float ZoomSpeed=0.4f;
	
	
	public Transform _target;
	private CorgiController _targetController;
	private LevelLimits _levelBounds;
	
	private float xMin;
	private float xMax;
	private float yMin;
	private float yMax;	
	
	private float offsetZ;
	private Vector3 lastTargetPosition;
	private Vector3 currentVelocity;
	private Vector3 lookAheadPos;
	
	private float shakeIntensity;
	private float shakeDecay;
	private float shakeDuration;
	
	private float _currentZoom;	
	private Camera _camera;
	
	private Vector3 _lookDirectionModifier;
    private Vector3 FirePos;
    private Vector3 lastmousePosition;
    private Vector3 _FinalPos;
    private float CancelTime = 0f;
    public Collider2D _targetCollider;
    public SpeedUp _SpeedUp;
    public bool speedup { get; set; }
    private CharacterBehavior _CharacterBehavior;


    void Start ()
	{

		_camera=GetComponent<Camera>();
		FollowsPlayer=true;
		_currentZoom=MinimumZoom;
        _target = gameObject.GetComponentInParent<playerScript>().transform;
        _targetCollider = _target.GetComponent<Collider2D>();
		if (_target.GetComponent<CorgiController>()==null)
			return;
		_targetController= _target.GetComponent<CorgiController>();
		_levelBounds = GameObject.FindGameObjectWithTag("LevelBounds").GetComponent<LevelLimits>();		
		
		lastTargetPosition = _target.position;
        lastmousePosition = gameObject.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        offsetZ = _target.position.z;
		
		_lookDirectionModifier=new Vector3(0,0,0);
		
		Zoom();
        CancelTime = 0f;

        _SpeedUp = GetComponent<SpeedUp>();
        _CharacterBehavior = _target.GetComponent<CharacterBehavior>();

    }
	

	void LateUpdate () 
	{
        CameraFollow();       
    }

    public void CameraFollow()
    {
        if (!FollowsPlayer)
            return;

        Zoom();

        

        Vector3 aheadTargetPos = _target.position;
        _FinalPos.x = _target.position.x;
        _FinalPos.y = _target.position.y;
        _FinalPos.z = _target.position.z - 5;
        Vector3 newCameraPosition = Vector3.SmoothDamp(_FinalPos, aheadTargetPos, ref currentVelocity, CameraSpeed);

        Vector3 shakeFactorPosition = new Vector3(0, 0, 0);

        if (shakeDuration > 0)
        {
            shakeFactorPosition = Random.insideUnitSphere * shakeIntensity * shakeDuration;
            shakeDuration -= shakeDecay * Time.deltaTime;
        }
        newCameraPosition = newCameraPosition + shakeFactorPosition;

        float posX = Mathf.Clamp(newCameraPosition.x, xMin, xMax);
        float posY = Mathf.Clamp(newCameraPosition.y, yMin, yMax);
        float posZ = newCameraPosition.z;

        transform.position = new Vector3(posX, posY, posZ);

        lastTargetPosition = _target.position;
    }
	
	private void Zoom()
	{
	
		float characterSpeed=Mathf.Abs(_targetController.Speed.x);
		float currentVelocity=0f;
		
		_currentZoom=Mathf.SmoothDamp(_currentZoom,(characterSpeed/10)*(MaximumZoom-MinimumZoom)+MinimumZoom,ref currentVelocity,ZoomSpeed);
			
		_camera.orthographicSize=_currentZoom;
		GetLevelBounds();

        if (_SpeedUp != null && _SpeedUp.PushE == true)
        {
            _currentZoom = Mathf.SmoothDamp(_currentZoom, (characterSpeed / _CharacterBehavior.speedupdata / 10) * (MaximumZoom - MinimumZoom) + MinimumZoom, ref currentVelocity, ZoomSpeed);

            _camera.orthographicSize = _currentZoom;
            GetLevelBounds();
        }

        else if ((_SpeedUp != null && _SpeedUp.PushE == false) || _SpeedUp == null)
        {
            _currentZoom = Mathf.SmoothDamp(_currentZoom, (characterSpeed / 10) * (MaximumZoom - MinimumZoom) + MinimumZoom, ref currentVelocity, ZoomSpeed); //50那邊原本10

            _camera.orthographicSize = _currentZoom;
            GetLevelBounds();
        }

    }
	
	private void GetLevelBounds()
	{
		float cameraHeight = gameObject.GetComponent<Camera>().orthographicSize * 2f;		
		float cameraWidth = gameObject.GetComponent<Camera>().aspect;
		
		xMin = _levelBounds.LeftLimit+(cameraWidth/2);
		xMax = _levelBounds.RightLimit-(cameraWidth/2); 
		yMin = _levelBounds.BottomLimit+(cameraHeight/2); 
		yMax = _levelBounds.TopLimit-(cameraHeight/2);	
	}

	public void Shake(Vector3 shakeParameters)
	{
		shakeIntensity = shakeParameters.x;
		shakeDuration=shakeParameters.y;
		shakeDecay=shakeParameters.z;
	}


	public void LookUp()
	{
		_lookDirectionModifier = new Vector3(0,VerticalLookDistance,0);
	}	

	public void LookDown()
	{
		_lookDirectionModifier = new Vector3(0,-VerticalLookDistance,0);
	}

	public void ResetLookUpDown()
	{	
		_lookDirectionModifier = new Vector3(0,0,0);
	}
	
	
}