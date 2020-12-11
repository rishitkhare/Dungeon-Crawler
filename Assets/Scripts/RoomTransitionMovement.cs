using System;
using UnityEngine;

public class RoomTransitionMovement : MonoBehaviour
{
    public event EventHandler OnRoomTransitionEnter;
    public event EventHandler OnRoomTransitionExit;

    public static RoomTransitionMovement RoomSystem;

    bool roomTransitionTrigger;

    public Vector2Int roomIndexes;
    public bool isSmoothMovement = false;
    public float smoothMovementSpeed = 4.0f;
    public float smoothMinSpeed = 0.05f;
    public float cameraSpeed = 14f;

    public float cameraOffsetX = 0f;
    public float cameraOffsetY = 0f;
    public float roomSizeX = 18;
    public float roomSizeY = 10;


    //used to ignore any other camera effects
    private Vector3 previousPosition;

    GameObject player;

    void Awake() {
        if(RoomSystem = null) {
            throw new System.ArgumentException("doubled instantiation of RoomTransitionMovement (singleton)");
        }
        RoomSystem = this;
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        SetGridIndexes();
        previousPosition = transform.position;
        roomTransitionTrigger = false;
    }

    private void Update() {
        //realigns camera after effects
        transform.position = previousPosition;

        SetGridIndexes();

        if(isSmoothMovement) {
            MoveCameraSmooth();
        }
        else {
            MoveCamera();
        }
        SnapCameraToRoom(smoothMinSpeed);

        //lock player's motion if camera is moving (for the NES feel)
        if(CameraNeedsToMove()) {

            if(!roomTransitionTrigger) {
                OnRoomTransitionEnter?.Invoke(this, EventArgs.Empty);
                roomTransitionTrigger = true;
            }
        }
        else {
            if(roomTransitionTrigger) {
                //invoke this only once with trigger
                OnRoomTransitionExit?.Invoke(this, EventArgs.Empty);
            }
            roomTransitionTrigger = false;
        }

        previousPosition = transform.position;
    }

    private void SetGridIndexes() {
        roomIndexes = RoomIndex(player.transform.position);
    }

    public static Vector2Int RoomIndex(Vector2 position) {
        // calculate grid
        return new Vector2Int {
            x = (int)Mathf.Floor((position.x - RoomSystem.cameraOffsetX + (RoomSystem.roomSizeX / 2f)) / RoomSystem.roomSizeX),
            y = (int)Mathf.Floor((position.y - RoomSystem.cameraOffsetY + (RoomSystem.roomSizeY / 2f)) / RoomSystem.roomSizeY)
        };
    }

    private void SnapCameraToRoom(float leeway) {
        //snaps camera to correct position if slightly off to prevent overshooting the movement
        if (Mathf.Abs(transform.position.x - (roomIndexes.x * roomSizeX + cameraOffsetX)) < leeway) {
            transform.position = new Vector3(roomIndexes.x * roomSizeX + cameraOffsetX, transform.position.y, -10);
        }

        if (Mathf.Abs(transform.position.y - (roomIndexes.y * roomSizeY + cameraOffsetY)) < leeway) {
            transform.position = new Vector3(transform.position.x, roomIndexes.y * roomSizeY + cameraOffsetY, -10);
        }
    }

    private void MoveCamera() {
        // move camera if necessary
        Vector3 deltaTransform = new Vector3(0, 0);


        // X
        if (transform.position.x > roomIndexes.x * roomSizeX + cameraOffsetX) {
            deltaTransform += new Vector3(-cameraSpeed * Time.deltaTime, 0);
        }
        else if (transform.position.x < roomIndexes.x * roomSizeX + cameraOffsetX) {
            deltaTransform += new Vector3(cameraSpeed * Time.deltaTime, 0);
        }

        // Y
        if (transform.position.y > roomIndexes.y * roomSizeY + cameraOffsetY) {
            deltaTransform += new Vector3(0, -cameraSpeed * Time.deltaTime);
        }
        else if (transform.position.y < roomIndexes.y * roomSizeY + cameraOffsetY) {
            deltaTransform += new Vector3(0, cameraSpeed * Time.deltaTime);
        }

        transform.position += deltaTransform;
    }

    private void MoveCameraSmooth() {
        // move camera if necessary
        Vector3 deltaTransform = new Vector3(0, 0);
        float Xdiff = roomIndexes.x * roomSizeX + cameraOffsetX - transform.position.x;
        float Ydiff = roomIndexes.y * roomSizeY + cameraOffsetY - transform.position.y;

        // X
        if (transform.position.x != roomIndexes.x * roomSizeX + cameraOffsetX) {
            deltaTransform += new Vector3((Xdiff) * Time.deltaTime, 0);
        }

        // Y
        if (transform.position.y != roomIndexes.y * roomSizeY + cameraOffsetY) {
            deltaTransform += new Vector3(0, (Ydiff) * Time.deltaTime);
        }

        deltaTransform *= smoothMovementSpeed;

        //prevents from going too slow
        if (deltaTransform.magnitude < smoothMinSpeed) {
            deltaTransform.Normalize();
            deltaTransform *= smoothMinSpeed;
        }

        transform.position += deltaTransform;
    }

    private bool CameraNeedsToMove() {
        return ! ( (transform.position.x == roomIndexes.x * roomSizeX + cameraOffsetX) && (transform.position.y == roomIndexes.y * roomSizeY + cameraOffsetY));
    }
}
