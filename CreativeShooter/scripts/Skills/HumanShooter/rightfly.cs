using UnityEngine;
using System.Collections;

public class rightfly : MonoBehaviour {

    public float rocketspeed = 5f;
    public int Damage = 20;
    public LayerMask CollisionMask;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.Translate(Vector2.right * 5 * rocketspeed * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((CollisionMask.value & (1 << collider.gameObject.layer)) == 0)
        {
            OnNotCollideWith(collider);
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

    public void OnCollideTakeDamage(Collider2D collider, CanTakeDamage takeDamage)
    {
        takeDamage.CmdTakeDamage(Damage, gameObject);
        DestroyBullet();
    }

    public void OnCollideOther(Collider2D collider)
    {
        return;
    }
}
