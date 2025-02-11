using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP;
    public Animator animator;

    private int currentHP;
    PlayerBase playerBase;
    PlayerController2Dnew playerController2Dnew;
    private bool isDead = false;
    public GameObject floatText;
    public Transform knockBackPoint;
    public Rigidbody2D rb;
    public LayerMask hitLayerMask;

    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        TryGetComponent<PlayerBase>(out playerBase);
        TryGetComponent<Animator>(out animator);
        TryGetComponent<PlayerController2Dnew>(out playerController2Dnew);
        if (rb == null)
        {
            if (!TryGetComponent<Rigidbody2D>(out rb))
            {
                Debug.Log("Rigidbody2D is missing");
            }

        }
        healthBar.SetMaxHealt(maxHP);
    }

    // Update is called once per frame
    public void TakeDamage(int dmg)
    {
        if(currentHP > 0 && !playerController2Dnew.isInvincible)
        {
            animator.SetTrigger("playerHit");
            currentHP -= dmg;
            rb.position = knockBackPoint.position;
            healthBar.SetCurrentHealth(currentHP);
            GameObject dmgText = Instantiate(floatText, transform.position, Quaternion.identity);
            dmgText.GetComponent<FloatText>().SetText(dmg.ToString());
        }

        if(currentHP <= 0 && !isDead)
        {
            
            Die();
        }

    }

    void Die()
    {
        if(!isDead)
        {
            isDead = true;
            animator.SetBool("isDead", isDead);
        }


    }


    public bool GetisDead()
    {
        return isDead;
    }
}
