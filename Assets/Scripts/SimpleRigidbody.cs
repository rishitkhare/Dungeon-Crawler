using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SimpleRigidbody : MonoBehaviour
{
    private Vector2 velocity;
    [SerializeField]
    private Vector2 direction = Vector2.down;

    public bool snapToGrid = false;
    public LayerMask collidableLayer;

    BoxCollider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = gameObject.GetComponent<BoxCollider2D>();

    }

    void FixedUpdate() {
        Move();
    }


    /// DIRECTION MUTATORS AND ACCESSORS

    public void SetDirection(Vector2 direction) {
        this.direction = direction;
    }

    public void SetDirectionX(float x) {
        direction.x = x;
    }
    public void SetDirectionY(float y) {
        direction.y = y;
    }

    public Vector2 GetDirection() { return direction; }

    public Vector2 GetTrueDirection() {
        if(velocity.magnitude != 0) {
            return velocity.normalized;
        }
        else {
            return direction;
        }
    }

    public Vector2 GetVelocity() {
        return velocity;
    }

    //Velocity mutator

    public void SetVelocity(Vector2 velocity) {
        this.velocity = velocity;
    }

    private void Move() {
        Vector2 deltaPosition = Vector2.zero;
        if(velocity.x != 0) {
            deltaPosition.x = RaycastXCollision(velocity.x * Time.fixedDeltaTime);
        }
        if(velocity.y != 0) {
            deltaPosition.y = RaycastYCollision(velocity.y * Time.fixedDeltaTime);
        }

        if(deltaPosition.magnitude > 0.03f) {
            transform.position += (Vector3) deltaPosition;
        }

        if (deltaPosition.Equals(Vector2.zero) && snapToGrid) {
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), transform.position.z);
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

        //start ray on edge of collider (depending on up or down)
        Vector2 origin = colliderCenterPos;
        origin.y += Mathf.Sign(deltaY) * 0.5f * myCollider.size.y;

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

    private void DebugRayHits(RaycastHit2D hit, Vector2 origin, Vector2 direction, float magnitude) {
        if(hit) {
            Debug.DrawRay(origin, Mathf.Sign(magnitude) * hit.distance * direction, Color.green);
        }
        else {
            Debug.DrawRay(origin, magnitude * direction, Color.red);
        }
    }
}
