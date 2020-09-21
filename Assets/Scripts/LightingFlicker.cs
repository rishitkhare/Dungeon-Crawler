using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class LightingFlicker : MonoBehaviour {
    public bool isVerticalFlicker = false;

    private float initialPosition;
    GameObject gameManager;
    UniversalSinCalculator sinCalc;
    Light2D lightComponent;

    public void Awake() {
        gameManager = GameObject.Find("GameManager");
        lightComponent = gameObject.GetComponent<Light2D>();
    }

    // Start is called before the first frame update
    public void Start() {
        if(isVerticalFlicker) {
            initialPosition = transform.position.y;
        }
        else {
            initialPosition = transform.position.x;
        }

        sinCalc = gameManager.GetComponent<UniversalSinCalculator>();

        lightComponent.pointLightInnerRadius = sinCalc.innerRadius;
        lightComponent.pointLightOuterRadius = sinCalc.outerRadius;
        lightComponent.intensity = sinCalc.intensity;
    }

    // Update is called once per frame
    public void Update() {

        Vector2 newPosition = transform.position;

        if(isVerticalFlicker) {
            newPosition.y = initialPosition + sinCalc.GetSinVariation();
        }
        else {
            newPosition.x = initialPosition + sinCalc.GetSinVariation();
        }

        transform.position = newPosition;
    }
}
