using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBoss : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Transform attackpoint;
    [SerializeField] private float range = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damage;
    [SerializeField] private AudioClip attacksound;
    [SerializeField] private AudioClip attackHeavysound;
    private float ratenormal = 1.5f;
    private float rateheavy = 0.75f;
    private float nextAttackTime = 0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / ratenormal;
            }

            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                AttackHeavy();
                nextAttackTime = Time.time + 1f / rateheavy;
            }
        }

    }

    private void Attack()
    {
        SoundManager.instance.PlaySound(attacksound);
        anim.SetTrigger("Attack");
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackpoint.position, range, enemyLayer);
        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Boss>().TakeDamage(damage);
        }
    }

    private void AttackHeavy()
    {
        SoundManager.instance.PlaySound(attackHeavysound);
        anim.SetTrigger("AttackH");
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackpoint.position, range, enemyLayer);
        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Boss>().TakeDamage(damage * 2);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackpoint == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpoint.position, range);
    }
}
