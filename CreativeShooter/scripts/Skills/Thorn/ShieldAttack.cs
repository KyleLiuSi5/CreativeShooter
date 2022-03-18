using UnityEngine;
using System.Collections;

public class ShieldAttack : MonoBehaviour {

    public LayerMask CollisionMask;
    public int Damage;
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


    void OnCollideTakeDamage(Collider2D collider, CanTakeDamage takeDamage)
    {
        takeDamage.CmdTakeDamage(Damage, gameObject);
        collider.gameObject.GetComponent<CharacterBehavior>().CmdChangeTag("Daze");
    }

    void OnCollideOther(Collider2D collider)
    {

    }
}
