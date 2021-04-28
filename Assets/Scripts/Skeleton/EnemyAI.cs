using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public class Node {
    public Node origin {get; private set; }
    public Vector3Int position {get; private set; }

    public Node(Node origin, Vector3Int position) {
        this.origin = origin;
        this.position = position;
    }
}

public class EnemyAI : MonoBehaviour {

    Transform playerTransform;
    public float maxWaitTime = 5f;
    public EnemyState stateAfterKnockback;
    public Tilemap colliderTilemap;

    SkeletonController controller;
    BoxCollider2D hitbox;
    float IdleClock = 3f;

    List<Node> path;
    List<Node> frontier;
    List<Node> explored;
    float pathFindTimer = 0f;


    void Awake() {
        controller = gameObject.GetComponent<SkeletonController>();
        hitbox = gameObject.GetComponent<BoxCollider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        path = new List<Node>();
        frontier = new List<Node>();
        explored = new List<Node>();
    }

    void Start() {
        Pathfind();
    }

    void Update() {

        if (pathFindTimer > 1f) {
            Pathfind();
            pathFindTimer = 0f;
        }

        pathFindTimer += Time.deltaTime;

        DebugPath();

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

        DebugPath();
    }

    public Vector2 GetDirection() {
        return Vector2.zero;
    }

    // Breadth First (flood fill) pathfinding algorithm
    private void Pathfind() {
        Vector3Int currentTile = colliderTilemap.WorldToCell(transform.position);
        Vector3Int targetTile = colliderTilemap.WorldToCell(playerTransform.position);

        path.Clear();
        frontier.Clear();
        explored.Clear();

        frontier.Add(new Node(null, currentTile));

        int failsafe = 0;
        bool pathCompleted = false;
        Node finalNode = null;

        while(!pathCompleted) {

            int frontierCount = frontier.Count;
            Debug.Log(frontierCount);

            for(int i = 0; i < frontierCount; i++) {
                Node n = frontier[0];
                frontier.RemoveAt(0);
                explored.Add(n);

                if(n.position == targetTile) {
                    pathCompleted = true;
                    finalNode = n;
                    break;
                }
                else {
                    Vector3Int[] directions = new Vector3Int[4];

                    directions[0] += n.position + Vector3Int.up;
                    directions[1] += n.position + Vector3Int.down;
                    directions[2] += n.position + Vector3Int.left;
                    directions[3] += n.position + Vector3Int.right;

                    float[] heuristics = GenerateHeuristics(n.position);

                    for(int j = 0; j < 4; j++) {
                        if(heuristics[j] != Mathf.Infinity) {
                            frontier.Add(new Node(n, directions[j]));
                        }
                    }
                }
            }

            failsafe++;

            if(failsafe > 100) {
                Debug.Log("Path failed!");

                return;
            }
        }

        path.Add(finalNode);
        Node trackNodePath = finalNode.origin;

        while(path[0].origin != null) {
            path.Insert(0, trackNodePath);
            trackNodePath = trackNodePath.origin;
        }

        frontier.Clear();
        explored.Clear();

    }

    private float[] GenerateHeuristics(Vector3Int from) {
        Vector3Int[] directions = new Vector3Int[4];

        directions[0] += from + Vector3Int.up;
        directions[1] += from + Vector3Int.down;
        directions[2] += from + Vector3Int.left;
        directions[3] += from + Vector3Int.right;

        float[] output = new float[4];

        for(int i = 0; i < 4; i++) {
            if(colliderTilemap.HasTile(directions[i]) || NodeAlreadyExists(directions[i])) {
                output[i] = Mathf.Infinity;
            }
            else {
                output[i] = GetDistanceToPlayer(directions[i]);
                output[i] += GetDistanceToSelf(directions[i]);
            }
        }

        return output;
    }

    private bool NodeAlreadyExists(Vector3Int tile) {

        foreach(Node n in explored) {
            if(tile == n.position) {
                return true;
            }
        }

        foreach(Node n in frontier) {
            if(tile == n.position) {
                return true;
            }
        }

        return false;
    }

    //draws the path in the scene view
    private void DebugPath() {
        for(int i = 0; i < path.Count - 1; i++) {
            Debug.DrawLine(colliderTilemap.GetCellCenterWorld(path[i].position), colliderTilemap.GetCellCenterWorld(path[i + 1].position));
        }
    }

    private void LogFrontier() {
        string output = "";
        foreach(Node n in frontier) {
            output += $"{n.position}\n";
        }
        Debug.Log(output);
    }

    private float GetDistanceToPlayer(Vector3Int otherPoint) {
        Vector3Int playerTile = colliderTilemap.WorldToCell(playerTransform.position);

        return Mathf.Abs(playerTile.x - otherPoint.x) + Mathf.Abs(playerTile.y - otherPoint.y);
    }

    private float GetDistanceToSelf(Vector3Int otherPoint) {
        Vector3Int selfTile = colliderTilemap.WorldToCell(transform.position);

        return Mathf.Abs(selfTile.x - otherPoint.x) + Mathf.Abs(selfTile.y - otherPoint.y);
    }

}
