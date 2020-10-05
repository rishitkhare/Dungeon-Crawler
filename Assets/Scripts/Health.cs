using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthCapacity = 6;
    public BoxCollider2D hurtBox;

    private int healthPts;

    // Start is called before the first frame update
    void Awake()
    {
        healthPts = healthCapacity;
    }

    public void TakeDamage(int damageLoss) {
        healthPts -= Mathf.Abs(damageLoss);

        if(healthPts <= 0) {

        }
    }

    public void GainHealth(int gainedHealth) {
        healthPts += Mathf.Abs(gainedHealth);
    }

    public int GetHealthPts() {
        return healthPts;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("PlayerHurt")) {

        }
    }

}