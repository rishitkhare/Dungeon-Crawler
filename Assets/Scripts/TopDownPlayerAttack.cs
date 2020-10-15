using System;
using UnityEngine;

[RequireComponent (typeof(TopDownPlayerController))]
public class TopDownPlayerAttack : MonoBehaviour
{
    public string attackButtonName = "Attack";

    public event EventHandler PlayerAttack;

    public float attackEndLag = 0.4f;
    float attackTimer;
    Boolean isAttacking;

    SimpleRigidbody rb;
    TopDownPlayerController controller;

    Animator anim;
    // Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<SimpleRigidbody>();
        controller = gameObject.GetComponent<TopDownPlayerController>();
        anim = gameObject.GetComponent<Animator>();
        isAttacking = false;
    }

    // Update is called once per frame
    void Update() {

        if(Input.GetButtonDown(attackButtonName)) {
            Attack();
            attackTimer = attackEndLag;
        }

        if(isAttacking) {
            attackTimer -= Time.deltaTime;
            AttackEndLag();
        }
    }

    void Attack() {
        controller.state = PlayerState.Attacking;
        isAttacking = true;
        anim.SetTrigger("Attack");
        PlayerAttack?.Invoke(this, new AttackEventArgs(rb.GetDirection(), attackEndLag));
    }

    void AttackEndLag() {
        if(attackTimer <= 0f) {
            controller.state = PlayerState.Idle;
            isAttacking = false;
        }
    }
}