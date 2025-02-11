using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : MonoBehaviour
{
    public SpriteRenderer sprite;
    
    public Transform player;
    public int meleeAttack;
    public float meleeAttackRange = 1f;
    public LayerMask attackMask;
    public Transform meleeAttackPoint;
    public bool isAttacked = false;
    public float speed = 2.5f;
    public float enragedSpeed = 5.0f;

    public bool isEnraged = false;

    public Transform rangeAttackPoint;
    public GameObject slimeBullet;

    public GameObject lights;

    public Animator animator;

    public GameObject floatText;

    public int maxHP;

    public int currentHP;
    public bool isDead = false;

    public bool isFlipped = false;

    public HealthBar healthBar;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        TryGetComponent<Animator>(out animator);
        sprite = GetComponentInChildren<SpriteRenderer>();
        currentHP = maxHP;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        healthBar.SetMaxHealt(maxHP);
    }

    private void Update()
    {
        if(isDead)
        {
            lights.SetActive(false);
        }
        
    }



    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = position - (Vector2)player.transform.position; //get the direction to the target
        if (direction.x > 0)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0));
            isFlipped = false;
        }
        else if (direction.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f));
            isFlipped = true;
        }
    }

    public void Attack()
    {
        
        Collider2D hitPlayer = Physics2D.OverlapCircle(meleeAttackPoint.position, meleeAttackRange, attackMask);
        if(hitPlayer != null)
        {
            hitPlayer.GetComponent<PlayerHealth>().TakeDamage(meleeAttack);
        }
    }



    public void TakeDamage(int dmg)
    {
        if (currentHP > 0)
        {
            isAttacked = true;
            currentHP -= dmg;
            healthBar.SetCurrentHealth(currentHP);
            GameObject dmgText = Instantiate(floatText, transform.position, Quaternion.identity);
            dmgText.GetComponent<FloatText>().SetText(dmg.ToString());
            FindObjectOfType<AudioManager>().PlaySound("SlimeHit");
            if (currentHP < maxHP / 2 && !isEnraged)
            {
                speed = enragedSpeed;
                sprite.color = new Color(1f, 0.7f, 0.7f, 1f);
                isEnraged = true;
                FindObjectOfType<AudioManager>().PlaySound("SlimeEnrage");
            }
        }



        if (currentHP <= 0 && !isDead)
        {

            Die();
        }

    }

    void Die()
    {
        if (!isDead)
        {
            FindObjectOfType<AudioManager>().PlaySound("SlimeDie");
            isDead = true;
            animator.SetTrigger("isDead");
        }


    }

    public void RangeAttack()
    {
        Instantiate(slimeBullet, rangeAttackPoint.position, rangeAttackPoint.rotation);
    }


}
