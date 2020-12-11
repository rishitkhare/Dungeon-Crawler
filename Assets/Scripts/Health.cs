using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthCapacity = 6;
    public BoxCollider2D hurtBox;
    public string hurtTag;

    protected int numberOfHurtColliders;
    protected Vector3 inflictorPosition;
    protected int healthPts;

    private SkeletonController skellycontroller;

    // Start is called before the first frame update
    virtual public void Awake()
    {
        healthPts = healthCapacity;
        numberOfHurtColliders = 0;
        skellycontroller = gameObject.GetComponent<SkeletonController>();
    }

    public void Update() {

        if (numberOfHurtColliders > 0) {
            TakeDamage(1);
        }
    }

    //only applies to enemies
    virtual public void TakeDamage(int damageLoss) {
        ScreenShake.screenShaker.ShakeScreen(3, 2f);
        healthPts -= Mathf.Abs(damageLoss);
        skellycontroller.Knockback(inflictorPosition);

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

    //TODO: refactor collision detection out of the Health Class
    //honestly I need to refactor this whole class it's a mess
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(hurtTag)) {
            numberOfHurtColliders++;
            inflictorPosition = collision.gameObject.transform.position;
        }
    }


    void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag(hurtTag)) {
            numberOfHurtColliders--;
        }
    }

}