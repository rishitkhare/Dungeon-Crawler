using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthCapacity = 6;
    public BoxCollider2D hurtBox;
    public bool isPlayer;

    private Animator playerAnimator;
    private int numberOfHurtColliders;

    private TopDownPlayerController controller;

    [SerializeField]
    private int healthPts;

    // Start is called before the first frame update
    void Awake()
    {
        healthPts = healthCapacity;
        controller = gameObject.GetComponent<TopDownPlayerController>();
        if(isPlayer) {
            playerAnimator = gameObject.GetComponent<Animator>();
        }
        numberOfHurtColliders = 0;
    }

    void Update() {
        if(numberOfHurtColliders > 0) {
            if(!controller.isI_frame) {
                TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damageLoss) {
        if(!controller.isI_frame) {
            controller.state = PlayerState.Knockback;
            healthPts -= Mathf.Abs(damageLoss);

            if(healthPts < 0) {
                healthPts = 0;
            }
        }
    }

    public void GainHealth(int gainedHealth) {
        healthPts += Mathf.Abs(gainedHealth);
    }

    public int GetHealthPts() {
        return healthPts;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PlayerHurt")) {
            numberOfHurtColliders++;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("PlayerHurt")) {
            numberOfHurtColliders--;
        }
    }

}