using System;
using UnityEngine;

public class RoomTransitionMovement : MonoBehaviour
{
    public event EventHandler OnRoomTransitionEnter;
    public event EventHandler OnRoomTransitionExit;

    bool roomTransitionTrigger;

    public int indexX;
    public int indexY;
    public bool isSmoothMovement = false;
    public float smoothMovementSpeed = 4.0f;
    public float smoothMinSpeed = 4.0f;
    public float cameraSpeed = 14f;

    public float minimumFloatSpeed;

    public float roomSizeX = 18;
    public float roomSizeY = 10;
    GameObject player;
    SimpleRigidbody playerRb;

    void Start() {
        indexX = 0;
        indexY = 0;

        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<SimpleRigidbody>();
        roomTransitionTrigger = false;
    }

    private void Update() {
        SetGridIndexes();

        if(isSmoothMovement) {
            MoveCameraSmooth();
        }
        else {
            MoveCamera();
        }
        SnapCameraToRoom(0.1f);

        //lock player's motion if camera is moving (for the NES feel)
        if(CameraNeedsToMove()) {
            
            playerRb.LockMovement();

            if(!roomTransitionTrigger) {
                OnRoomTransitionEnter?.Invoke(this, EventArgs.Empty);
                roomTransitionTrigger = true;
            }
        }
        else {
            if(roomTransitionTrigger) {
                OnRoomTransitionExit?.Invoke(this, EventArgs.Empty);
            }
            roomTransitionTrigger = false;
        }
    }

    private void SetGridIndexes() {
        // calculate grid
        indexX = (int)Mathf.Floor((player.transform.position.x + (roomSizeX / 2f)) / roomSizeX);
        indexY = (int)Mathf.Floor((player.transform.position.y + (roomSizeY / 2f)) / roomSizeY);
    }

    private void SnapCameraToRoom(float leeway) {
        //snaps camera to correct position if slightly off to prevent overshooting the movement
        if (Mathf.Abs(transform.position.x - indexX * roomSizeX) < leeway) {
            transform.position = new Vector3(indexX * roomSizeX, transform.position.y, -10);
        }

        if (Mathf.Abs(transform.position.y - indexY * roomSizeY) < leeway) {
            transform.position = new Vector3(transform.position.x, indexY * roomSizeY, -10);
        }
    }

    private void MoveCamera() {
        // move camera if necessary
        Vector3 deltaTransform = new Vector3(0, 0);


        // X
        if (transform.position.x > indexX * roomSizeX) {
            deltaTransform += new Vector3(-cameraSpeed * Time.deltaTime, 0);
        }
        else if (transform.position.x < indexX * roomSizeX) {
            deltaTransform += new Vector3(cameraSpeed * Time.deltaTime, 0);
        }

        // Y
        if (transform.position.y > indexY * roomSizeY) {
            deltaTransform += new Vector3(0, -cameraSpeed * Time.deltaTime);
        }
        else if (transform.position.y < indexY * roomSizeY) {
            deltaTransform += new Vector3(0, cameraSpeed * Time.deltaTime);
        }

        transform.position += deltaTransform;
    }

    private void MoveCameraSmooth() {
        // move camera if necessary
        Vector3 deltaTransform = new Vector3(0, 0);
        float Xdiff = indexX * roomSizeX - transform.position.x;
        float Ydiff = indexY * roomSizeY - transform.position.y;

        // X
        if (transform.position.x != indexX * roomSizeX) {
            deltaTransform += new Vector3((Xdiff + smoothMinSpeed) * Time.deltaTime, 0);
        }

        // Y
        if (transform.position.y != indexY * roomSizeY) {
            deltaTransform += new Vector3(0, (Ydiff +smoothMinSpeed) * Time.deltaTime);
        }

        deltaTransform *= smoothMovementSpeed;

        //prevents from going too slow
        if (deltaTransform.magnitude < minimumFloatSpeed) {
            deltaTransform.Normalize();
            deltaTransform *= minimumFloatSpeed;
        }

        transform.position += deltaTransform;
    }

    private bool CameraNeedsToMove() {
        return ! ( (transform.position.x == indexX * roomSizeX) && (transform.position.y == indexY * roomSizeY));
    }
}
