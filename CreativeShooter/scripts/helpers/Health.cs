using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class Health : NetworkBehaviour, CanTakeDamage
{
    [SyncVar]
	public int CurrentHealth;
    [SyncVar]
    public int OriginalHealth;

	public GameObject HurtEffect;
	public GameObject DestroyEffect;
    


    [Command]
    public void CmdTakeDamage(int damage,GameObject instigator)
	{
        RpcTakeDamage(damage, instigator);
    }
    [ClientRpc]
    public void RpcTakeDamage(int damage, GameObject instigator)
    {
        Instantiate(HurtEffect, instigator.transform.position, transform.rotation);
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            CmdDestroyObject();
            return;
        }

        Color initialColor = GetComponent<Renderer>().material.color;
        Color flickerColor = new Color32(255, 20, 20, 255);
        StartCoroutine(Flicker(initialColor, flickerColor, 0.02f));
    }



    IEnumerator Flicker(Color initialColor, Color flickerColor, float flickerSpeed)
    {
        for (var n = 0; n < 10; n++)
        {
            GetComponent<Renderer>().material.color = initialColor;
            yield return new WaitForSeconds(flickerSpeed);
            GetComponent<Renderer>().material.color = flickerColor;
            yield return new WaitForSeconds(flickerSpeed);
        }
        GetComponent<Renderer>().material.color = initialColor;

        // makes the character colliding again with layer 12 (Projectiles) and 13 (Enemies)
        Physics2D.IgnoreLayerCollision(9, 12, false);
        Physics2D.IgnoreLayerCollision(9, 13, false);
    }
	
    [Command]
	private void CmdDestroyObject()
	{
        RpcDestroyObject();
	}
    [ClientRpc]
    private void RpcDestroyObject()
    {
        if (DestroyEffect != null)
        {
            var instantiatedEffect = (GameObject)Instantiate(DestroyEffect, transform.position, transform.rotation);
            instantiatedEffect.transform.localScale = transform.localScale;
            NetworkServer.Spawn(DestroyEffect);
        }
        gameObject.SetActive(false);
    }
}
