using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthCapacity = 6;
    public BoxCollider2D hurtBox;

    private TopDownPlayerController controller;

    [SerializeField]
    private int healthPts;

    // Start is called before the first frame update
    void Awake()
    {
        healthPts = healthCapacity;
        controller = gameObject.GetComponent<TopDownPlayerController>();
    }

    public void TakeDamage(int damageLoss) {
        healthPts -= Mathf.Abs(damageLoss);

        if(healthPts < 0) {
            healthPts = 0;
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
            Debug.Log("Damage Taken!");
            TakeDamage(1);
            controller.state = PlayerState.Knockback;
        }
    }

}