using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthCapacity = 6;
    public BoxCollider2D hurtBox;

    private TopDownPlayerController controller;

    public int HealthPts { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        HealthPts = healthCapacity;
        controller = gameObject.GetComponent<TopDownPlayerController>();
    }

    public void TakeDamage(int damageLoss) {
        HealthPts -= Mathf.Abs(damageLoss);

        if(HealthPts <= 0) {

        }
    }

    public void GainHealth(int gainedHealth) {
        HealthPts += Mathf.Abs(gainedHealth);
    }

    public int GetHealthPts() {
        return HealthPts;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("TriggerEnter");
        if(collision.CompareTag("PlayerHurt")) {
            TakeDamage(1);
            controller.state = PlayerState.Knockback;
        }
    }

}