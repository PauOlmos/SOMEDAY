using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingProjectile : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    public bool canFail;
    public float seekingTime = 0.5f;
    public float seekTimer;
    public float speed = 1.0f;
    public Vector3 direction;
    public bool shotByPlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            if (canFail)
            {
                seekTimer += Time.deltaTime;
                if(seekTimer < seekingTime) 
                {
                    Seek();
                }
                else
                {
                    transform.position += direction * speed * Time.deltaTime;
                }
            }
            else
            {
                Seek();
            }
        }
    }

    void Seek()
    {
        direction = target.position - transform.position;
        direction.Normalize();

        // Move towards the target
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss" || other.gameObject.tag == "Enemy")
        {
            if (shotByPlayer)
            {
                other.gameObject.GetComponent<EnemyHP>().DamageEnemy(2,false);
                Destroy(gameObject);
            }
        }
        else if(other.gameObject.tag == "Player")
        {
            if (!shotByPlayer)
            {
                other.gameObject.GetComponent<PlayerHp>().TakeDamage();
                Destroy(gameObject);
            }
        }
    }
}
