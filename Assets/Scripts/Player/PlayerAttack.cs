using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private float attackTimer;
    private float attackCooldown = 1.2f;
    private float attackDuration = 1.05f;
    public bool attackActive = true;

    PassiveAbility passiveAbility;
    public bool attacking = false;
    PlayerMovement pMov;
    Parry parry;

    public Transform attacPos;
    void Start()
    {
        pMov = GameObject.Find("Player").GetComponent<PlayerMovement>();
        parry = GameObject.Find("Shield").GetComponent<Parry>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pMov.canParry == true && Input.GetButtonDown("Attack") && parry.parrying == false)
        {
            UseAttack();
        }
        if (attackActive == false)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer > attackDuration)
            {

                SetAttack(false);
                attacking = false;
            }
            else

            {
                pMov.canAttack = false;
                gameObject.transform.position = attacPos.position;
                gameObject.transform.rotation = attacPos.rotation;
            }

            if (attackTimer > attackCooldown)
            {
                attackTimer = 0;
                attackActive = true;
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {

            if (other.gameObject.tag == "Boss" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "WeakPoint")
            {

                if (other.gameObject.tag == "WeakPoint")
                {
                    Debug.Log("WeakPointHit");
                    other.gameObject.GetComponentInParent<EnemyHP>().DamageEnemy(1,true);
                }
                else other.gameObject.GetComponent<EnemyHP>().DamageEnemy(1,false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {

            if (collision.gameObject.tag == "Boss" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "WeakPoint")
            {
                if (collision.gameObject.tag == "WeakPoint")
                {
                    Debug.Log("WeakPointHit");
                    collision.gameObject.GetComponentInParent<EnemyHP>().DamageEnemy(1,true);
                }
                else collision.gameObject.GetComponent<EnemyHP>().DamageEnemy(1,false);
            }
        }
    }


    void UseAttack()
    {
        SetAttack(true);
        attackActive = false;
        attacking = true;
    }

    void SetAttack(bool active)
    {
        if (active == false)
        {
            gameObject.transform.position = new Vector3(0, -30000, 0);
        }
        gameObject.GetComponent<MeshRenderer>().enabled = active;
        gameObject.GetComponent<BoxCollider>().enabled = active;

    }
}
