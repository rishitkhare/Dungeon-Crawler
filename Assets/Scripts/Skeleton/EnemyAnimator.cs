using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public string IdleAnimation;
    public string WalkingAnimation;
    public string DamageAnimation;

    Animator animator;
    SkeletonController controller;

    int IdleHash;
    int WalkingHash;
    int DamageHash;

    // Start is called before the first frame update
    void Start()
    {
        IdleHash = Animator.StringToHash(IdleAnimation);
        WalkingHash = Animator.StringToHash(WalkingAnimation);
        DamageHash = Animator.StringToHash(DamageAnimation);
        animator = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<SkeletonController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(controller.state) {
            case EnemyState.Idle:
                animator.Play(IdleHash);
                break;
            case EnemyState.Walking:
                animator.Play(WalkingHash);
                break;
            case EnemyState.Knockback:
                animator.Play(DamageHash);
                break;
        }
    }
}
