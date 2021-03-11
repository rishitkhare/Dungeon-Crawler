using System;
using UnityEngine;


//attach this script to a child of an object with PlayerMovement script

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent (typeof(Animator))]
public class AttackAnimator : MonoBehaviour
{

    TopDownPlayerAttack playerAttackComponent;
    Animator hitboxAnimator;

    public string attackParameterName;
    int attackParameterHash;

    void Awake()
    {
        playerAttackComponent = transform.parent.GetComponentInParent<TopDownPlayerAttack>();
        hitboxAnimator = gameObject.GetComponent<Animator>();

        attackParameterHash = Animator.StringToHash(attackParameterName);

        playerAttackComponent.PlayerAttack += SetAnimation;
    }

    void SetAnimation(object sender, EventArgs e) {

        Vector2 direction = ((AttackEventArgs)e).direction;

        //rotation of sprite (assumes that angle = 0 is facing up)
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        transform.eulerAngles = new Vector3(0, 0, angle);

        ActivateHitboxAnimation();
    }

    void ActivateHitboxAnimation() {
        hitboxAnimator.SetTrigger(attackParameterHash);
    }
}
