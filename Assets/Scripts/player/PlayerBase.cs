using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    //Component
    Animator animator;

    // Start is called before the first frame update
    public const string PLAYER_IDLE = "playerIdle";
    public const string PLAYER_RUN = "playerRun";
    public const string PLAYER_ATK1 = "playerAttack1";
    public const string PLAYER_ATK2 = "playerAttack2";
    public const string PLAYER_DASH = "playerDash";
    public const string PLAYER_DIE = "playerDie";
    public const string PLAYER_HIT = "playerHit";
    public const string PLAYER_JUMP_UP = "playerJumpUp";
    public const string PLAYER_JUMP_DOWN = "playerJumpDown";
    public const string PLAYER_VICTORY = "playerVictory";

    public string currentAnimState;
    Vector3 refLocalScale;
    bool flipX;

    void Start()
    {
        refLocalScale = transform.localScale;

        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("Animator is missing");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FilpXLocalScale(bool newValue)
    {
        if (flipX == newValue)
        {
            return;
        }
        if (newValue)
        {
            transform.localScale = new Vector3(-1 * refLocalScale.x, refLocalScale.y, refLocalScale.z);
        }
        else
        {
            transform.localScale = refLocalScale;
        }
        flipX = newValue;
    }


    public void ChangeAnimationState(string newState)
    {
        if (currentAnimState == newState)
        {
            return;
        }
        animator.Play(newState);
        currentAnimState = newState;
    }
}
