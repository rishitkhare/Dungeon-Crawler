using System;
using UnityEngine;


//attach this script to a child of an object with PlayerMovement script

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent (typeof(Animator))]
public class AttackAnimator : MonoBehaviour
{

    TopDownPlayerAttack playerAttackComponent;
    BoxCollider2D playerHitBox;
    Animator animator;

    public float attackRange = 1f;

    public string attackParameterName;
    public string playerAttackParameter;
    int attackParameterHash;
    int playerAttackParameterHash;

    void Awake()
    {
        playerAttackComponent = transform.parent.GetComponentInParent<TopDownPlayerAttack>();

        playerHitBox = transform.parent.GetComponentInParent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();

        attackParameterHash = Animator.StringToHash(attackParameterName);
        playerAttackParameterHash = Animator.StringToHash(playerAttackParameter);

       playerAttackComponent.PlayerAttack += SetAnimation;
    }

    void SetAnimation(object sender, EventArgs e) {

        Vector2 direction = ((AttackEventArgs)e).direction;

        //positioning
        transform.localPosition = playerHitBox.offset;
        transform.localPosition += (Vector3) direction;

        //rotation of sprite (assumes the picture is facing up)
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        transform.eulerAngles = new Vector3(0, 0, angle);

        SetAnimationTrigger();
    }

    void SetAnimationTrigger() {
        animator.SetTrigger(attackParameterHash);
    }
}
