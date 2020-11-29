using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthCapacity = 6;
    public BoxCollider2D hurtBox;
    public string hurtTag;

    protected int numberOfHurtColliders;
    protected int healthPts;

    // Start is called before the first frame update
    virtual public void Awake()
    {
        healthPts = healthCapacity;
        numberOfHurtColliders = 0;
    }

    public void Update() {

        if (numberOfHurtColliders > 0) {
            TakeDamage(1);
        }
    }

    virtual public void TakeDamage(int damageLoss) {
        healthPts -= Mathf.Abs(damageLoss);
        if (healthPts <= 0) {
            //call death function
            healthPts = 0;
        }
    }

    public void GainHealth(int gainedHealth) {
        healthPts += Mathf.Abs(gainedHealth);
    }

    public int GetHealthPts() {
        return healthPts;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(hurtTag)) {
            numberOfHurtColliders++;
        }
    }


    void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag(hurtTag)) {
            numberOfHurtColliders--;
        }
    }

}