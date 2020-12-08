using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public Transform playerTransform;
    public float maxWaitTime = 5f;
    public EnemyState stateAfterKnockback;

    SkeletonController controller;
    float IdleClock = 3f;

    void Awake() {
        controller = gameObject.GetComponent<SkeletonController>();
    }

    void Update() {
        //cycles between moving and not moving
        if(IdleClock < 0){
            IdleClock = Random.Range(1f, maxWaitTime);
            if (controller.state == EnemyState.Idle){
                controller.state = EnemyState.Walking;
                stateAfterKnockback = EnemyState.Walking;
            }
            else if(controller.state == EnemyState.Walking) {
                controller.state = EnemyState.Idle;
                stateAfterKnockback = EnemyState.Idle;
            }
        }

        IdleClock -= Time.deltaTime;
    }

    //Chooses which direction the enemy will go in based on pathfinding algorithm
    public Vector2 GetDirection() {
        Vector3 newPosition;

        //0 = up, 1 = down, 2 = right, 3 = left
        float[] options = new float[4];

        //see if moving up is a good idea
        newPosition = transform.position + Vector3.up * 0.5f;
        options[0] = GetDistanceToPlayer(newPosition);

        //down
        newPosition = transform.position + Vector3.down * 0.5f;
        options[1] = GetDistanceToPlayer(newPosition);

        //right
        newPosition = transform.position + Vector3.right * 0.5f;
        options[2] = GetDistanceToPlayer(newPosition);

        //left
        newPosition = transform.position + Vector3.left * 0.5f;
        options[3] = GetDistanceToPlayer(newPosition);

        int optionChoice = GetMinIndex(options);

        switch(optionChoice) {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.down;
            case 2:
                return Vector2.right;
            case 3:
                return Vector2.left;
        }

        Debug.Log("EnemyAI error! Did not successfully choose a direction!");
        return Vector2.zero;
    }

    private float GetDistanceToPlayer(Vector3 otherPoint) {
        return Mathf.Abs((otherPoint - playerTransform.position).magnitude);
    }

    private int GetMinIndex(float[] array) {
        int minIndex = 0;

        for(int i = 1; i < array.Length; i ++) {
            if(array[minIndex] > array[i]) {
                minIndex = i;
            }
        }

        return minIndex;
    }
}
