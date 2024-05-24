using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform shotBy;
    public Transform target;
    public bool canFail;
    public float seekingTime = 0.5f;
    public float seekTimer;
    public float speed = 1.0f;
    public Vector3 direction;
    public bool shotByPlayer;
    Vector3 posAux;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1000);
            if (canFail)
            {
                seekTimer += Time.deltaTime;
                if(seekTimer < seekingTime) 
                {
                    gameObject.transform.LookAt(target);
                    Seek(target.position);
                    posAux = (target.transform.position - transform.position) * 1000.0f;
                }
                else
                {
                    gameObject.transform.LookAt(posAux);
                    Seek(posAux);

                }
            }
            else
            {
                gameObject.transform.LookAt(gameObject.transform.forward);
                Seek(target.position);

            }
        }
    }

    void Seek(Vector3 target)
    {
        direction = target - transform.position;
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
                other.gameObject.GetComponent<EnemyHP>().DamageEnemy(2, true);
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.tag == "Player")
        {
            if (!shotByPlayer)
            {
                other.gameObject.GetComponent<PlayerHp>().TakeDamage();
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.layer == 6 || other.gameObject.layer == 3)
        {
            if(!shotByPlayer) Destroy(gameObject);
            
        }
        if (other.gameObject.tag == "DestroyProjectiles") Destroy(gameObject);
    }
}
