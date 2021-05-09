using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room {
    public float minXBound;
    public float maxXBound;
    public float minYBound;
    public float maxYBound;

    public bool Contains(Vector2 point) {
        return
            point.x > minXBound &&
            point.x < maxXBound &&
            point.y > minYBound &&
            point.y < maxYBound;
    }
}
