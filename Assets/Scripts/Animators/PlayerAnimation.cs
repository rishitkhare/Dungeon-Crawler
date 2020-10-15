using System;
using UnityEngine;

//converts custom state machine to animator

[RequireComponent(typeof(TopDownPlayerController))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    TopDownPlayerController controller;
    SimpleRigidbody rb;
    Animator anim;

    //Hashes
    private int IdleHash = Animator.StringToHash("Idle");
    private int WalkHash = Animator.StringToHash("Walk");
    private int AttackHash = Animator.StringToHash("Attack");
    private int DirectionXHash = Animator.StringToHash("DirectionX");
    private int DirectionYHash = Animator.StringToHash("DirectionY");

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<TopDownPlayerController>();
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<SimpleRigidbody>();
    }

    void Update() {
        anim.SetFloat(DirectionXHash, rb.GetDirection().x);
        anim.SetFloat(DirectionYHash, rb.GetDirection().y);
        UpdateAnimatorState();
    }

    // Update is called once per frame
    void UpdateAnimatorState() {
        switch (controller.state) {
            case PlayerState.Idle:
                anim.Play("Idle");
                break;

            case PlayerState.Walking:
                anim.Play("Walk");
                break;
            case PlayerState.Frozen:
                anim.Play("Idle");
                break;
        }
    }
}
