using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    TopDownPlayerController controller;
    public int damagedScreenShakeFrames = 40;
    public float damagedScreenShakeIntensity = 2f;


    override
    public void Awake() {

        healthPts = healthCapacity;
        numberOfHurtColliders = 0;

        controller = gameObject.GetComponent<TopDownPlayerController>();
    }

    override
    public void TakeDamage(int damageLoss) {

        if (!controller.isI_frame) {
            healthPts -= Mathf.Abs(damageLoss);
            controller.KnockPlayerBack();
            ScreenShake.screenShaker.ShakeScreen(damagedScreenShakeFrames, damagedScreenShakeIntensity);
        }

        if (healthPts <= 0) {
            //call death function
            healthPts = 0;
        }
    }
}
