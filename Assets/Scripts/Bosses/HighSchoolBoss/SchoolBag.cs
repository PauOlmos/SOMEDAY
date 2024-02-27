using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolBag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 3) Destroy(gameObject);
    }
}
