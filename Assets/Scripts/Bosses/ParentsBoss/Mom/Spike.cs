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
    public AudioClip[] sounds;
    public GameObject soundGO;
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
            if (hp <= 0)
            {
                GameObject sfx = Instantiate(soundGO, gameObject.transform.position, Quaternion.identity);
                sfx.GetComponent<AudioSource>().clip = sounds[1];
                sfx.GetComponent<AudioSource>().Play();
                Destroy(Prefab);
            }
            else
            {
                gameObject.GetComponent<AudioSource>().clip = sounds[0];
                gameObject.GetComponent<AudioSource>().loop = false;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Sword")
        {
            hp--;
            if (hp <= 0)
            {
                GameObject sfx = Instantiate(soundGO, gameObject.transform.position, Quaternion.identity);
                sfx.GetComponent<AudioSource>().clip = sounds[1];
                sfx.GetComponent<AudioSource>().Play();
                Destroy(Prefab);
            }
            else
            {
                gameObject.GetComponent<AudioSource>().clip = sounds[0];
                gameObject.GetComponent<AudioSource>().loop = false;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

}
