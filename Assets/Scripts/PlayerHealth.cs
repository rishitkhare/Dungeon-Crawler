using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    TopDownPlayerController controller;
    Animator playerAnimator;
    public ScreenShake shaker;
    public int damagedScreenShakeFrames = 40;
    public float damagedScreenShakeIntensity = 2f;


    override
    public void Awake() {

        healthPts = healthCapacity;
        numberOfHurtColliders = 0;

        playerAnimator = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<TopDownPlayerController>();
    }

    override
    public void TakeDamage(int damageLoss) {

        if (!controller.isI_frame) {
            healthPts -= Mathf.Abs(damageLoss);
            controller.KnockPlayerBack();
            shaker.ShakeScreen(damagedScreenShakeFrames, damagedScreenShakeIntensity);
        }

        if (healthPts <= 0) {
            //call death function
            healthPts = 0;
        }
    }
}
