using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public Rigidbody2D rb;
    public float walkSpeed = 4f;
    public Animator animator;
    private float speedLimiter = 0.7f;
    public float inputHorizontal;
    public float inputVertical;

    Vector3 refLocalScale;
    bool flipX;
    string currentAnimState;

    const string PLAYER_IDLE = "player1_spear_idle";
    const string PLAYER_RUN = "player1_spear_Run_R";

    // Start is called before the first frame update
    void Start()
    {
        refLocalScale = transform.localScale;
        if(rb == null)
        {
            if(!TryGetComponent<Rigidbody2D>(out rb))
            {
                Debug.Log("Rigidbody2D is missing");
            }
            
        }

        if (animator == null)
        {
            if (!TryGetComponent<Animator>(out animator))
            {
                Debug.Log("Animator is missing");
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if(inputHorizontal != 0 || inputVertical != 0)
        {
            if(inputHorizontal != 0 && inputVertical != 0)
            {
                inputHorizontal *= speedLimiter;
                inputVertical *= speedLimiter;
            }
            rb.velocity = new Vector2(inputHorizontal * walkSpeed, inputVertical * walkSpeed);
            if(inputHorizontal > 0.01f)
            {
                FilpXLocalScale(false);
            }
            else if(inputHorizontal < -0.01f)
            {
                FilpXLocalScale(true);
            }
            ChangeAnimationState(PLAYER_RUN);
        }
        else
        {
            rb.velocity = Vector2.zero;
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    void ChangeAnimationState(string newState)
    {
        if(currentAnimState == newState)
        {
            return;
        }
        animator.Play(newState);

        currentAnimState = newState;
    }

    void FilpXLocalScale(bool newValue)
    {
        if (flipX == newValue)
        {
            return;
        }
        if(newValue)
        {
            transform.localScale = new Vector3(-1 * refLocalScale.x, refLocalScale.y, refLocalScale.z);
        }
        else
        {
            transform.localScale = refLocalScale;
        }
        flipX = newValue;
    }
}
