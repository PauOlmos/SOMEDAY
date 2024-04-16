using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bossProjectile;
    public Transform target;

    public float timer;
    public float speed = 30;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 0.75f)
        {
            //target.position = gameObject.transform.position + gameObject.transform.right * 100;
            GameObject projectile = Instantiate(bossProjectile, gameObject.transform.position + transform.up, Quaternion.identity);
            projectile.AddComponent<SeekingProjectile>();
            projectile.GetComponent<SeekingProjectile>().canFail = true;
            projectile.GetComponent<SeekingProjectile>().shotByPlayer = false;
            projectile.GetComponent<SeekingProjectile>().seekingTime = 0.1f;
            projectile.GetComponent<SeekingProjectile>().target = target;
            projectile.GetComponent<SeekingProjectile>().speed = speed;
            projectile.tag = "BasicProjectile";
            projectile.layer = 7;
            projectile.AddComponent<DieByTime>();
            projectile.GetComponent<DieByTime>().deathTime = 5.0f;
            timer = 0.0f;
        }

    }
}
