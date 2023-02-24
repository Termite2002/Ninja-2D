using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatTextPrefab;

    private float hp;
    private string currentAnimName;

    public bool isDead => hp <= 0;
    private void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, transform);
    }
    public virtual void OnDespawn()
    {

    }
    protected virtual void OnDeath()
    {
        ChangeAnim("Die");

        Invoke(nameof(OnDespawn), 1.5f);
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
    public void OnHit(float damage)
    {
        Debug.Log("hit");
        if (!isDead)
        {
            hp -= damage;
            if (isDead)
            {
                hp = 0;
                OnDeath();
            }
            healthBar.SetNewHp(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }
}
