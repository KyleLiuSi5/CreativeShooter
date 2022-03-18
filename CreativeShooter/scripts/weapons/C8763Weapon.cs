using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class C8763Weapon : NetworkBehaviour {

    public LayerMask CollisionMask;
    public int Damage;
    public GameObject HitEffect;
    public GameObject Owner;

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if ((CollisionMask.value & (1 << collider.gameObject.layer)) == 0)
        {
            return;
        }

        var isOwner = collider.gameObject == Owner;
        if (isOwner)
        {
            return;
        }

        var takeDamage = (CanTakeDamage)collider.GetComponent(typeof(CanTakeDamage));
        if (takeDamage != null)
        {
            OnCollideTakeDamage(collider, takeDamage);
            return;
        }

        OnCollideOther(collider);
    }

    public void OnCollideTakeDamage(Collider2D collider, CanTakeDamage takeDamage)
    {
        Instantiate(HitEffect, collider.transform.position, collider.transform.rotation);
        collider.gameObject.GetComponent<CharacterBehavior>().CmdChangeTag("Daze");
        takeDamage.CmdTakeDamage(Damage, gameObject);
    }

    public void OnCollideOther(Collider2D collider)
    {
        
    }
   
}
