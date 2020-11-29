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
    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int WalkHash = Animator.StringToHash("Walk");
    private readonly int TintRedHash = Animator.StringToHash("Tint Red");
    private readonly int I_FrameHash = Animator.StringToHash("Invincible");
    private readonly int DirectionXHash = Animator.StringToHash("DirectionX");
    private readonly int DirectionYHash = Animator.StringToHash("DirectionY");

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
                anim.Play(IdleHash);
                break;
            case PlayerState.Walking:
                anim.Play(WalkHash);
                break;
            case PlayerState.Frozen:
                anim.Play(IdleHash);
                break;
            case PlayerState.Knockback:
                anim.Play(TintRedHash);
                break;
        }

        anim.SetBool(I_FrameHash, controller.isI_frame);
    }
}
