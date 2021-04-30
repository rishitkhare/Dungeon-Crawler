using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public class Node : IComparable<Node> {
    public Node Origin {get; private set; }
    public Vector3Int Position {get; private set; }
    public int HCost { get; private set; }
    public int GCost { get; private set; }
    public int TCost { get; private set; }

    public Node(Node origin, Vector3Int position, int gCost) {
        this.Origin = origin;
        this.Position = position;
        this.GCost = gCost;

        if(origin == null) {
            HCost = 0;
        }
        else {
            HCost = origin.HCost + 1;
        }

        TCost = HCost + GCost;
    }

    public int CompareTo(Node n) {
        int costDiff = this.TCost - n.TCost;

        if(costDiff == 0f) {
            return this.HCost - n.HCost;
        }
        else {
            return costDiff;
        }
    }
}

public class EnemyAI : MonoBehaviour {

    public float maxIdleTime = 5f;
    public float maxWalkTime = 8f;
    public EnemyState stateAfterKnockback;
    public Tilemap colliderTilemap;
    public Tilemap pathfindingVisual;
    public Tile frontierTile;
    public Tile exploredTile;
    public Tile pathTile;

    Vector3 hitboxPosition;
    Vector3 playerPosition;

    SkeletonController controller;
    BoxCollider2D hitbox;
    BoxCollider2D playerHitbox;
    float IdleClock = 3f;

    List<Node> path;
    List<Node> frontier;
    List<Node> explored;
    float pathFindTimer;
    public float pathFindWaitTime = 1f;

    private IEnumerator coroutine;

    void Awake() {
        controller = gameObject.GetComponent<SkeletonController>();
        hitbox = gameObject.GetComponent<BoxCollider2D>();
        playerHitbox = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();

        path = new List<Node>();
        frontier = new List<Node>();
        explored = new List<Node>();

        coroutine = PathFindCoroutine();
        pathFindTimer = pathFindWaitTime;
    }

    void Update() {
        hitboxPosition = transform.position + (Vector3) hitbox.offset;
        playerPosition = playerHitbox.transform.position + (Vector3)playerHitbox.offset;

        if (pathFindTimer > pathFindWaitTime) {
            //Pathfind();
            Init_PathFindCoroutine();
            pathFindTimer = 0f;
        }

        pathFindTimer += Time.deltaTime;

        DebugPath();

        //cycles between moving and not moving
        if(IdleClock < 0){
            if (controller.state == EnemyState.Idle){
                controller.state = EnemyState.Walking;
                stateAfterKnockback = EnemyState.Walking;
                IdleClock = UnityEngine.Random.Range(3f, maxWalkTime);
                Debug.Log($"Walking for {IdleClock} seconds");
            }
            else if(controller.state == EnemyState.Walking) {
                controller.state = EnemyState.Idle;
                stateAfterKnockback = EnemyState.Idle;
                IdleClock = UnityEngine.Random.Range(1f, maxIdleTime);
                Debug.Log($"Waiting for {IdleClock} seconds");
            }
        }

        if(controller.state != EnemyState.Knockback) {
            IdleClock -= Time.deltaTime;
        }

        DebugPath();
    }

    public Vector2 GetDirection() {
        if(path.Count <= 2) {
            return (playerPosition - transform.position).normalized;
        }
        else {
            Vector3 secondNodePosition = colliderTilemap.GetCellCenterWorld(path[1].Position);

            float distanceToSecondNode = (hitboxPosition - secondNodePosition).sqrMagnitude;

            if (distanceToSecondNode < 1.1f) {
                path.RemoveAt(0);
                //transform.position = firstNodePosition - (Vector3) hitbox.offset;
            }

            Vector3 firstNodePosition = colliderTilemap.GetCellCenterWorld(path[0].Position);

            Vector2 response = (firstNodePosition - hitboxPosition).normalized;

            Debug.DrawRay(hitboxPosition, (firstNodePosition - hitboxPosition).normalized, Color.green);
            Debug.DrawLine(hitboxPosition, firstNodePosition, Color.blue);
            return response;
        }
    }

    private void Init_PathFindCoroutine() {
        StopCoroutine(coroutine);
        coroutine = PathFindCoroutine();
        StartCoroutine(coroutine);

    }

