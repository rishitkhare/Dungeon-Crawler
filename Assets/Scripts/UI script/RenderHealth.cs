using UnityEngine;
using UnityEngine.UI;

public class RenderHealth : MonoBehaviour {
    public int heart_num;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite noHeart;

    private Health playerHealth;
    private Image spriteRender;

    // Start is called before the first frame update
    void Start() {
        playerHealth = GameObject.Find("Player").GetComponent<Health>();
        spriteRender = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        int heartStatus = playerHealth.HealthPts - (2 * heart_num);

        if(heartStatus > 1) {
            spriteRender.sprite = fullHeart;
        }
        else if(heartStatus == 1) {
            spriteRender.sprite = halfHeart;
        }
        else {
            spriteRender.sprite = noHeart;
        }
    }
}
