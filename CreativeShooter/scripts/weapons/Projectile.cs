using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public abstract class Projectile : NetworkBehaviour 
{	
	public float Speed;
	public LayerMask CollisionMask;
	public GameObject Owner {get; private set; }
	public Vector2 Direction  {get; private set; }
	public Vector2 InitialVelocity  {get; private set; }
    public Collider2D OwnerCollider;

	public void Initialize (GameObject owner, Vector2 direction, Vector2 initialVelocity )
	{
        transform.right = direction;
        Owner = owner;
        Direction = direction;
        InitialVelocity = initialVelocity;

        OnInitialized();
    }

	protected virtual void OnInitialized()
	{

	}

	public virtual void OnTriggerEnter2D(Collider2D collider)
	{
        OwnerCollider = Owner.GetComponent<Collider2D>();
        if ((CollisionMask.value & (1 << collider.gameObject.layer)) == 0)
        {
            OnNotCollideWith(collider);
            return;
        }


        var isOwner = collider.gameObject == Owner;
        if (isOwner)
        {
            OnCollideOwner();
            return;
        }

        if (collider.tag == "UPUP")
        {
            OnCollideUpUp();
            return;
        }

        var takeDamage = (CanTakeDamage)collider.GetComponent(typeof(CanTakeDamage));
        var GetHurt = (CanTakeDamage)OwnerCollider.GetComponent(typeof(CanTakeDamage));
        if (takeDamage != null)
        {
            if (collider.tag == "ThornsShield")
            {
                OnCollideThorns(OwnerCollider, GetHurt);
                return;
            }
            else if (collider.tag == "HumanCastle")
            {
                if (OwnerCollider.tag == "Player")
                {
                    OnCollideOther(collider);
                }
                else if (OwnerCollider.tag == "Enemy")
                {
                    OnCollideTakeDamage(collider, takeDamage);
                }
            }
            else if (collider.tag == "MonsterCastle")
            {
                if (OwnerCollider.tag == "Player")
                {
                    OnCollideTakeDamage(collider, takeDamage);
                }
                else if (OwnerCollider.tag == "Enemy")
                {
                    OnCollideOther(collider);
                }
            }
            else
            {
                OnCollideTakeDamage(collider, takeDamage);
                return;
            }
        }
        OnCollideOther(collider);
    }


    protected virtual void OnCollideUpUp()
    {

    }



	protected virtual void OnNotCollideWith(Collider2D collider)
	{

	}



    protected virtual void OnCollideOwner()
	{

	}



    protected virtual void OnCollideTakeDamage(Collider2D collider, CanTakeDamage takeDamage)
	{

    }


    protected virtual void OnCollideOther(Collider2D collider)
	{

    }


    protected virtual void OnCollideThorns(Collider2D OwnerCollider, CanTakeDamage GetHurt)
    {

    }

}
