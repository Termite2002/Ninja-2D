using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public GameObject hitVFX;
    public Rigidbody2D rb;
    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        rb.velocity = transform.right * 6f;
        Invoke(nameof(OnDespawn), 4f);
    }
    public void OnDespawn()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Character>().OnHit(30f);
            Instantiate(hitVFX, transform.position, transform.rotation);
            OnDespawn();
        }
    }
}
