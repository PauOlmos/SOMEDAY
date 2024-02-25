using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieByTime : MonoBehaviour
{
    // Start is called before the first frame update
    float timer;
    float deathTime = 5.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > deathTime ) Destroy(gameObject);
    }
}
