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
    SpriteRenderer spriteRenderer;

    public EnemyState state {get; set; }
    public float speed = 2f;
    public float knockback = 10f;
    public float knockbackDecel = 0.88f;
    public float knockbackTime = 0.1f;

    private Vector2 knockbackDirection;

    private float knockbackTimer;

    public void Knockback(Vector3 damagerPosition) {
        knockbackDirection = (transform.position - damagerPosition).normalized;
        state = EnemyState.Knockback;
        rb.SetVelocity(knockbackDirection * knockback);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<SimpleRigidbody>();
        ai = gameObject.GetComponent<EnemyAI>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        state = EnemyState.Idle;
    }

    // Update is called once per frame
    void FixedUpdate()
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
                Vector2 direction = ai.GetDirection();
                rb.SetVelocity(direction * speed);
                if(direction.x != 0f) {
                    spriteRenderer.flipX = direction.x < 0;
                }

                break;

            case EnemyState.Knockback:
                knockbackTimer -= Time.deltaTime;
                rb.SetVelocity(rb.GetVelocity() * knockbackDecel);

                break;

        }
    }

    private void SetNewState() {
        switch (state) {

            case EnemyState.Knockback:
                if (knockbackTimer <= 0f) {
                    state = ai.stateAfterKnockback;
                    knockbackTimer = knockbackTime;
                }

                break;

        }
    }
}
