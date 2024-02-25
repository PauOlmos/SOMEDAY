using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale += Vector3.one * Time.deltaTime * 3000;
        if (gameObject.transform.localScale.x > 8500.0f) Destroy(gameObject);
    }
}
