using System;
using UnityEngine;

public class AttackEventArgs : EventArgs {

    public Vector2 direction;
    public float attackEndLag;

    public AttackEventArgs(Vector2 direction, float atkLag) {
        this.direction = direction;
        attackEndLag = atkLag;
    }
}