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
    public GameObject tutorialBoss;
    public Transform attacPos;
    public GameObject finalBoss;
    public AudioClip damageSound;
    void Start()
    {
        pMov = GameObject.Find("Player").GetComponent<PlayerMovement>();
        parry = GameObject.Find("Shield").GetComponent<Parry>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pMov.canParry == true && Input.GetButtonDown("Attack") && parry.parrying == false && Time.timeScale != 0)
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
                if (pMov.pStatus != PlayerMovement.playerState.dashing)
                {

                    gameObject.transform.position = attacPos.position;
                    gameObject.transform.rotation = attacPos.rotation;
                }
                else
                {
                    gameObject.transform.position = new Vector3(0, -30000, 0);

                }
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
            if (other.gameObject.tag == "FinalBoss")
            {
                finalBoss.GetComponent<FinalBoss>().hp--;
                BossManager.SoundEffect(damageSound);
            }
            if (other.gameObject.tag == "SwordWeakPoint")
            {
                Destroy(other.gameObject);
            }
            else if (other.gameObject.tag == "Boss" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "WeakPoint")
            {
                
                if (other.gameObject.tag == "WeakPoint")
                {
                    //Debug.Log("WeakPointHit");
                    tutorialBoss.GetComponent<EnemyHP>().DamageEnemy(1,true);
                    if(tutorialBoss.GetComponent<TutorialBoss>() != null) tutorialBoss.GetComponent<TutorialBoss>().stunTimer += 2.0f;
                    other.gameObject.SetActive(false);
                }
                else other.gameObject.GetComponent<EnemyHP>().DamageEnemy(1,false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "FinalBoss")
            {
                finalBoss.GetComponent<FinalBoss>().hp--;
                BossManager.SoundEffect(damageSound);

            }
            if (collision.gameObject.tag == "SwordWeakPoint")
            {
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Boss" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "WeakPoint")
            {
                if (collision.gameObject.tag == "WeakPoint")
                {
                    //Debug.Log("WeakPointHit");
                    tutorialBoss.GetComponent<EnemyHP>().DamageEnemy(1, true);
                    tutorialBoss.GetComponent<TutorialBoss>().stunTimer += 2.0f;
                    collision.gameObject.SetActive(false);


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
        //gameObject.GetComponent<MeshRenderer>().enabled = active;
        gameObject.GetComponent<BoxCollider>().enabled = active;

    }
}
