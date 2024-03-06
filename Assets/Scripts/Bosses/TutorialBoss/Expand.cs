using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand : MonoBehaviour
{
    public float expandMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale += Vector3.one * Time.deltaTime * 3;
        if (gameObject.transform.localScale.x > 8.5f) Destroy(gameObject);
    }
}
