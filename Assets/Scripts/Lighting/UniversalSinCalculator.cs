using UnityEngine;

public class UniversalSinCalculator : MonoBehaviour
{
    public float amplitude = 0.04f;
    public float speed = 0.1f;
    public float innerRadius = 0.33f;
    public float outerRadius = 3.47f;
    public float intensity = 1.59f;
    public SortingLayer[] sortingLayers; 
    public Color color;

    private float sinVariation;
    private float counter;

    // Start is called before the first frame update
    void Start() {
        counter = 0;
    }

    public float GetSinVariation() {
        return sinVariation;
    }

    // Update is called once per frame
    void Update() {
        sinVariation = amplitude * Mathf.Sin(counter);

        counter += 60f * Time.deltaTime * speed;
    }
}