    private IEnumerator PathFindCoroutine() {
        Vector3Int currentTile = colliderTilemap.WorldToCell(hitboxPosition);
        Vector3Int targetTile = colliderTilemap.WorldToCell(playerPosition);

        path.Clear();
        frontier.Clear();
        explored.Clear();

        if (currentTile == targetTile) {
            // You've already made it! No need for pathfinding
            yield break;
        }

        frontier.Add(new Node(null, currentTile, 0));

        int failsafe = 0;
        Node finalNode = null;

        while (true) {

            //If you delete this line, it will become breadth search
            frontier.Sort();

            Node n = frontier[0];
            frontier.RemoveAt(0);
            explored.Add(n);

            if (n.Position == targetTile) {
                finalNode = n;
                break;
            }
            else {
                Vector3Int[] directions = new Vector3Int[4];

                directions[0] += n.Position + Vector3Int.up;
                directions[1] += n.Position + Vector3Int.down;
                directions[2] += n.Position + Vector3Int.left;
                directions[3] += n.Position + Vector3Int.right;

                bool[] availableDirections = IsDirectionsAllowed(directions, n);

                for (int j = 0; j < 4; j++) {
                    if (availableDirections[j]) {
                        frontier.Add(new Node(n, directions[j], GetDistanceToPlayer(directions[j])));
                    }
                }
            }

            failsafe++;

            if (failsafe > 400) {
                Debug.Log("Path failed!");
                GeneratePathfindVisuals();
                yield break;
            }

            //GeneratePathfindVisuals();
            //yield return null;
        }

        path.Add(finalNode);
        Node trackNodePath = finalNode.Origin;

        while (path[0].Origin != null) {
            path.Insert(0, trackNodePath);
            trackNodePath = trackNodePath.Origin;
        }

        GeneratePathfindVisuals();

        frontier.Clear();
        explored.Clear();

    }

    private bool[] IsDirectionsAllowed(Vector3Int[] directions, Node origin) {

        bool[] output = new bool[4];

        for(int i = 0; i < directions.Length; i++) {
            if(colliderTilemap.HasTile(directions[i]) || NodeIsExplored(directions[i])
                || CompareCostsOfFrontier(origin, directions[i])) {

                output[i] = false;
            }
            else {
                output[i] = true;
            }
        }

        return output;
    }

    // If we are adding a node to the frontier that's already there, we check
    // whether or not the newly determined route is cheaper and replace
    private bool CompareCostsOfFrontier(Node origin, Vector3Int position) {
        foreach(Node n in frontier) {
            if(n.Position == position && n.HCost - 1 > origin.HCost) {
                frontier.Remove(n);

                frontier.Add(new Node(origin, position, GetDistanceToPlayer(position)));

                return true;
            }
        }

        return false;
    }

    private bool NodeIsExplored(Vector3Int tile) {

        foreach(Node n in explored) {
            if(tile == n.Position) {
                return true;
            }
        }
        foreach (Node n in frontier) {
            if (tile == n.Position) {
                return true;
            }
        }

        return false;
    }

    //draws the path in the scene view
    private void DebugPath() {
        for(int i = 0; i < path.Count - 1; i++) {
            Debug.DrawLine(colliderTilemap.GetCellCenterWorld(path[i].Position), colliderTilemap.GetCellCenterWorld(path[i + 1].Position));
        }
    }

    private int GetDistanceToPlayer(Vector3Int otherPoint) {
        Vector3Int playerTile = colliderTilemap.WorldToCell(playerPosition);

        return Mathf.Abs(playerTile.x - otherPoint.x) + Mathf.Abs(playerTile.y - otherPoint.y);
    }

    private void GeneratePathfindVisuals() {
        pathfindingVisual.ClearAllTiles();

        foreach(Node n in explored) {
            pathfindingVisual.SetTile(n.Position, exploredTile);
        }
        foreach (Node n in frontier) {
            pathfindingVisual.SetTile(n.Position, frontierTile);
        }
        foreach(Node n in path) {
            pathfindingVisual.SetTile(n.Position, pathTile);
        }
    }
}
