using System;
using UnityEngine;

[RequireComponent(typeof(SimpleRigidbody))]
public class TopDownPlayerMovement : MonoBehaviour {
    public float Speed = 4f;
    public Vector2 input = Vector2.zero;
    public bool isMoving;

    Animator myAnimator;
    SimpleRigidbody rb;
    SpriteRenderer spr;

    RoomTransitionMovement roomMovement;

    //Animator Hashes
    readonly int directionXHash = Animator.StringToHash("DirectionX");
    readonly int directionYHash = Animator.StringToHash("DirectionY");
    readonly int isIdleHash = Animator.StringToHash("Idle");

    void Start() {
        rb = gameObject.GetComponent<SimpleRigidbody>();
        myAnimator = gameObject.GetComponent<Animator>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        UpdateAnimatorDirectionParameter();

        roomMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RoomTransitionMovement>();

        roomMovement.OnRoomTransitionEnter += DisableAnimator;
        roomMovement.OnRoomTransitionExit += EnableAnimator;
    }


    void DisableAnimator(object sender, EventArgs e) {
        Debug.Log("TriggerEnter!");
        myAnimator.enabled = false;
    }

    void EnableAnimator(object sender, EventArgs e) {
        Debug.Log("TriggerExit!");
        myAnimator.enabled = true;
    }

    void Update() {
        input = GetNormalizedInput();

        //direction will only change when walking, and not be lost after the player stops walking
        if (!input.Equals(Vector2.zero)) {
            isMoving = true;

            if (Vector2.Dot(input, rb.GetDirection()) <= 0) {
                rb.SetDirection(new Vector2(input.x, input.y));

                //prevents diagonal directions
                if((int)rb.GetDirection().x != rb.GetDirection().x) {
                    rb.SetDirectionX(0);
                    rb.SetDirectionY(Mathf.Sign(rb.GetDirection().y));
                }
            }

            UpdateAnimatorDirectionParameter();
        }
        else {
            isMoving = false;
        }

        myAnimator.SetBool(isIdleHash, !isMoving);

        //flip axis according to direction
        if (rb.GetDirection().Equals(Vector2.right)) {
            spr.flipX = true;
        }
        else {
            spr.flipX = false;
        }

    }

    // Update is called once per frame
    void FixedUpdate() {
        rb.SetVelocity(input * Speed);
    }

    private Vector2 GetNormalizedInput() {
        Vector2 output = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        output = Vector3.Normalize(output); // makes it so that character does not go faster diagonally.

        return output;
    }

    private void UpdateAnimatorDirectionParameter() {
        myAnimator.SetFloat(directionXHash, rb.GetDirection().x);
        myAnimator.SetFloat(directionYHash, rb.GetDirection().y);
    }
}

