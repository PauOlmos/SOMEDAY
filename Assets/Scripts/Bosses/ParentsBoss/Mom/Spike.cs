using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public GameObject shadow;
    public float timer = 0.0f;
    public GameObject Prefab;
    public int hp = 2;
    public LayerMask nothing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer < 4.0)
        {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime);
        }
        else
        {
            gameObject.GetComponent<CapsuleCollider>().excludeLayers = nothing;
            //gameObject.isStatic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            hp--;
            Destroy(collision.gameObject);
            if (hp <= 0) Destroy(Prefab);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Sword")
        {
            hp--;
            if (hp <= 0) Destroy(Prefab);
        }
    }

}
