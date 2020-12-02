using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {
    Idle,
    Walking,
    Knockback
}


[RequireComponent(typeof(SimpleRigidbody))]
public class SkeletonController : MonoBehaviour
{
    SimpleRigidbody rb;
    EnemyAI ai;

    public EnemyState state;
    public float knockback = 10f;
    public float knockbackTime = 0.1f;

    private Vector2 knockbackDirection;
    private float roomXindex;
    private float roomYindex;

    private float knockbackTimer;

    public void Knockback(Vector3 damagerPosition) {
        knockbackDirection = transform.position - damagerPosition;
        knockbackDirection.Normalize();
        state = EnemyState.Knockback;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<SimpleRigidbody>();
        ai = gameObject.GetComponent<EnemyAI>();
        state = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        SetNewState();
        PerformStateOperations();
    }

    private void PerformStateOperations() {
        switch(state) {
            case EnemyState.Idle:
                rb.SetVelocity(Vector2.zero);

                break;

            case EnemyState.Walking:
                rb.SetVelocity(ai.GetDirection());

                break;

            case EnemyState.Knockback:
                knockbackTimer -= Time.deltaTime;
                rb.SetVelocity(knockbackDirection * knockback * 0.3f);

                break;

        }
    }

    private void SetNewState() {
        switch (state) {

            case EnemyState.Knockback:
                if (knockbackTimer <= 0f) {
                    state = EnemyState.Idle;
                    knockbackTimer = knockbackTime;
                }

                break;

        }
    }
}
