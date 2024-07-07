using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
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
    [SerializeField] private AudioClip ragesound;
    private float cooldownTimer = Mathf.Infinity;
    public Slider healthbar;
    public GameObject door;

    private float rageattackCooldown;
    private int currenthp;
    private int ragethreshold;
    private bool firstrage = true;
    private Animator anim;
    private HealthforBoss playerhealth;
    private EnemyPatrol enemyPatrol;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Start()
    {
        currenthp = hp;
        ragethreshold = hp / 2;
        rageattackCooldown = attackCooldown / 2;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && currenthp > ragethreshold)
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                SoundManager.instance.PlaySound(atksound);
                anim.SetTrigger("attack");
            }
        }
        else if (PlayerInSight() && currenthp <= ragethreshold)
        {
            if (cooldownTimer >= rageattackCooldown)
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
            playerhealth = hit.transform.GetComponent<HealthforBoss>();
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
        healthbar.value = currenthp;
        if (currenthp <= ragethreshold && firstrage)
        {
            anim.SetTrigger("rage");
            firstrage = false;
        }
        if (currenthp <= 0)
        {
            anim.SetTrigger("die");
            GetComponent<Collider2D>().enabled = false;
            enabledoor();
            this.enabled = false;
            enemyPatrol.enabled = false;
        }
    }

    public void playerRespawn()
    {
        currenthp = hp;
        healthbar.value = currenthp;
        firstrage = true;

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

    private void ERageSound()
    {
        SoundManager.instance.PlaySound(ragesound);
    }
    private void disablecollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    private void enablecollider()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    public void Respawnhealth()
    {
        currenthp = hp;
    }

    private void enabledoor()
    {
        door.SetActive(true);
    }
}
