using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRun : StateMachineBehaviour
{
    
    public float attackRange = 3f;
    public float rangeAttackDelay = 5f;
    Transform player;
    Rigidbody2D rb;

    SlimeBoss slimeBoss;
    public float rangeAttackTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        slimeBoss = animator.GetComponent<SlimeBoss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        slimeBoss.LookAtPlayer();
        
        if(rangeAttackTimer > 0)
        {
            rangeAttackTimer -= Time.deltaTime;
        }
        else
        {
            FindObjectOfType<AudioManager>().PlaySound("SlimeRangeAttack");
            animator.SetTrigger("Attack_range");
            rangeAttackTimer = rangeAttackDelay;
        }


        if(slimeBoss.isAttacked && slimeBoss.currentHP > slimeBoss.maxHP/2)
        {
            rb.velocity = Vector2.zero;
            animator.SetTrigger("slimeHit");
        }
        else
        {
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, slimeBoss.speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }

        if(Vector2.Distance(rb.position, player.position) < attackRange)
        {
            
            animator.SetTrigger("Attack_melee");
        }    
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack_melee");
        slimeBoss.isAttacked = false;
    }


}
