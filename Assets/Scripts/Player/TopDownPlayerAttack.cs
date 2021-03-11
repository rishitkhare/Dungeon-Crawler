using System;
using UnityEngine;

[RequireComponent (typeof(TopDownPlayerController))]
public class TopDownPlayerAttack : MonoBehaviour
{
    public string attackButtonName = "Attack";

    public event EventHandler PlayerAttack;

    public float attackEndLag = 0.3f;
    float attackTimer;

    SimpleRigidbody rb;
    TopDownPlayerController controller;

    Animator anim;
    // Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<SimpleRigidbody>();
        controller = gameObject.GetComponent<TopDownPlayerController>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

        if(Input.GetButtonDown(attackButtonName)) {
            Attack();
            attackTimer = attackEndLag;
        }

        if (controller.state == PlayerState.Attacking) {
            AttackEndLag();
        }
    }

    void Attack() {
        controller.state = PlayerState.Attacking;
        anim.SetTrigger("Attack");
        PlayerAttack?.Invoke(this, new AttackEventArgs(rb.GetDirection(), attackEndLag));
    }

    void AttackEndLag() {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f) {
            controller.state = PlayerState.Idle;
        }
    }
}