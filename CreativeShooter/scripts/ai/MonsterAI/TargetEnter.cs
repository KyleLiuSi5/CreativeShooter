using UnityEngine;
using System.Collections;

public class TargetEnter : MonoBehaviour {
    //用來判斷是否為可以行走的位置
    public bool CantWalk = false;
    public LayerMask ColliderMask;

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.layer == 8)
        {
            CantWalk = true;
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 8)
        {
            CantWalk = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        CantWalk = false;
    }
}
