using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SimpleProjectile : Projectile, CanTakeDamage
{
	/// the amount of damage the projectile inflicts
	public int Damage;
    public float Change;
	/// the effect to instantiate when the projectile gets destroyed
	public GameObject DestroyedEffect;
	/// the amount of points to give the player when destroyed
	public int PointsToGiveToPlayer;	
	/// the lifetime of the projectile
	public float TimeToLive;
    public GameObject UpUpcollider;


	public void Update () 
	{
		// if true, we destroy it
		if ((TimeToLive -= Time.deltaTime) <= 0)
		{
			DestroyProjectile();
			return;
		}
		// we move the projectile
		transform.Translate(Direction * ((Mathf.Abs (InitialVelocity.x)+Speed) * Time.deltaTime),Space.World);

	}	

    [Command]
	public void CmdTakeDamage(int damage, GameObject instigator)
	{
        RpcTakeDamage(damage , instigator);
	}
    [ClientRpc]
    public void RpcTakeDamage(int damage, GameObject instigator)
    {
        DestroyProjectile();
    }

	
	protected override void OnCollideOther(Collider2D collider)
	{
        DestroyProjectile();
    }



    protected override void OnCollideTakeDamage(Collider2D collider, CanTakeDamage takeDamage)
	{

            takeDamage.CmdTakeDamage(Damage, gameObject);
            DestroyProjectile();
        
	}

    protected override void OnCollideThorns(Collider2D OwnerCollider, CanTakeDamage GetHurt)
    {
        
        GetHurt.CmdTakeDamage(Damage, gameObject);
        DestroyProjectile();
    }

    protected override void OnCollideUpUp()
    {
        if(Change == 0)
        {
            Change = Damage * 0.75f;
            Damage = (int)Change;
        }       
    }


    private void DestroyProjectile()
	{
		if (DestroyedEffect!=null)
		{
			Instantiate(DestroyedEffect,transform.position,transform.rotation);
		}
		
		Destroy(gameObject);
	}
}
