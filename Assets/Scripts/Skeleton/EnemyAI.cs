using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{

    public Transform playerTransform;
    public float maxWaitTime = 5f;
    public EnemyState stateAfterKnockback;
    public LayerMask collidableLayer;

    private List<Vector2Int> previousNodes;
    private Vector2Int currentNode;

    SkeletonController controller;
    BoxCollider2D hitbox;
    float IdleClock = 3f;

    void Awake() {
        controller = gameObject.GetComponent<SkeletonController>();
        hitbox = gameObject.GetComponent<BoxCollider2D>();
        transform.position = new Vector3((int) transform.position.x, (int) transform.position.y, transform.position.z);
        previousNodes = new List<Vector2Int>();
        currentNode = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        previousNodes.Add(currentNode);
    }

    void Update() {
        //cycles between moving and not moving
        if(IdleClock < 0){
            IdleClock = UnityEngine.Random.Range(1f, maxWaitTime);
            if (controller.state == EnemyState.Idle){
                controller.state = EnemyState.Walking;
                stateAfterKnockback = EnemyState.Walking;
            }
            else if(controller.state == EnemyState.Walking) {
                controller.state = EnemyState.Idle;
                stateAfterKnockback = EnemyState.Idle;
            }
        }

        if(controller.state != EnemyState.Knockback) {
            IdleClock -= Time.deltaTime;
        }
    }

    //Chooses which direction the enemy will go in based on pathfinding algorithm
    public Vector2 GetDirection() {
        if(IsAtNode(currentNode)) {
            FindNextNode();
        }

        return DirectionOfNextNode();
    }

    private void FindNextNode() {
        //update the list of previous nodes
        previousNodes.Insert(0, currentNode);
        while(previousNodes.Count > 2) {
            previousNodes.RemoveAt(previousNodes.Count - 1);
        }

        //list all options
        List<Vector2Int> nodeOptions = new List<Vector2Int> {
            currentNode + Vector2Int.up,
            currentNode + Vector2Int.down,
            currentNode + Vector2Int.right,
            currentNode + Vector2Int.left };

        //eliminate directions blocked by an obstacle
        //also avoid going over repeat nodes
        List<Vector2Int> realNodeOptions = new List<Vector2Int>();
        foreach(Vector2Int nodeOption in nodeOptions) {
            if(!ColliderIsPresent(nodeOption) && !previousNodes.Contains(nodeOption)) {
                realNodeOptions.Add(nodeOption);
            }
        }
        //reference semantics will take care of this I hope
        nodeOptions = realNodeOptions;

        //calculate costs of remaining options and select minimum (fencepost)
        Vector2Int chosenNode = nodeOptions[0];

        float minimumCost = GetDistanceToPlayer(nodeOptions[0]);
        for(int i = 1; i < nodeOptions.Count; i++) {
            float iCost = GetDistanceToPlayer(nodeOptions[i]);
            if(iCost < minimumCost) {
                minimumCost = iCost;
                chosenNode = nodeOptions[i];
            }
        }

        //set chosen node as new target
        currentNode = chosenNode;
    }

    private bool IsAtNode(Vector2Int node) {
        return Mathf.Abs(transform.position.y - node.y) < 0.02f
            && Mathf.Abs(transform.position.x - node.x) < 0.02f;
    }

    private Vector2 DirectionOfNextNode() {
        Vector2 response = currentNode - (Vector2) transform.position;
        response.Normalize();

        return response;
    }

    private float GetDistanceToPlayer(Vector2 otherPoint) {
        return Mathf.Abs((otherPoint - (Vector2) playerTransform.position).magnitude);
    }

    private bool ColliderIsPresent(Vector2Int nextNode) {
        Vector2 colliderCenter = (Vector2) transform.position + hitbox.offset;
        Vector2 difference = nextNode - (Vector2)transform.position;
        float rayLength = difference.magnitude;
        difference.Normalize();

        RaycastHit2D hit =
            Physics2D.Raycast(colliderCenter, difference, rayLength, collidableLayer);
        if(hit) {
            Debug.DrawRay(transform.position, rayLength * difference, Color.green);
        } else {
            Debug.DrawRay(transform.position, rayLength * difference, Color.red);
        }
        return hit;
    }

}
