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

    //the skeleton will periodically check for when to do path updates
    float pathTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<SimpleRigidbody>();
        pathTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {


        pathTimer += Time.deltaTime;
    }
}
