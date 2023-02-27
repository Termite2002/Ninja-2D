using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{

    public void OnDespawn()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Character>().OnHit(30f);
            OnDespawn();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            OnDespawn();
        }
    }
}
