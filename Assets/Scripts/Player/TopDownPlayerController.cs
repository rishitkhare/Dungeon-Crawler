using System;
using UnityEngine;

public enum PlayerState {
    Idle,
    Walking,
    Attacking,
    Knockback,
    Frozen,
    RoomTransition
}

[RequireComponent(typeof(SimpleRigidbody))]
[RequireComponent(typeof(Health))]
public class TopDownPlayerController : MonoBehaviour {
    public float speed = 4f;
    public float accelFactor = 0.85f;
    public float knockback = 10f;
    public float knockbackTime = 0.1f;
    public float PostHitInvincibilityTime = 0.6f;

    private Vector2 targetVelocity;

    private float knockbackTimer;
    private Vector2 knockbackDirection;

    private Vector2 input = Vector2.zero;

    public bool isI_frame;
    private float I_frameTimer;

    public PlayerState state { get; set;}

    SimpleRigidbody rb;

    RoomTransitionMovement roomMovement;

    void Freeze(object sender, EventArgs e) {
        state = PlayerState.Frozen;
    }

    void Unfreeze(object sender, EventArgs e) {
        state = PlayerState.Idle;
    }

    public void KnockPlayerBack(Vector3 inflictorPosition) {
        state = PlayerState.Knockback;
        knockbackDirection = (transform.position - inflictorPosition).normalized;
        rb.SetVelocity(knockback * knockbackDirection);
        targetVelocity = Vector3.zero;
    }

    void Start() {
        //timer = 0f;
        state = PlayerState.Idle;
        rb = gameObject.GetComponent<SimpleRigidbody>();
        knockbackTimer = knockbackTime;
        isI_frame = false;

        roomMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RoomTransitionMovement>();

        roomMovement.OnRoomTransitionEnter += Freeze;
        roomMovement.OnRoomTransitionExit += Unfreeze;
    }

    void FixedUpdate() {
        input = GetNormalizedInput();

        CheckIFrames();

        SetNewState();
        PerformStateOperations();

        ApplyAcceleration();
    }

    private void ApplyAcceleration() {

        rb.AddVelocity(accelFactor * (targetVelocity - rb.GetVelocity()));
    }

    private void CheckIFrames() {
        if (isI_frame) {
            I_frameTimer += Time.deltaTime;
            if (I_frameTimer > PostHitInvincibilityTime) {
                isI_frame = false;
                I_frameTimer = 0;
            }
        }
    }

    private void PerformStateOperations() {
        switch (state) {
            case PlayerState.Idle:
                targetVelocity = Vector3.zero;

                break;

            case PlayerState.Walking:
                MovePlayer();

                break;

            case PlayerState.Knockback:
                knockbackTimer -= Time.deltaTime;
                isI_frame = true;

                break;

            case PlayerState.Attacking:
                rb.SetVelocity(Vector2.zero);

                break;

            case PlayerState.Frozen:
                rb.SetVelocity(Vector2.zero);

                break;
            case PlayerState.RoomTransition:
                rb.SetVelocity(rb.GetDirection() * knockback * 0.3f);

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
        targetVelocity = speed * input;

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

