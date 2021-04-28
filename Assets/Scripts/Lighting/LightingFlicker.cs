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
        lightComponent.color = sinCalc.color;
        //lightComponent. = sinCalc.sortingLayers;
    }

    // Update is called once per frame
    public void Update() {
        if(IsOnScreen()) {
            if (!lightComponent.enabled) {
                lightComponent.enabled = true;
            }
            FlickerLights();
        }
        else {
            if(lightComponent.enabled) {
                lightComponent.enabled = false;
            }
        }
    }

    private void FlickerLights() {

        lightComponent.pointLightInnerRadius = sinCalc.innerRadius + sinCalc.GetSinVariation();
        lightComponent.pointLightOuterRadius = sinCalc.outerRadius + sinCalc.GetSinVariation();
        /*
        Vector2 newPosition = transform.position;

        
        if (isVerticalFlicker) {
            newPosition.y = initialPosition + sinCalc.GetSinVariation();
        }
        else {
            newPosition.x = initialPosition + sinCalc.GetSinVariation();
        }
        
        transform.position = newPosition;
        */
    }


    //culling the lights
    private bool IsOnScreen() {
        Vector3 lightPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);



        return !(lightPosition.x <= 0 || lightPosition.x >= 1 || lightPosition.y >= 1 || lightPosition.y <= 0);
    }
}
