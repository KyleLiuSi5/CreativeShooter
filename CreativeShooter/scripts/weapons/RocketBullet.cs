using UnityEngine;
using System.Collections;

public class RocketBullet : MonoBehaviour {

    private float Speed = 0f;
    public LayerMask CollisionMask;
    private int Damage = 20;
    private float TimeToLive = 10f;
    public Collider2D OwnerCollider;

    void Start()
    {
        Speed = 10f;
        TimeToLive = 10f;
        OwnerCollider = gameObject.GetComponentInParent<Collider2D>();        
    }

    void Update()
    {
        gameObject.transform.Translate(Vector2.right * Speed * Time.deltaTime);
        if((TimeToLive -= Time.deltaTime) <= 0)
        {
            DestroyBullet();
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((CollisionMask.value & (1 << collider.gameObject.layer)) == 0)
        {
            OnNotCollideWith(collider);
            return;
        }

        var isOwner = collider.gameObject == OwnerCollider;
        if(isOwner)
        {
            OnCollideOwner(collider);
            return;
        }


        var takeDamage = (CanTakeDamage)collider.GetComponent(typeof(CanTakeDamage));
        
        if (takeDamage != null)
        {          
            OnCollideTakeDamage(collider, takeDamage);
            DestroyBullet();
        }

        OnCollideOther(collider);

    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void OnNotCollideWith(Collider2D collider)
    {

    }

    public void OnCollideTakeDamage(Collider2D collider , CanTakeDamage takeDamage)
    {
        takeDamage.CmdTakeDamage(Damage, gameObject);
        DestroyBullet();
    }

    public void OnCollideOther(Collider2D collider)
    {
        return;
    }
    public void OnCollideOwner(Collider2D collider)
    {

    }


}
