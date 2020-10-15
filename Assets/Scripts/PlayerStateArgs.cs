using System;

public class PlayerStateArgs : EventArgs {
    public PlayerState state;


    public PlayerStateArgs(PlayerState state) {
        this.state = state;
    }
}