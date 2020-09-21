using System;
using UnityEngine;

[RequireComponent (typeof(TopDownPlayerMovement))]
public class TopDownPlayerAttack : MonoBehaviour
{
    public string attackButtonName = "Attack";

    public event EventHandler PlayerAttack;

    public string attackAnimatorBooleanParameter = "AttackBool";
    int attackAnimatorBoolHash;

    public float attackEndLag = 0.4f;
    float attackTimer = 0f;

    SimpleRigidbody rb;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<SimpleRigidbody>();
        anim = gameObject.GetComponent<Animator>();
        attackAnimatorBoolHash = Animator.StringToHash(attackAnimatorBooleanParameter);

    }

    // Update is called once per frame
    void Update() {

        if(Input.GetButtonDown(attackButtonName)) {
            attackTimer = attackEndLag;
            PlayerAttack?.Invoke(this, new DirectionArgs(rb.GetDirection()));
            anim.SetBool(attackAnimatorBoolHash, true);
        }

        //timer + animation parameters
        if (attackTimer > 0f) {
            attackTimer -= Time.deltaTime;
            rb.LockMovement();
            rb.LockDirection();
        }
        else {
            anim.SetBool(attackAnimatorBoolHash, false);
        }
        
    }
}