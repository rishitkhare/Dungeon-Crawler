using System;
using UnityEngine;

public class DirectionArgs : EventArgs {

    public Vector2 direction;

    public DirectionArgs(Vector2 direction) {
        this.direction = direction;
    }
}
