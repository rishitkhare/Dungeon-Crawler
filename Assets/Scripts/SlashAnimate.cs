using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAnimate : MonoBehaviour
{

    TopDownPlayerAttack attackComponent;

    // Start is called before the first frame update
    void Start()
    {
        attackComponent = transform.parent.GetComponentInParent<TopDownPlayerAttack>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
