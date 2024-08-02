using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Movement.isAir)
            animator.SetBool("isAir", true);
        else
            animator.SetBool("isAir", false);

        if(!Movement.superSpeed)
        {
            animator.SetBool("superSpeed", false);
            if (Movement.isRunning)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", true);
            }
            else if (Movement.isWalking)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
            }
        }
        else if(Movement.superSpeed)
        {
            animator.SetBool("superSpeed", true);
        }
    }
}