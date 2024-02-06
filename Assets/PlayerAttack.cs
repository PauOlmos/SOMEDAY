using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private float attackTimer;
    private float attackCooldown = 3.0f;
    private float attackDuration = 0.25f;
    private bool attackActive = true;

    PassiveAbility passiveAbility;
    public bool attacking = false;
    PlayerMovement pMov;
    Parry parry;
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
        if (other.gameObject.tag == "Boss" || other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHP>().DamageEnemy(1);
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
        gameObject.GetComponent<MeshRenderer>().enabled = active;
        gameObject.GetComponent<BoxCollider>().enabled = active;

    }
}
