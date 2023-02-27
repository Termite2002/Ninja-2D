using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
	public SpriteRenderer theSR;
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other.gameObject.GetComponent<Player>().getHp() < 100f)
        {
			StartCoroutine(Heal(other));
		}
	}
    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //        StopCoroutine(Heal(other));
    //}
    IEnumerator Heal(Collider2D other)
	{
		theSR.sprite = null;
		Debug.Log("fa");
		for (float currentHealth = other.gameObject.GetComponent<Player>().getHp(); currentHealth <= 100; currentHealth += 0.05f)
		{
			other.gameObject.GetComponent<Player>().setHp(currentHealth);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		other.gameObject.GetComponent<Player>().setHp(100f);
		Destroy(gameObject);
	}
}
