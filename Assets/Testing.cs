using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    public float speed = 10f;
    public bool adjustForFramerate;
    public float accelRate = 0.15f;

    private float velocity;
    private float timer;
    private bool isMoving;

    // Start is called before the first frame update
    void Start() {
        isMoving = true;
    }

    // Update is called once per frame
    void FixedUpdate() {

        float targetVelocity = speed * Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown("space")) {
            transform.position = new Vector3(-8f, transform.position.y);
            timer = 0f;
            velocity = 30f;
            isMoving = true;
        }

        if (isMoving && velocity < 0.1f) {
            Debug.Log($"stopped at {transform.position.x} ({timer} s)");
            isMoving = false;
        }

        if (adjustForFramerate) {
            float timeFactor = Time.fixedDeltaTime * 60f;
            float adjustedDecel = accelRate * (1f - Mathf.Pow(accelRate, timeFactor));

            adjustedDecel /= 1 - accelRate;

            velocity += adjustedDecel * (targetVelocity - velocity);
        }
        else {
            velocity += accelRate * (targetVelocity - velocity);
        }

        transform.position += Time.fixedDeltaTime * (new Vector3(velocity, 0));
        timer += Time.deltaTime;

    }
}
