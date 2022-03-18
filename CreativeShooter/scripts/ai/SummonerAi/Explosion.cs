using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Explosion : NetworkBehaviour {

    public int damage = 50;
    public ParticleSystem ExplosionAni;
    public Collider2D targetCollider;


    void OnTriggerEnter2D(Collider2D playerexplosionArea)//player's collider
    {

        var takedamage = (CanTakeDamage)playerexplosionArea.GetComponent(typeof(CanTakeDamage));//從CANTAKEDAMAGE裡抓取該collider判斷是否為可被傷害的物件
        if (takedamage != null)//是為可以被傷害的物件
        {
            if (playerexplosionArea.tag == "Player")
            {
                OnColliderTakeDamage(playerexplosionArea, takedamage);
            }
        }
    }
    public void OnColliderTakeDamage(Collider2D collider, CanTakeDamage takedamage)
    {
        Sabtour();
        takedamage.CmdTakeDamage(damage, collider.gameObject);
    }
    public void TakeDamage(int damage, GameObject instigator)
    {
        Sabtour();
    }

    public void Sabtour()
    {

        Destroy(gameObject);
        Instantiate(ExplosionAni, gameObject.transform.position, Quaternion.identity);
    }
}
