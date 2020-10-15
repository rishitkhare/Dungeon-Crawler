using System;
using UnityEngine;

public enum PlayerState {
    Idle,
    Walking,
    Attacking,
    Knockback,
    Frozen
}

[RequireComponent(typeof(SimpleRigidbody))]
[RequireComponent(typeof(Health))]
public class TopDownPlayerController : MonoBehaviour {
    public float speed = 4f;
    public Vector2 input = Vector2.zero;
    public float knockback = 10f;
    public float knockbackTime = 0.1f;

    private float knockbackTimer;

    //float timer;

    public PlayerState state { get; set;}

    SimpleRigidbody rb;

    RoomTransitionMovement roomMovement;

    void Freeze(object sender, EventArgs e) {
        state = PlayerState.Frozen;
    }

    void Unfreeze(object sender, EventArgs e) {
        state = PlayerState.Idle;
    }

    void Start() {
        //timer = 0f;
        state = PlayerState.Idle;
        rb = gameObject.GetComponent<SimpleRigidbody>();
        knockbackTimer = knockbackTime;

        roomMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RoomTransitionMovement>();

        roomMovement.OnRoomTransitionEnter += Freeze;
        roomMovement.OnRoomTransitionExit += Unfreeze;
    }

    void Update() {
        input = GetNormalizedInput();

        SetNewState();
        PerformStateOperations();
    }

    private void PerformStateOperations() {
        switch (state) {
            case PlayerState.Idle:
                rb.SetVelocity(Vector2.zero);

                break;

            case PlayerState.Walking:
                MovePlayer();

                break;

            case PlayerState.Knockback:
                knockbackTimer -= Time.deltaTime;

                //pushes player back in the opposite direction they are facing.
                rb.SetVelocity(-(knockback * 0.3f) * rb.GetDirection());

                break;

            case PlayerState.Attacking:
                rb.SetVelocity(Vector2.zero);

                break;

            case PlayerState.Frozen:
                rb.SetVelocity(Vector2.zero);

                break;


        }
    }

    private void SetNewState() {
        switch(state) {
            case PlayerState.Idle:
                if (!input.Equals(Vector2.zero)) {
                    state = PlayerState.Walking;
                }

                break;

            case PlayerState.Walking:
                if (input.Equals(Vector2.zero)) {
                    state = PlayerState.Idle;
                }

                break;

            case PlayerState.Knockback:
                if(knockbackTimer <= 0f) {
                    state = PlayerState.Idle;
                    knockbackTimer = knockbackTime;
                }

                break;
        }
    }

    private void MovePlayer() {
        rb.SetVelocity(input * speed);

        if (Vector2.Dot(input, rb.GetDirection()) <= 0) {
            rb.SetDirection(new Vector2(input.x, input.y));

            //prevents diagonal directions
            if ((int)rb.GetDirection().x != rb.GetDirection().x) {
                rb.SetDirectionX(0);
                rb.SetDirectionY(Mathf.Sign(rb.GetDirection().y));
            }
        }
    }

    private Vector2 GetNormalizedInput() {
        Vector2 output = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        output = Vector3.Normalize(output); // makes it so that character does not go faster diagonally.

        return output;
    }
}

