using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class SimpleRigidbody : MonoBehaviour
{
    Vector2 velocity;
    private Vector2 direction = Vector2.down;

    public LayerMask collidableLayer;

    bool isLockMovement = false;
    bool isLockDirection = false;
    float lockMovementTimer = 0f;
    float lockDirectionTimer = 0f;

    BoxCollider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = gameObject.GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        LockMoveTimerUpdate();
        LockDirectionTimerUpdate();
    }

    void FixedUpdate() {
        if(!isLockMovement) {
            Move();
        }
    }



    /// DIRECTION MUTATORS AND ACCESSORS


    public void SetDirection(Vector2 direction) {
        if(!isLockDirection) {
            this.direction = direction;
        }
    }
    public void SetDirectionX(float x) {
        if(!isLockDirection) {
            direction.x = x;
        }
    }
    public void SetDirectionY(float y) {
        if(!isLockDirection) {
            direction.y = y;
        }
    }


    public Vector2 GetDirection() { return direction; }





    void LockMoveTimerUpdate() {
        if (lockMovementTimer <= 0f) {
            isLockMovement = false;
            lockMovementTimer = 0f;
        }
        else {
            isLockMovement = true;
            lockMovementTimer -= Time.deltaTime;
        }
    }

    void LockDirectionTimerUpdate() {
        if (lockDirectionTimer <= 0f) {
            isLockDirection = false;
            lockDirectionTimer = 0f;
        }
        else {
            isLockDirection = true;
            lockDirectionTimer -= Time.deltaTime;
        }
    }

    public void SetVelocity(Vector2 velocity) {
        this.velocity = velocity;
    }

    public void LockMovement(float time) {
        lockMovementTimer = time;
    }
    public void LockMovement() {
        LockMovement(0.05f);
    }

    public void LockDirection(float time) {
        lockDirectionTimer = time;
    }
    public void LockDirection() {
        LockDirection(0.05f);
    }

    private void Move() {
        Vector2 deltaPosition = Vector2.zero;
        if(velocity.x != 0) {
            deltaPosition.x = RaycastXCollision(velocity.x * Time.fixedDeltaTime);
        }
        if(velocity.y != 0) {
            deltaPosition.y = RaycastYCollision(velocity.y * Time.fixedDeltaTime);
        }

        if(deltaPosition.magnitude > 0.05f) {
            transform.position += (Vector3) deltaPosition;
        }
    }

    private float RaycastXCollision(float deltaX) {
        //finds center of collider
        Vector2 colliderCenterPos = transform.position;
        colliderCenterPos.x += myCollider.offset.x;
        colliderCenterPos.y += myCollider.offset.y;

        //start ray on edge of collider (depending on right or left)
        Vector2 origin = colliderCenterPos;
        origin.x += Mathf.Sign(deltaX) * 0.5f * myCollider.size.x;

        //cast 3 rays for detection
        RaycastHit2D hit1 = Physics2D.Raycast(origin, Vector2.right, deltaX, collidableLayer);
        DebugRayHits(hit1, origin, Vector2.right, deltaX);

        origin.y += myCollider.size.y * 0.45f;
        RaycastHit2D hit2 = Physics2D.Raycast(origin, Vector2.right, deltaX, collidableLayer);
        DebugRayHits(hit2, origin, Vector2.right, deltaX);

        origin.y -= myCollider.size.y * 0.9f;
        RaycastHit2D hit3 = Physics2D.Raycast(origin, Vector2.right, deltaX, collidableLayer);
        DebugRayHits(hit3, origin, Vector2.right, deltaX);

        if (hit1 || hit2 || hit3) {
            return Mathf.Sign(deltaX) * Mathf.Min(Mathf.Min(hit1.distance, hit2.distance), hit3.distance);
        }
        else {
            return deltaX;
        }
    }

    private float RaycastYCollision(float deltaY) {
        //finds center of collider
        Vector2 colliderCenterPos = transform.position;
        colliderCenterPos.x += myCollider.offset.x;
        colliderCenterPos.y += myCollider.offset.y;

        //start ray on edge of collider (depending on right or left)
        Vector2 origin = colliderCenterPos;
        origin.y += Mathf.Sign(deltaY) * 0.5f * myCollider.size.x;

        //cast 3 rays for detection
        RaycastHit2D hit1 = Physics2D.Raycast(origin, Vector2.up, deltaY, collidableLayer);
        DebugRayHits(hit1, origin, Vector2.up, deltaY);

        origin.x += myCollider.size.x * 0.45f;
        RaycastHit2D hit2 = Physics2D.Raycast(origin, Vector2.up, deltaY, collidableLayer);
        DebugRayHits(hit2, origin, Vector2.up, deltaY);

        origin.x -= myCollider.size.x * 0.9f;
        RaycastHit2D hit3 = Physics2D.Raycast(origin, Vector2.up, deltaY, collidableLayer);
        DebugRayHits(hit3, origin, Vector2.up, deltaY);

        if (hit1 || hit2 || hit3) {
            return Mathf.Sign(deltaY) * Mathf.Min(Mathf.Min(hit1.distance, hit2.distance), hit3.distance);
        }
        else {
            return deltaY;
        }
    }

    private void DebugRayHits(RaycastHit2D hit, Vector2 origin, Vector2 direction, float mag) {
        if(hit) {
            Debug.DrawRay(origin, Mathf.Sign(mag) * hit.distance * direction, Color.green);
        }
        else {
            Debug.DrawRay(origin, mag * direction, Color.red);
        }
    }
}
