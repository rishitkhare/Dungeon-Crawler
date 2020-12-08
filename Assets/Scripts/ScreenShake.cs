using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public int shakeFramerate = 20;

    //how much to offset camera from 'correct' position each frame
    private Vector3 shakeLevel;

    //how many frames until screen stops shaking
    private int shakeTimer;

    void Start() {
        shakeTimer = -1;
        shakeLevel = Vector3.zero;
    }


    void LateUpdate() {
        int framesPerShake = 60 / shakeFramerate;
            
        if(shakeTimer > 0) {
            if(shakeTimer % framesPerShake == 0) {
                ShakeFrame();
            }
            shakeTimer--;
        }
        else {
            shakeLevel = Vector3.zero;
        }

        //apply shakeOffset
        transform.position += shakeLevel;
    }

    void ShakeFrame() {
        shakeLevel *= -1;
    }

    public void ShakeScreen(int frames, float intensity) {
        shakeTimer = frames;
        shakeLevel = new Vector3(intensity / 16f, 0, 0); //for now, it just shakes horizontally
    }


}
