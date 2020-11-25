using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameObject m_camera;
    public Health playerHealth;
    int previousPlayerHealth;
    public float spaceBetweenHeartShines;
    float shineTimer;

    //last index should be leftmost
    public List<Animator> heartAnimators;

    //hashes
    readonly int full = Animator.StringToHash("FullHeart");

    readonly int halfDamage = Animator.StringToHash("HalfHeartDamage");
    readonly int half = Animator.StringToHash("HalfHeart");
    readonly int noneDamage = Animator.StringToHash("NoHeartDamage");
    readonly int none = Animator.StringToHash("NoHeart");
    readonly int shine = Animator.StringToHash("Shine");

    // Start is called before the first frame update
    void Start()
    {
        m_camera = GameObject.Find("Main Camera");
        shineTimer = 0f;
    }

    void Update() {
        if (playerHealth.GetHealthPts() != previousPlayerHealth) {
            SetHeartAnimations(playerHealth.GetHealthPts(), previousPlayerHealth);
        }

        if(shineTimer > spaceBetweenHeartShines) {
            shineTimer = 0;
            foreach(Animator a in heartAnimators) {
                a.SetTrigger(shine);
            }
        }

        previousPlayerHealth = playerHealth.GetHealthPts();
        shineTimer += Time.deltaTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector2(m_camera.transform.position.x, m_camera.transform.position.y);
    }

    private void SetHeartAnimations(int health, int prevCount) {
        bool isDamage = health < prevCount;

        for(int i = 1; i <= heartAnimators.Count; i ++) {
            if(health >= 2 * i) {
                heartAnimators[i - 1].Play(full);
            }
            else if(health + 1 == 2 * i) {
                if(isDamage) {
                    heartAnimators[i - 1].Play(halfDamage);
                }
                else {
                    heartAnimators[i - 1].Play(half);
                }
            }
            else {
                if (isDamage && health + 2 == 2 * i) {
                    heartAnimators[i - 1].Play(noneDamage);
                }
                else {
                    heartAnimators[i - 1].Play(none);
                }
            }
        }
    }
}
