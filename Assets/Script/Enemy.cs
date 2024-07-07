using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    [SerializeField] private int hp;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioClip atksound;
    [SerializeField] private AudioClip hurtsound;
    [SerializeField] private AudioClip diesound;
    private float cooldownTimer = Mathf.Infinity;

    private int currenthp;
    private Animator anim;
    private Health playerhealth;
    private EnemyPatrol enemyPatrol;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Start()
    {
        currenthp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                SoundManager.instance.PlaySound(atksound);
                anim.SetTrigger("attack");
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
           new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
           0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerhealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerhealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(int playerDamage)
    {
        currenthp -= playerDamage;
        anim.SetTrigger("hurt");


        if (currenthp <= 0)
        {
            anim.SetTrigger("die");
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().mass = 0;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            
            this.enabled = false;
            enemyPatrol.enabled = false;
        }
    }

    private void deactivate()
    {
        gameObject.SetActive(false);
    }

    private void EHurtSound()
    {
        SoundManager.instance.PlaySound(hurtsound);
    }
    private void EDieSound()
    {
        SoundManager.instance.PlaySound(diesound);
    }
}
