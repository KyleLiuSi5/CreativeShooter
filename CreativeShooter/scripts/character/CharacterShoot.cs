using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CharacterShoot : NetworkBehaviour
{
    public Weapon InitialWeapon;
    public Weapon nowWeapon;
    public Weapon TankWeapon;
    public Transform WeaponAttachment;
    private Vector3 WeaponLocation;
    public bool EightDirectionShooting = true;
    public bool StrictEightDirectionShooting = true;


    private Weapon _weapon;
    private Tank _tank;
    private YTT _SwitchWeapon;
    public RRRRR _RRRRR;
    public C87666 _C87666;
    public Wheel _Wheel;
    private float _fireTimer;

    private float _horizontalMove;
    private float _verticalMove;

    private CharacterBehavior _characterBehavior;
    private CorgiController _controller;
    public playerScript _playersc;

    void Start()
    {

        _characterBehavior = GetComponent<CharacterBehavior>();
        _controller = GetComponent<CorgiController>();
        _tank = GetComponent<Tank>();
        _SwitchWeapon = GetComponent<YTT>();
        _RRRRR = GetComponent<RRRRR>();
        _playersc = GetComponent<playerScript>();
        _C87666 = GetComponent<C87666>();
        _Wheel = GetComponent<Wheel>();

        if (WeaponAttachment == null)
            WeaponAttachment = transform;
        ChangeWeapon(InitialWeapon);
    }

    public void ShootOnce()
    {
        if (!_characterBehavior.Permissions.ShootEnabled || _characterBehavior.BehaviorState.IsDead)
            return;
        if (!_characterBehavior.BehaviorState.CanShoot)
        {
            _characterBehavior.BehaviorState.FiringDirection = 3;
            return;
        }

        if (!_characterBehavior.BehaviorState.CanMoveFreely)
            return;

        CmdFireProjectile();
        _fireTimer = 0;
    }

    public void ShootStart()
    {
        if (!_characterBehavior.Permissions.ShootEnabled || _characterBehavior.BehaviorState.IsDead)
            return;
        if (!_characterBehavior.BehaviorState.CanShoot)
        {
            _characterBehavior.BehaviorState.FiringDirection = 3;
            return;
        }

        if (!_characterBehavior.BehaviorState.CanMoveFreely)
            return;

        _characterBehavior.BehaviorState.FiringStop = false;
        _characterBehavior.BehaviorState.Firing = true;


        _fireTimer += Time.deltaTime;
        if (_fireTimer > _weapon.FireRate)
        {
            CmdFireProjectile();
            _fireTimer = 0;
        }
    }

    [Command]
    public void CmdShootStop()
    {
        RpcShootStop();
    }
    [ClientRpc]
    public void RpcShootStop()
    {
        if (!_characterBehavior.Permissions.ShootEnabled)
            return;
        if (!_characterBehavior.BehaviorState.CanShoot)
        {
            _characterBehavior.BehaviorState.FiringDirection = 3;
            return;
        }
        _characterBehavior.BehaviorState.FiringStop = true;
        _characterBehavior.BehaviorState.Firing = false;
        _characterBehavior.BehaviorState.FiringDirection = 3;
        _weapon.GunFlames.enableEmission = false;
        _weapon.GunShells.enableEmission = false;
    }

    
    public void ChangeWeapon(Weapon newWeapon)
    {
        if (_weapon != null)
        {
            CmdShootStop();
        }
        if(nowWeapon != null)
        {
            Destroy(nowWeapon.gameObject);
        }
        _weapon = (Weapon)Instantiate(newWeapon, WeaponAttachment.transform.position, WeaponAttachment.transform.rotation);
        nowWeapon = _weapon;
        _weapon.transform.parent = transform;
        _weapon.SetGunFlamesEmission(false);
        _weapon.SetGunShellsEmission(false);
    }

    [Command]
    public void CmdFireProjectile()
    {
        RpcFireProjectile();
    }
    [ClientRpc]
    public void RpcFireProjectile()
    {
        if (gameObject.tag != "Daze")
        {
            float HorizontalShoot = _horizontalMove;
            float VerticalShoot = _verticalMove;

            if (_weapon.ProjectileFireLocation == null)
                return;

            if (!EightDirectionShooting)
            {
                HorizontalShoot = 0;
                VerticalShoot = 0;
            }

            if (StrictEightDirectionShooting)
            {
                HorizontalShoot = Mathf.Round(HorizontalShoot);
                VerticalShoot = Mathf.Round(VerticalShoot);
            }

            float angle = Mathf.Atan2(HorizontalShoot, VerticalShoot) * Mathf.Rad2Deg;

            Vector2 direction = Vector2.up;

            if (HorizontalShoot > -0.1f && HorizontalShoot < 0.1f && VerticalShoot > -0.1f && VerticalShoot < 0.1f)
            {
                bool _isFacingRight = transform.localScale.x > 0;
                angle = _isFacingRight ? 90f : -90f;

            }

            if (Mathf.Abs(HorizontalShoot) < 0.1f && VerticalShoot > 0.1f)
                _characterBehavior.BehaviorState.FiringDirection = 1;
            if (Mathf.Abs(HorizontalShoot) > 0.1f && VerticalShoot > 0.1f)
                _characterBehavior.BehaviorState.FiringDirection = 2;
            if (Mathf.Abs(HorizontalShoot) > 0.1f && VerticalShoot < -0.1f)
                _characterBehavior.BehaviorState.FiringDirection = 4;
            if (Mathf.Abs(HorizontalShoot) < 0.1f && VerticalShoot < -0.1f)
                _characterBehavior.BehaviorState.FiringDirection = 5;
            if (Mathf.Abs(VerticalShoot) < 0.1f)
                _characterBehavior.BehaviorState.FiringDirection = 3;

            bool _facingRight = transform.localScale.x > 0;
            float horizontalDirection = _facingRight ? 1f : -1f;

            direction = Quaternion.Euler(0, 0, -angle) * direction;

            _weapon.GunRotationCenter.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle * horizontalDirection + 90f));

            if (_tank != null && _playersc.SwitchT == 1)
            {
                _weapon.FireRate = 1f;
                WeaponLocation.x = _weapon.ProjectileFireLocation.position.x;
                WeaponLocation.y = _weapon.ProjectileFireLocation.position.y + 1;
                WeaponLocation.z = _weapon.ProjectileFireLocation.position.z;
                var projectile1 = (Projectile)Instantiate(_weapon.Projectile, WeaponLocation, _weapon.ProjectileFireLocation.rotation);
                projectile1.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }
            else if (_tank != null && _playersc.SwitchT == 0)
            {
                _weapon.FireRate = 0.2f;
                var projectile = (Projectile)Instantiate(_weapon.Projectile, _weapon.ProjectileFireLocation.position, _weapon.ProjectileFireLocation.rotation);
                projectile.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }
            else if (_SwitchWeapon != null && _playersc.Switchi == 1)
            {
                _weapon.FireRate = 1f;
                Vector3 WeaponLocation1;
                Vector3 WeaponLocation2;
                Vector3 WeaponLocation3;
                Vector3 WeaponLocation4;
                Vector3 WeaponLocation5;
                Vector3 WeaponLocation6;
                WeaponLocation.x = _weapon.ProjectileFireLocation.position.x;
                WeaponLocation.y = _weapon.ProjectileFireLocation.position.y;
                WeaponLocation.z = _weapon.ProjectileFireLocation.position.z;
                var projectile1 = (Projectile)Instantiate(_weapon.Projectile, WeaponLocation, _weapon.ProjectileFireLocation.rotation);
                projectile1.Initialize(gameObject, direction, _controller.Speed);              
                WeaponLocation1.x = _weapon.ProjectileFireLocation.position.x;
                WeaponLocation1.y = _weapon.ProjectileFireLocation.position.y + 0.5f;
                WeaponLocation1.z = _weapon.ProjectileFireLocation.position.z;
                var projectile2 = (Projectile)Instantiate(_weapon.Projectile, WeaponLocation1, _weapon.ProjectileFireLocation.rotation);
                projectile2.Initialize(gameObject, direction, _controller.Speed);
                WeaponLocation2.x = _weapon.ProjectileFireLocation.position.x;
                WeaponLocation2.y = _weapon.ProjectileFireLocation.position.y + 1;
                WeaponLocation2.z = _weapon.ProjectileFireLocation.position.z;
                var projectile3 = (Projectile)Instantiate(_weapon.Projectile, WeaponLocation2, _weapon.ProjectileFireLocation.rotation);
                projectile3.Initialize(gameObject, direction, _controller.Speed);
                WeaponLocation3.x = _weapon.ProjectileFireLocation.position.x;
                WeaponLocation3.y = _weapon.ProjectileFireLocation.position.y + 1.5f;
                WeaponLocation3.z = _weapon.ProjectileFireLocation.position.z;
                var projectile4 = (Projectile)Instantiate(_weapon.Projectile, WeaponLocation3, _weapon.ProjectileFireLocation.rotation);
                projectile4.Initialize(gameObject, direction, _controller.Speed);
                WeaponLocation4.x = _weapon.ProjectileFireLocation.position.x + 1;
                WeaponLocation4.y = _weapon.ProjectileFireLocation.position.y + 0.25f;
                WeaponLocation4.z = _weapon.ProjectileFireLocation.position.z;
                var projectile5 = (Projectile)Instantiate(_weapon.Projectile, WeaponLocation4, _weapon.ProjectileFireLocation.rotation);
                projectile5.Initialize(gameObject, direction, _controller.Speed);
                WeaponLocation5.x = _weapon.ProjectileFireLocation.position.x + 1;
                WeaponLocation5.y = _weapon.ProjectileFireLocation.position.y + 0.75f;
                WeaponLocation5.z = _weapon.ProjectileFireLocation.position.z;
                var projectile6 = (Projectile)Instantiate(_weapon.Projectile, WeaponLocation5, _weapon.ProjectileFireLocation.rotation);
                projectile6.Initialize(gameObject, direction, _controller.Speed);
                WeaponLocation6.x = _weapon.ProjectileFireLocation.position.x + 1;
                WeaponLocation6.y = _weapon.ProjectileFireLocation.position.y + 1.25f;
                WeaponLocation6.z = _weapon.ProjectileFireLocation.position.z;
                var projectile7 = (Projectile)Instantiate(_weapon.Projectile, WeaponLocation6, _weapon.ProjectileFireLocation.rotation);
                projectile7.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }
            else if (_SwitchWeapon != null && _playersc.Switchi == 0)
            {
                _weapon.FireRate = 0.2f;
                var projectile = (Projectile)Instantiate(_weapon.Projectile, _weapon.ProjectileFireLocation.position, _weapon.ProjectileFireLocation.rotation);
                projectile.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }
            else if(_RRRRR != null && _playersc.Switchj == 0)
            {
                _weapon.FireRate = 0.2f;
                var projectile = (Projectile)Instantiate(_weapon.Projectile, _weapon.ProjectileFireLocation.position, _weapon.ProjectileFireLocation.rotation);
                projectile.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }
            else if (_RRRRR != null && _playersc.Switchj == 1)
            {
                _weapon.FireRate = 0.02f;
                var projectile = (Projectile)Instantiate(_weapon.Projectile, _weapon.ProjectileFireLocation.position, _weapon.ProjectileFireLocation.rotation);
                projectile.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }
            else if(_C87666 != null && _playersc.SwitchC == 1)
            {
                _weapon.FireRate = 5f;
                var projectile = (Projectile)Instantiate(_weapon.Projectile, _weapon.ProjectileFireLocation.position, _weapon.ProjectileFireLocation.rotation);
                projectile.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }
            else if(_C87666 != null && _playersc.SwitchC == 0)
            {
                _weapon.FireRate = 0.2f;
                var projectile = (Projectile)Instantiate(_weapon.Projectile, _weapon.ProjectileFireLocation.position, _weapon.ProjectileFireLocation.rotation);
                projectile.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }
            else if(_Wheel != null && _playersc.SwitchW == 1)
            {
                _weapon.FireRate = 5f;
                var projectile = (Projectile)Instantiate(_weapon.Projectile, _weapon.ProjectileFireLocation.position, _weapon.ProjectileFireLocation.rotation);
                projectile.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }
            else if(_Wheel != null && _playersc.SwitchW == 0)
            {
                _weapon.FireRate = 0.2f;
                var projectile = (Projectile)Instantiate(_weapon.Projectile, _weapon.ProjectileFireLocation.position, _weapon.ProjectileFireLocation.rotation);
                projectile.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }
            else
            {
                _weapon.FireRate = 0.2f;
                var projectile = (Projectile)Instantiate(_weapon.Projectile, _weapon.ProjectileFireLocation.position, _weapon.ProjectileFireLocation.rotation);
                projectile.Initialize(gameObject, direction, _controller.Speed);
                _weapon.SetGunFlamesEmission(true);
                _weapon.SetGunShellsEmission(true);
            }

        }
    }


    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
    {

        angle = angle * (Mathf.PI / 180f);
        var rotatedX = Mathf.Cos(angle) * (point.x - pivot.x) - Mathf.Sin(angle) * (point.y - pivot.y) + pivot.x;
        var rotatedY = Mathf.Sin(angle) * (point.x - pivot.x) + Mathf.Cos(angle) * (point.y - pivot.y) + pivot.y;
        return new Vector3(rotatedX, rotatedY, 0);
    }

    public void SetHorizontalMove(float value)
    {
        _horizontalMove = value;
    }


    public void SetVerticalMove(float value)
    {
        _verticalMove = value;
    }


    [Command]
    public void CmdFlip()
    {
        RpcFlip();
    }
    [ClientRpc]
    public void RpcFlip()
    {
        if (_weapon.GunShells != null)
            _weapon.GunShells.transform.eulerAngles = new Vector3(_weapon.GunShells.transform.eulerAngles.x, _weapon.GunShells.transform.eulerAngles.y + 180, _weapon.GunShells.transform.eulerAngles.z);
        if (_weapon.GunFlames != null)
            _weapon.GunFlames.transform.eulerAngles = new Vector3(_weapon.GunFlames.transform.eulerAngles.x, _weapon.GunFlames.transform.eulerAngles.y + 180, _weapon.GunFlames.transform.eulerAngles.z);

    }


}
