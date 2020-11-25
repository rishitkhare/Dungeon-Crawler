using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownLayer : MonoBehaviour
{
    private SpriteRenderer spRenderer;
    public int layeringOffset;

    // Start is called before the first frame update
    void Start()
    {
        spRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spRenderer.sortingOrder = (int) ( -1 * transform.position.y * 16) + layeringOffset;
    }
}
