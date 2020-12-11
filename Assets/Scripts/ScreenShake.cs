using UnityEngine;
using System;

public class ScreenShake : MonoBehaviour
{
    public int shakeFramerate = 20;

    //singleton implementation
    public static ScreenShake screenShaker;

    //how much to offset camera from 'correct' position each frame
    private Vector2 cameraOffset;

    private float shakeLevel;

    //how many frames until screen stops shaking
    private int shakeTimer;

    void Start() {
        shakeTimer = -1;
        shakeLevel = 0f;
        if(screenShaker != null) {
            throw new ArgumentException("Doubled instantiation of ScreenShake (singleton)");
        }
        screenShaker = this;
    }


    void LateUpdate() {
        int framesPerShake = 60 / shakeFramerate;
            
        if(shakeTimer > 0) {
            if(shakeTimer % framesPerShake == 0) {
                ShakeFrame();
                shakeLevel -= 0.03f;
            }
            shakeTimer--;
        }
        else {
            shakeLevel = 0f;
            cameraOffset = Vector2.zero;
        }

        //apply shakeOffset
        transform.position += (Vector3) cameraOffset;
    }

    void ShakeFrame() {
        cameraOffset = new Vector2(UnityEngine.Random.Range(-shakeLevel, shakeLevel), UnityEngine.Random.Range(-shakeLevel, shakeLevel));
    }

    public void ShakeScreen(int frames, float intensity) {
        shakeTimer = frames;
        shakeLevel = intensity / 16f;
    }


}
