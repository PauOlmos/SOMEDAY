using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieByTime : MonoBehaviour
{
    // Start is called before the first frame update
    float timer;
    public float deathTime = 0.0f;
    void Start()
    {
        if (deathTime == 0.0f) deathTime = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > deathTime) Destroy(gameObject);
    }
}
